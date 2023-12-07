using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships
{
    public class SubgroupingFeatureModelConstraint : BinaryRelationshipConstraint
    {
        public SubgroupingFeatureModelConstraint(Guid id, IFeatureModelNode fromNode, IFeatureModelNode toNode) : base(id, fromNode, toNode)
        {

        }
        public override bool IsCrossTreeConstraint => true;
    }
}
