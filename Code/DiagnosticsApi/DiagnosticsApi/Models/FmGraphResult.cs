using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace DiagnosticsApi.Models
{
    [Serializable]
    public class FmGraphResult
    {
        public NodeResult[] Nodes;
        public ConstraintResult[] Edges;


        public FmGraphResult(
            IEnumerable<IFeatureModelNode> nodes,
            IEnumerable<IFeatureModelConstraint> constraints)
        {
            // TODO: Only works for relationshipConstraints atm. Not Expression Constraints.
            Edges = constraints.Select(c => new ConstraintResult((IRelationshipConstraint)c)).ToArray();
            Nodes = nodes.Select(n => new NodeResult(n)).ToArray();
        }

        public override string ToString()
        {
            var result = "";
            result += $"\nNodes: {Nodes.Count()} \n";

            foreach (var node in Nodes)
                result += $"\t{node.Id} - {node.Name} \n";

            result += $"\nConstraints: {Edges.Count()} \n";
            foreach (var constraint in Edges)
                result += $"\t{constraint.FromNode} - {constraint.Type} - {constraint.ToNodes[0]}\n";

            return result;
        }
    }
}
