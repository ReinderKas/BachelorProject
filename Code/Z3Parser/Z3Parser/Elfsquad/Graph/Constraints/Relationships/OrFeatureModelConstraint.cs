using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships
{
    public class OrFeatureModelConstraint : NaryRelationshipConstraint
    {
        public OrFeatureModelConstraint(Guid id, IFeatureModelNode fromNode, IEnumerable<IFeatureModelNode> toNodes) : base(id, fromNode, toNodes)
        {
            fromNode.AddChildren(toNodes.ToArray());
        }

        public override bool IsCrossTreeConstraint => false;
    }
}
