using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Microsoft.Z3;
using Z3Parser.FeatureModels;

internal class Program
{
    private static void Main(string[] args)
    {
        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [mandatory1] - excludes -> [mandatory1]
                }");

        Console.WriteLine("\n\n\n\n\n\n\n");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [root] - alternative -> [alt1]
                    [root] - alternative -> [alt2]

                    [mandatory1] - excludes -> [alt1]
                    [mandatory1] - excludes -> [alt2]
                }");
    }




    private static void ProveModel(string modelString)
    {
        Console.Write($"Proof for:");
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.Write($"{modelString}\n\n");
        Console.ResetColor();



        var model = ModelBuilder.CreateModel(modelString).Result;

        model.InitializeZ3Solver();
        model.Solve();

        PrintProof(model.Proof);

    }

    private static void PrintProof(Expr proof)
    {
        for(int i = 0; i < proof.Args.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Proof - Arg: {i}");
            Console.ResetColor();
            Console.WriteLine(proof.Args[i]);
            //Console.WriteLine($"\nRecursively: ");
            //VisitProofRecursively(proof.Args[i]);
            Console.WriteLine($"-------------------------------");
        }
    }

    private static void VisitProofRecursively(Expr expression)
    {
        Console.WriteLine($"-----------------------------------------------------------------------------------------------");
        Console.WriteLine($"{expression}\n");

        foreach (var arg in expression.Args)
            VisitProofRecursively(arg);

        return;
    }
}