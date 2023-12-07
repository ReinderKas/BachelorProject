using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships
{
    public class MandatoryFeatureModelConstraint : BinaryRelationshipConstraint
    {
        public MandatoryFeatureModelConstraint(Guid id, IFeatureModelNode fromNode, IFeatureModelNode toNode) : base(id, fromNode, toNode)
        {
            fromNode.AddChildren(toNode);
        }
        public override bool IsCrossTreeConstraint => false;
    }
}
