using System;
using System.Reflection.Metadata.Ecma335;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public class UpperBoundConstraint : IFeatureModelConstraint
    { 
        public ArcherUpperBoundExpression UpperBoundExpression;
        private readonly IFeatureModelNode _node;
        public Guid Id => Guid.Empty;
        public Guid FromNodeId => _node.NodeId;
        public IFeatureModelNode FromNode => _node;
        public bool IsCrossTreeConstraint => false;

        public UpperBoundConstraint(ArcherUpperBoundExpression upperBoundExpression, IFeatureModelNode node)
        {
            UpperBoundExpression = upperBoundExpression;
            _node = node;
        }

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            return new UpperBoundConstraint(UpperBoundExpression, newFromNode ?? _node);
        }
    }
}