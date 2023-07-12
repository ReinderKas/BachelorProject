using Microsoft.Z3;
using Z3Parser.FeatureModels;

namespace Z3Parser
{
    public class DiagnosticsParser
    {
        public string ModelString;
        public Z3Solver ModelSolver;
        public bool HasSolution;
        public Expr Proof;


        public DiagnosticsParser(string modelString)
        {
            ModelString = modelString;
            HasSolution = false;
            ModelSolver = ModelBuilder.CreateModel(ModelString).Result;
            ModelSolver.InitializeZ3Solver();
        }

        public void ProveModel()
        {
            Console.WriteLine("\n");
            Console.Write($"Proof for:");
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write($"{ModelString}\n\n");
            Console.ResetColor();


            HasSolution = ModelSolver.Solve();

            if (!HasSolution)
                Proof = ModelSolver.Proof;
        }


        public void PrintProof()
        {
            PrintVariablesInProof(Proof);
            PrintArguments(Proof);
            Console.WriteLine("\n\n\n");
        }

        private void PrintVariablesInProof(Expr proof)
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
    }
}
