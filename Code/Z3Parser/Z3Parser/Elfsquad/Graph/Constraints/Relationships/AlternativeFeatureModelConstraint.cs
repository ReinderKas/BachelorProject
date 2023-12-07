using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships
{
    public class AlternativeFeatureModelConstraint : NaryRelationshipConstraint
    {
        public AlternativeFeatureModelConstraint(Guid id, IFeatureModelNode fromNode, IEnumerable<IFeatureModelNode> toNodes) : base(id, fromNode, toNodes)
        {
            fromNode.AddChildren(toNodes.ToArray());
        }

        public override bool IsCrossTreeConstraint => false;
    }
}
