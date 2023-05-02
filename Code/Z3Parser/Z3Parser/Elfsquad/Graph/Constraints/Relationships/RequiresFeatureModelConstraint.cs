using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using System;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships
{
    public class RequiresFeatureModelConstraint : BinaryRelationshipConstraint
    {
        public bool IsDefaultSelector;

        public RequiresFeatureModelConstraint(Guid id, IFeatureModelNode fromNode, IFeatureModelNode toNode, bool isDefaultSelector) : base(id, fromNode, toNode)
        {
            this.IsDefaultSelector = isDefaultSelector;
        }
        public override bool IsCrossTreeConstraint => true;
    }
}
