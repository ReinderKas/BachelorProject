using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public class ArcherConditionConstraint : IFeatureModelConstraint
    {
        private readonly IFeatureModelNode _node;
        public ArcherCondition ArcherCondition { get; }
        public Guid Id => Guid.Empty;
        public Guid FromNodeId => _node.NodeId;
        public IFeatureModelNode FromNode => _node;
        public bool IsCrossTreeConstraint => false;

        public ArcherConditionConstraint(ArcherCondition archerCondition, IFeatureModelNode node)
        {
            _node = node;
            ArcherCondition = archerCondition;
        }

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            return new ArcherConditionConstraint(ArcherCondition, newFromNode ?? _node);
        }
    }
}