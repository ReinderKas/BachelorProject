using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Microsoft.Z3;
using System.Text.Json.Serialization;

namespace DiagnosticsApi.Models
{
    [Serializable]
    public class UnsatCoreResult
    {
        public NodeResult[] Nodes { get; set; }
        public ConstraintResult[] Constraint { get; set; }

        //TODO: Currently string, could this be Z3.Expr?
        //      Need something to fix this within the front-end
        public string[] UnsatisfiableCore { get; set; }


        public UnsatCoreResult(
            Expr[] core,
            Expr[] nodes,
            NodeProperty[] modelVariables,
            ConstraintResult[] constraint)
        {
            UnsatisfiableCore = core.Select(c => c.ToString()).ToArray();
            Constraint = constraint;
            
            Nodes = ConvertNodes(nodes);
        }

        private NodeResult[] ConvertNodes(Expr[] nodes)
        {
            var result = new List<NodeResult>();

            foreach (var node in nodes)
                VisitRecursively(node, result); // Distinct?

            result = result.DistinctBy(n => n.Name).ToList();

            PrintVariablesInUnsatCore(result);
            return result.ToArray();
        }



        public void PrintVariablesInUnsatCore(List<NodeResult> result)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Arguments in Unsat Core.");
            Console.ResetColor();

            foreach (var res in result)
                Console.WriteLine($"  {res.Name}");
            Console.WriteLine("\n----------------------------------");
        }


        private void VisitRecursively(Expr proof, List<NodeResult> res)
        {
            if (!proof.Args.Any())
            {
                // TODO: This won't work for Nodes that do not have a unique name.

                if (proof.ToString() != "true" && proof.ToString() != "false")
                    res.Add(new NodeResult(proof.ToString()));
                    
                return;
            }

            foreach (var arg in proof.Args)
                VisitRecursively(arg, res);
        }
    }
}
