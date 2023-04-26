using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;
using Microsoft.Z3;

namespace Z3Parser.FeatureModels
{
    /*  This class implements components of the ArcherSolver.cs class of Elfsquad.
     */

    public class Z3Solver
    {
        public Context Z3Context;
        public Optimize OptimizedSolver;
        public Solver Solver;

        public FeatureModelGraph FmGraph;
        public ArcherModel[] ArcherModel;

        internal NodeProperty[] Variables;

        public Z3Solver()
        {
            Z3Context = new Context(new Dictionary<string, string>
                        {
                            { "unsat_core", "false" },  // enable generation of unsat cores
                            { "model", "true" },        // enable model generation
                            { "proof", "true" },        // enable proof generation
                            { "timeout", "60000" },     // set timout for solving
                        });

            /*  Elfsquad uses Expressions.
             *  These are Language Specific to the company.
             *  It's easiest to create all this without the Expressions first!.
             */

            // InitializeZ3ExpressionBuilder();
        }

        public class Requirements
        {

        }

        public async Task InitializeFeatureModelGraph(string model)
        {
            ArcherModel = new ArcherTranslator().Parse(model);
            FmGraph = await new ArcherModelTranslator().BuildGraphAsync(ArcherModel[0]);

            if (FmGraph == null)
                throw new Exception("Could not create Feature Model Graph instance. Please validate the model: " + model);
        }

        public bool Solve()
        {
            InitializeVariables();
            InitializeConstraints();

            InitializeSolvers();

            return true;
        }

        private void InitializeSolvers()
        {
            OptimizedSolver = Z3Context.MkOptimize();
            Solver = Z3Context.MkSolver();
        }

        private void InitializeVariables()
        {
            Variables = FmGraph.TopologicalSortNodes().Reverse().ToArray();
        }

        private void InitializeConstraints()
        {
            //var child = ArcherVariableStore.GetExpression(SelectedProperty(relationship.ToNodes().First())) as BoolExpr;
            //var conditionExpr = BuildConditionExpression(variable);

            //var parent = ArcherVariableStore.GetExpression(SelectedProperty(relationship.FromNode)) as BoolExpr;

            //foreach (var constraint in FmGraph.GetConstraints())
            //{
            //    var relationshipExpr = constraint switch
            //    {
            //        OptionalFeatureModelConstraint => Optional(parent, child),
            //        MandatoryFeatureModelConstraint => Mandatory(parent, child),
            //        ExcludesFeatureModelConstraint => Excludes(parent, child),
            //        RequiresFeatureModelConstraint => Requires(parent, child),
            //        AlternativeFeatureModelConstraint => Alternative(parent, children)
            //        _ => null
            //    };
            //}
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
