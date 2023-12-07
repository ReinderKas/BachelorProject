using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;
using Microsoft.Z3;
using System.Data;
using Z3Parser.Elfsquad.Archer.Elfsquad.Core.Archer;
using Z3Parser.Z3Constraints;

namespace Z3Parser.FeatureModels
{
    /*  This class implements components of the ArcherSolver.cs class of Elfsquad.
     *  
     *  
     *  Currently implemented in this project:
     *    - Mandatory
     *    - Optional
     *    - Alternative
     *    - Or
     *    
     *    - Excludes
     *    - Requires
     */

    public class Z3Solver
    {
        public Context Z3Context;
        public Solver Solver;

        public FeatureModelGraph FmGraph;
        public ArcherModel[] ArcherModel;

        public NodeProperty[] Variables;
        public ArcherVariableStore ArcherVariableStore;

        public Expr Proof;
        public Expr[] UnsatCore;

        public HashSet<AConstraint> Z3Constraints;

        public Z3Solver()
        {
            Z3Context = new Context(new Dictionary<string, string>
                        {
                            { "unsat_core", "true" },  // enable generation of unsat cores
                            { "model", "true" },        // enable model generation
                            { "proof", "true" },        // enable proof generation
                            { "timeout", "60000" },     // set timout for solving
                        });

            ArcherVariableStore = new(this);
            Z3Constraints = new();

            /*  Elfsquad uses Expressions.
             *  These are Language Specific to the company.
             *  It's easiest to create all this without the Expressions first!.
             */
        }

        public bool Solve()
        {
            // Have to Initialize new Solver here if FastDiag is to be used!
            InitializeZ3Solver();


            int counter = 0;

            foreach(var constraint in Z3Constraints.Select(c =>c.Expression))
            {
                try
                {
                    counter++;
                    //Console.WriteLine($"({counter}/{Z3Constraints.Count()}) {constraint}");
                    Solver.AssertAndTrack(constraint, constraint);
                }
                catch (Z3Exception ex) 
                {
                    //Console.WriteLine($"(Z3) Something went wrong for {constraint}: {ex.Message}");
                }
                catch (Exception ex) 
                {
                    Console.WriteLine($"Something went wrong for {constraint}: {ex.Message}");
                }
            }

            var solution = Solver.Check();
            if (solution == Status.SATISFIABLE)
                return true;

            Proof = Solver.Proof;
            UnsatCore = Solver.UnsatCore;

            return false;
        }

        public async Task InitializeFeatureModelGraph(string model)
        {
            ArcherModel = new ArcherTranslator().Parse(model);
            FmGraph = await new ArcherModelTranslator().BuildGraphAsync(ArcherModel[0]);

            if (FmGraph == null)
                throw new Exception("Could not create Feature Model Graph instance. Please validate the model: " + model);
        }

        public void InitializeZ3Solver()
        {
            InitializeSolvers();
            InitializeVariables();
<<<<<<< HEAD
            InitializeBooleanConstraints();
=======

            if (!Z3Constraints.Any())
                InitializeConstraints();
            
>>>>>>> d9e4fa450c45705edac18f0f97751a17644afad7
            InitializeRequirements();

        }

        private void InitializeSolvers()
        {
            Solver = Z3Context.MkSolver();
        }

        private void InitializeRequirements()
        {
            // TODO: For now only a Requirement on the Root.

            var root = ArcherVariableStore.GetExpression(SelectedProperty(FmGraph.GetRoot())) as BoolExpr;

            Z3Constraints.Add( 
                new UserRequirement(
                    Z3Context.MkEq(root, Z3Context.MkTrue())
                ));
        }

        private void InitializeVariables()
        {
            Variables = FmGraph.TopologicalSortNodes().Reverse().ToArray();
        }

        private void InitializeBooleanConstraints()
        {
            foreach(var variable in Variables)
            {
                if (variable.Property == "Selected") continue;

                var relationship = FmGraph.GetRelationshipConstraint(variable.Node);
                if (relationship == null) continue;


                // Initialize the Hierarchical model.
                if (relationship is AlternativeFeatureModelConstraint or OrFeatureModelConstraint)
                    InitializeNaryConstraint(relationship);
                else
                    InitializeBinaryConstraint(relationship);



                // Initialize the Non-Cross Tree Constraints.
                foreach (var crossTreeRelationships in FmGraph.GetConstraints(variable.Node.NodeId).OfType<BinaryRelationshipConstraint>())
                {
                    InitializeBinaryConstraint(crossTreeRelationships);
                }
            }
        }

        private void InitializeNaryConstraint(IRelationshipConstraint relationship)
        {
            var parent = ArcherVariableStore.GetExpression(SelectedProperty(relationship.FromNode)) as BoolExpr;
            var children = new List<BoolExpr>();

            foreach (var child in relationship.ToNodes())
            {
                var selectedProp = SelectedProperty(child);
                var childExpr = ArcherVariableStore.GetExpression(selectedProp) as BoolExpr;

                children.Add(childExpr);
            }

            Z3Constraints.Add(
                relationship switch
                {
                    AlternativeFeatureModelConstraint => InitAlternative(parent, children.ToArray()),
                    OrFeatureModelConstraint => InitOr(parent, children.ToArray()),
                    _ => throw new NotImplementedException($"Type {relationship.GetType()} is not suported")
                });
        }

        private void InitializeBinaryConstraint(IRelationshipConstraint relationship)
        {
            var parent = ArcherVariableStore.GetExpression(SelectedProperty(relationship.FromNode)) as BoolExpr;
            var child = ArcherVariableStore.GetExpression(SelectedProperty(relationship.ToNodes().First())) as BoolExpr;

            Z3Constraints.Add(
                relationship switch
                {
                    OptionalFeatureModelConstraint => InitOptional(parent, child),
                    MandatoryFeatureModelConstraint => InitMandatory(parent, child),
                    ExcludesFeatureModelConstraint => InitExcludes(parent, child),
                    RequiresFeatureModelConstraint => InitRequires(parent, child),
                    _ => throw new NotImplementedException($"Type {relationship.GetType()} is not suported")
                });
        }


        private AConstraint InitMandatory(BoolExpr parent, BoolExpr child)
        {
            return new Mandatory(Z3Context.MkEq(parent, child));
        }

        private AConstraint InitOptional(BoolExpr parent, BoolExpr child)
        {
            return new Optional(
                Z3Context.MkOr(
                parent,
                Z3Context.MkAnd(Z3Context.MkNot(parent), Z3Context.MkNot(child)))
            );
        }

        private AConstraint InitAlternative(BoolExpr parent, params BoolExpr[] children)
        {
            return new Alternative(
                Z3Context.MkAnd(
                InitOr(parent, children).Expression,
                Z3Context.MkAtMost(children, 1))
            );
        }

        private AConstraint InitOr(BoolExpr parent, params BoolExpr[] children)
        {
            return new Or(Z3Context.MkEq(parent, Z3Context.MkOr(children)));
        }

        private AConstraint InitRequires(BoolExpr parent, BoolExpr child)
        {
            return new Requires(Z3Context.MkImplies(parent, child));
        }

        private AConstraint InitExcludes(BoolExpr parent, BoolExpr child)
        {
            return new Excludes(Z3Context.MkOr(Z3Context.MkNot(parent), Z3Context.MkNot(child)));
        }

        private NodeProperty SelectedProperty(IFeatureModelNode node)
        {
            return new NodeProperty(node, "Selected", NodePropertyType.Boolean);
        }
    }
}
