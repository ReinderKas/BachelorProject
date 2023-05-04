using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using Microsoft.Z3;
using System.Dynamic;
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

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [mandatory1] - excludes -> [mandatory2]
                }");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [root] - alternative -> [alt1]
                    [root] - alternative -> [alt2]

                    [mandatory1] - excludes -> [alt1]
                    [mandatory2] - excludes -> [alt2]
                }");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [root] - mandatory -> [mandatory3]
                    [root] - alternative -> [alt1]
                    [root] - alternative -> [alt2]
                    [root] - alternative -> [alt3]

                    [mandatory1] - excludes -> [alt1]
                    [mandatory1] - excludes -> [alt2]
                    [mandatory1] - excludes -> [alt3]
                }");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [root] - mandatory -> [mandatory3]
                    [root] - mandatory -> [mandatory4]
                    [root] - mandatory -> [mandatory5]
                    [root] - mandatory -> [mandatory6]
                    [root] - alternative -> [alt1]
                    [root] - alternative -> [alt2]
                    [root] - alternative -> [alt3]
                    [root] - alternative -> [alt4]
                    [root] - alternative -> [alt5]
                    [root] - alternative -> [alt6]

                    [mandatory1] - excludes -> [alt1]
                    [mandatory2] - excludes -> [alt2]
                    [mandatory3] - excludes -> [alt3]
                    [mandatory4] - excludes -> [alt4]
                    [mandatory5] - excludes -> [alt5]
                    [mandatory6] - excludes -> [alt6]
                }");


        ProveModel(@" 
                model [root] {
                    [root] - optional -> [opt1]
                    [root] - mandatory -> [mand1]
                    [root] - mandatory -> [mand2]


                    [mand1] - requires -> [opt1]
                    [opt1] - excludes -> [mand2]
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
        if (!model.Solve())
            PrintProof(model.Proof);
        else
            Console.WriteLine("Found Solution!");
        Console.WriteLine("\n\n\n\n\n\n\n");

    }

    private static void PrintProof(Expr proof)
    {
        PrintVariablesInProof(proof);
        PrintArguments(proof);
    }

    private static void PrintVariablesInProof(Expr proof)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Arguments in proof.");
        Console.ResetColor();

        var result = new List<Expr>();
        VisitRecursively(proof, result);

        foreach(var res in result.Distinct().OrderBy(v => v.ToString()))
            Console.WriteLine($"  {res.ToString()}");
        Console.WriteLine("\n----------------------------------");
    }


    private static void VisitRecursively(Expr proof, List<Expr> res)
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


    private static void PrintArguments(Expr proof)
    {
        for(int i = 0; i < proof.Args.Length; i++)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Proof - Arg: {i}");
            Console.ResetColor();
            Console.WriteLine(proof.Args[i]);
            Console.WriteLine($"-------------------------------");
        }
    }
}