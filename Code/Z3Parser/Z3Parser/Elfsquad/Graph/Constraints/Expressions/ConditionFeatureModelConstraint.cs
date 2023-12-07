using Elfskot.Core.Models.Entities.FeatureModel;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public class ConditionFeatureModelConstraint : ExpressionConstraint
    {
        public Guid ConditionId;
        public ConditionFeatureModelConstraint(Guid conditionId, IFeatureModelNode affectedNode, Expression expression) 
            : base(affectedNode, expression)
        {
            this.ConditionId = conditionId;
        }
    }
}
