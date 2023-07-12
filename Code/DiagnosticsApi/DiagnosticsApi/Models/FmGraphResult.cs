using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace DiagnosticsApi.Models
{
    [Serializable]
    public class FmGraphResult
    {
        public Node[] Nodes;
        public Constraint[] Edges;


        public FmGraphResult(
            IEnumerable<IFeatureModelNode> nodes,
            IEnumerable<IFeatureModelConstraint> constraints)
        {
            // TODO: Only works for relationshipConstraints atm. Not Expression Constraints.
            Edges = constraints.Select(c => new Constraint((IRelationshipConstraint)c)).ToArray();
            Nodes = nodes.Select(n => new Node(n)).ToArray();
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

    [Serializable]
    public class Node
    {
        public string Name;
        public Guid Id;

        public Node(IFeatureModelNode node)
        {
            Name = node.FeatureModelNode.Name;
            Id = node.NodeId;
        }
    }


    // TODO: Currently only works for normal Relationship Constraints. Not Expression Constraints
    [Serializable]
    public class Constraint
    {
        public ConstraintType Type;
        public Guid FromNode;
        public Guid[] ToNodes;


        public Constraint(IRelationshipConstraint constraint)
        {
            switch (constraint)
            {
                case MandatoryFeatureModelConstraint:
                    Type = ConstraintType.Mandatory; break;
                case OptionalFeatureModelConstraint:
                    Type = ConstraintType.Optional; break;
                case AlternativeFeatureModelConstraint:
                    Type = ConstraintType.Alternative; break;
                case OrFeatureModelConstraint:
                    Type = ConstraintType.Or; break;

                case ExcludesFeatureModelConstraint:
                    Type = ConstraintType.Excludes; break;
                case RequiresFeatureModelConstraint:
                    Type = ConstraintType.Requires; break;
                default:
                    break;
            }

            FromNode = constraint.FromNodeId;
            ToNodes = constraint.ToNodeIds().ToArray();
        }
    }

    public enum ConstraintType
    {
        Mandatory,
        Optional,
        Alternative,
        Or,

        Excludes,
        Requires,
    }
}
