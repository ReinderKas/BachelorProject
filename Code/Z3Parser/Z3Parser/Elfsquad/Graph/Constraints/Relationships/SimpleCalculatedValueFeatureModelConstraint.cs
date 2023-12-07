using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfskot.Core.Models.Entities.FeatureModels;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships
{
    public class SimpleCalculatedValueFeatureModelConstraint : BinaryRelationshipConstraint
    {
        public double FromValue;
        public double ToValue;
        public ImpliesRelationshipTypes ImpliesType;

        public SimpleCalculatedValueFeatureModelConstraint(
            Guid id, 
            IFeatureModelNode fromNode, 
            IFeatureModelNode toNode, 
            double fromValue, 
            double toValue, 
            ImpliesRelationshipTypes type
            ) : base(id, fromNode, toNode)
        {
            FromValue = fromValue;
            ToValue = toValue;
            ImpliesType = type;
        }

        public override bool IsCrossTreeConstraint => true;
    }
}
