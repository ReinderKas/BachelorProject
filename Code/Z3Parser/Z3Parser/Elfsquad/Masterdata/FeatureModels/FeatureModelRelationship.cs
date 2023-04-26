using Elfskot.Core.Models.Entities.FeatureModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

namespace Elfskot.Core.Models.Entities.FeatureModels
{
    [Serializable]
    public class FeatureModelRelationship
    {
        public Guid Id { get; set; }

        public Guid FeatureModelId { get; set; }

        public Guid? FromNodeId { get; set; }
        public FeatureModelNode FromNode { get; set; }

        public Guid? ToNodeId { get; set; }
        public FeatureModelNode ToNode { get; set; }


        public FeatureModelRelationshipTypes Type { get; set; }

        public List<FeatureModelRelationshipCondition> Conditions { get; set; } = new List<FeatureModelRelationshipCondition>();

        #region Implies only properties

        public Expression Expression { get; set; }
        public Guid? ExpressionId { get; set; }
        public Delegate CompiledExpression;

        #endregion Implies only properties

    }

    public enum ImpliesRelationshipTypes
    {
        Ratio = 0,
        Plus = 1,
        Constant = 3
    }

    public enum ImpliesRoundingTypes
    {
        None,
        RoundUp,
        RoundDown
    }

    public enum FeatureModelRelationshipTypes
    {
        Optional,
        Mandatory,
        Alternative,
        Or,
        Required,
        Excludes,
        Implies
    }
}