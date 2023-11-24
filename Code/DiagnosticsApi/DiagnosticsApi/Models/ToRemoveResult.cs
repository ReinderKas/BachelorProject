using Microsoft.Z3;
using Z3Parser.Z3Constraints;

namespace DiagnosticsApi.Models
{
    public class ToRemoveResult
    {
        public string RelationshipType;
        public NodeResult[] Nodes;


        public ToRemoveResult() { }

        public ToRemoveResult(AConstraint constraint)
        {
            RelationshipType = GetRelType(constraint);
            Nodes = GetvariablesInExpression(constraint.Expression);
        }  

        public string GetRelType(AConstraint constraint)
        {
            return constraint switch
            {
                Excludes => "excludes",
                Requires => "requires",
                _ => throw new NotImplementedException($"Type {constraint.GetType()} should not be a removeable Constraint.")
            };
        }

        private NodeResult[] GetvariablesInExpression(Expr constraint)
        {
            var result = new List<string>();
            VisitRecursively(constraint, result);

            result = result.Distinct().OrderBy(v => v.ToString()).ToList();

            return result.Select(n => new NodeResult(n)).ToArray();
        }


        private void VisitRecursively(Expr proof, List<string> res)
        {
            if (!proof.Args.Any())
            {
                if (proof.ToString() != "true" && proof.ToString() != "false")
                    res.Add(proof.ToString());
                return;
            }

            foreach (var arg in proof.Args)
                VisitRecursively(arg, res);
        }

        public override string ToString() => $"{RelationshipType} : {Nodes[0].Name} - {Nodes[1].Name}";
    }
}
