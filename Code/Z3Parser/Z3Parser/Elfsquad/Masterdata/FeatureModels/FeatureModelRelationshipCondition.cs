using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Elfskot.Core.Models.Entities.FeatureModels
{
    [Serializable]
    public abstract class FeatureModelRelationshipCondition
    {
        public Guid FeatureModelRelationshipId { get; set; }

        public abstract bool Invoke();
        public Entities.FeatureModel.Expression ConditionExpression { get; set; }
        public Guid? ConditionExpressionId { get; set; }
    }

    public class ExpressionFeatureModelRelationshipCondition : FeatureModelRelationshipCondition
    {
        public string Expression { get; set; }

        public override bool Invoke() => true;
    }
    
    public class SubGroupingFeatureModelRelationshipCondition : FeatureModelRelationshipCondition
    {
        public Guid? FromNodeId { get; set; }

        public override bool Invoke() => true;
    }
}