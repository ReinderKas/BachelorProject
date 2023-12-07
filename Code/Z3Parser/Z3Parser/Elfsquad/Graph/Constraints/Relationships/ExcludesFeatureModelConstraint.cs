using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships
{
    public class ExcludesFeatureModelConstraint : BinaryRelationshipConstraint
    {
        public bool IsHideInView;

        public ExcludesFeatureModelConstraint(Guid id, IFeatureModelNode fromNode, IFeatureModelNode toNode, bool isHideInView) : base(id, fromNode, toNode)
        {
            this.IsHideInView = isHideInView;
        }

        public override bool IsCrossTreeConstraint => true;
    }
}
