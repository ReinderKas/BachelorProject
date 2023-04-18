using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Microsoft.Z3;
using System.Runtime.CompilerServices;
using static Z3Parser.Z3Solver;

namespace Z3Parser
{
    /*  This class implements components of the ArcherSolver.cs class of Elfsquad.
     */


    public class Z3Solver
    {
        public Context Z3Context;
        public Optimize OptimizedSolver;
        public Solver Solver;

        public FeatureModelGraph FmGraph;
        public Requirements[] Reqs;


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

            OptimizedSolver = Z3Context.MkOptimize();
            Solver = Z3Context.MkSolver();


            /*  Elfsquad uses Expressions.
             *  These are Language Specific to the company.
             *  It's easiest to create all this without the Expressions first!.
             */

            // InitializeZ3ExpressionBuilder();
        }

        public class Requirements
        {

        }

        public bool Solve(FeatureModelGraph FeatureModelCSP, Requirements[] Requirements)
        {
            FmGraph = FeatureModelCSP;
            Reqs = Requirements;


            InitializeVariables();
            InitializeConstraints();

            if (!FindSolution())
            {
                InspectProof();
                return false;
            }
            return true;
        }

        private void InitializeVariables()
        {
            Variables = FmGraph.TopologicalSortNodes().Reverse().ToArray();
        }

        private void InitializeConstraints()
        {

        }


        private void InspectProof() 
        {

        }


        private bool FindSolution()
        {
            _requirements = requirements;
            _scriptEngine =
                new ArcherFunctionCallEngine(this, ArcherVariableStore);

            foreach (var variable in Variables)
            {
                if (_processedVariables.Contains(variable)) continue;

                if (!Process(variable)) return false;
            }


            var result = Z3Solver.Check();


            // Does UNKNOWN result also have a proof?
            return result == Status.SATISFIABLE;
        }
    }
}
