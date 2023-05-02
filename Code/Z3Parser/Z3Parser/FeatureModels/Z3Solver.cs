using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;
using Microsoft.Z3;
using Z3Parser.Elfsquad.Archer.Elfsquad.Core.Archer;

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


        public Z3Solver()
        {
            Z3Context = new Context(new Dictionary<string, string>
                        {
                            { "unsat_core", "false" },  // enable generation of unsat cores
                            { "model", "true" },        // enable model generation
                            { "proof", "true" },        // enable proof generation
                            { "timeout", "60000" },     // set timout for solving
                        });

            ArcherVariableStore = new(this);

            /*  Elfsquad uses Expressions.
             *  These are Language Specific to the company.
             *  It's easiest to create all this without the Expressions first!.
             */

            // InitializeZ3ExpressionBuilder();
        }

        public bool Solve()
        {
            var solution = Solver.Check();
            if (solution == Status.SATISFIABLE)
                return true;

            Proof = Solver.Proof;
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
            InitializeConstraints();
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

            Solver.Add(
                Z3Context.MkEq(root, Z3Context.MkTrue()) 
                );
        }

        private void InitializeVariables()
        {
            Variables = FmGraph.TopologicalSortNodes().Reverse().ToArray();
        }

        private void InitializeConstraints()
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

            Solver.Add(
                relationship switch
                {
                    AlternativeFeatureModelConstraint => Alternative(parent, children.ToArray()),
                    OrFeatureModelConstraint => Or(parent, children.ToArray()),
                    _ => throw new NotImplementedException($"Type {relationship.GetType()} is not suported")
                });
        }

        private void InitializeBinaryConstraint(IRelationshipConstraint relationship)
        {
            var parent = ArcherVariableStore.GetExpression(SelectedProperty(relationship.FromNode)) as BoolExpr;
            var child = ArcherVariableStore.GetExpression(SelectedProperty(relationship.ToNodes().First())) as BoolExpr;

            Solver.Add(
                relationship switch
                {
                    OptionalFeatureModelConstraint => Optional(parent, child),
                    MandatoryFeatureModelConstraint => Mandatory(parent, child),
                    ExcludesFeatureModelConstraint => Excludes(parent, child),
                    RequiresFeatureModelConstraint => Requires(parent, child),
                    _ => throw new NotImplementedException($"Type {relationship.GetType()} is not suported")
                });
        }


        private BoolExpr Mandatory(BoolExpr parent, BoolExpr child)
        {
            return Z3Context.MkEq(parent, child);
        }

        private BoolExpr Optional(BoolExpr parent, BoolExpr child)
        {
            return Z3Context.MkOr(
                parent,
                Z3Context.MkAnd(Z3Context.MkNot(parent), Z3Context.MkNot(child)));
        }

        private BoolExpr Alternative(BoolExpr parent, params BoolExpr[] children)
        {
            return Z3Context.MkAnd(
                Or(parent, children),
                Z3Context.MkAtMost(children, 1));
        }

        private BoolExpr Or(BoolExpr parent, params BoolExpr[] children)
        {
            return Z3Context.MkEq(parent, Z3Context.MkOr(children));
        }

        private BoolExpr Requires(BoolExpr parent, BoolExpr child)
        {
            return Z3Context.MkImplies(parent, child);
        }

        private BoolExpr Excludes(BoolExpr parent, BoolExpr child)
        {
            return Z3Context.MkOr(Z3Context.MkNot(parent), Z3Context.MkNot(child));
        }

        private NodeProperty SelectedProperty(IFeatureModelNode node)
        {
            return new NodeProperty(node, "Selected", NodePropertyType.Boolean);
        }
    }
}
