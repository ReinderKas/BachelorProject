using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public class LowerBoundConstraint : IFeatureModelConstraint
    { 
        public ArcherLowerBoundExpression LowerBoundExpression;
        private readonly IFeatureModelNode _node;
        public Guid Id => Guid.Empty;
        public Guid FromNodeId => _node.NodeId;
        public IFeatureModelNode FromNode => _node;
        public bool IsCrossTreeConstraint => false;

        public LowerBoundConstraint(ArcherLowerBoundExpression lowerBoundExpression, IFeatureModelNode node)
        {
            LowerBoundExpression = lowerBoundExpression;
            _node = node;
        }

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            return new LowerBoundConstraint(LowerBoundExpression, newFromNode ?? _node);
        }
    }
}