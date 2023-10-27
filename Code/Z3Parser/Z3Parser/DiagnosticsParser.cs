using Microsoft.Z3;
using Z3Parser.FeatureModels;
using Z3Parser.Z3Constraints;

namespace Z3Parser
{
    public class DiagnosticsParser
    {
        public string ModelString;
        public Z3Solver ModelSolver;
        public bool HasSolution;

        public Expr Proof() => ModelSolver.Proof;
        public Expr[] UnsatCore() => ModelSolver.UnsatCore;
        public Expr[] UnsatCoreArguments() => ModelSolver.UnsatCore.SelectMany(c => c.Args).Distinct().ToArray();


        public DiagnosticsParser(string modelString)
        {
            ModelString = modelString;
            HasSolution = false;
            ModelSolver = ModelBuilder.CreateModel(ModelString).Result;
        }

        public void ProveModel()
        {
            Console.WriteLine("\n");
            Console.Write($"Proof for:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{ModelString}\n\n");
            Console.ResetColor();

            HasSolution = ModelSolver.Solve();
        }



        // As per Chapter 7.4.2 of the holy bible.
        public IEnumerable<AConstraint> FastDiagnose()
        {
            var coreConstraints = ModelSolver.Z3Constraints.Where(c => c.Hierarchical);
            var toDiagnose = ModelSolver.Z3Constraints.Except(coreConstraints);

            // No Constraints to Diagnose.
            if (!toDiagnose.Any()
                || !GetSolution(coreConstraints))
                return Enumerable.Empty<AConstraint>();

            // Used to be implemented for User Requirements.
            return FD(null, toDiagnose, ModelSolver.Z3Constraints);
        }



        private IEnumerable<AConstraint> FD(
            IEnumerable<AConstraint> d,
            IEnumerable<AConstraint> c,
            IEnumerable<AConstraint> ac)
        {
            if (d != null)
            {
                if (GetSolution(ac))
                    return new List<AConstraint>();
            }
            if (c.Count() == 1) return c;

            var k = c.Count() / 2;
            var c1 = c.Take(k).ToList();
            var c2 = c.Skip(k).ToList();

            var d1 = FD(c2, c1, ac.Except(c2));
            var d2 = FD(d1, c2, ac.Except(d1));
            return d1.Concat(d2);
        }


        private bool GetSolution(IEnumerable<AConstraint> constraints)
        {
            Console.WriteLine("Getting Solution.");

            ModelSolver.Z3Constraints = constraints.ToHashSet();
            return ModelSolver.Solve();
        }



        #region Print Diagnostics

        public void PrintProof()
        {
            PrintVariablesInProof(Proof());
            PrintArguments(Proof());
            Console.WriteLine("\n\n\n");
        }

        public void PrintUnsatCore()
        {
            Console.WriteLine($"Printing unsat core: {UnsatCore().Count()}");
            foreach(var coreExpr in UnsatCore())
            {
                Console.WriteLine(coreExpr.ToString());
            }
            Console.WriteLine($"\n\n\n");
        }

        public void PrintVariablesInProof(Expr proof)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Arguments in proof.");
            Console.ResetColor();

            var result = new List<Expr>();
            VisitRecursively(proof, result);

            foreach (var res in result.Distinct().OrderBy(v => v.ToString()))
                Console.WriteLine($"  {res.ToString()}");
            Console.WriteLine("\n----------------------------------");
        }


        private void VisitRecursively(Expr proof, List<Expr> res)
        {
            if (!proof.Args.Any())
            {
                if (proof.ToString() != "true" && proof.ToString() != "false")
                    res.Add(proof);
                return;
            }

            foreach (var arg in proof.Args)
                VisitRecursively(arg, res);
        }


        private void PrintArguments(Expr proof)
        {
            for (int i = 0; i < proof.Args.Length; i++)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Proof - Arg: {i}");
                Console.ResetColor();
                Console.WriteLine(proof.Args[i]);
                Console.WriteLine($"-------------------------------");
            }
        }

        #endregion
    }
}
