using Elfskot.Core.Models.Entities.FeatureModel;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public class MaxValueFeatureModelConstraint : ExpressionConstraint
    {
        public MaxValueFeatureModelConstraint(IFeatureModelNode affectedNode, Expression expression) 
            : base(affectedNode, expression)
        {

        }
    }
}
