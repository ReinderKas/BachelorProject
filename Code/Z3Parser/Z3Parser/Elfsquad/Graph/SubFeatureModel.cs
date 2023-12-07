using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator
{
    public class SubFeatureModel
    {
        private IEnumerable<IFeatureModelNode> _allNodes;
        private IEnumerable<IFeatureModelConstraint> _allConstraints;
        private HashSet<Guid> _nodeIds;

        public SubFeatureModel(IEnumerable<IFeatureModelNode> nodes, IEnumerable<IFeatureModelConstraint> constraints)
        {
            this._nodeIds = nodes.Select(n => n.NodeId).ToHashSet();
            this._allNodes = nodes;
            this._allConstraints = constraints;
        }

        public IEnumerable<IFeatureModelNode> Nodes => _allNodes.ToList();
        public IEnumerable<IFeatureModelConstraint> Constraints => _allConstraints;
        public bool ContainsNode(IFeatureModelNode node) => _allNodes.Contains(node);
        public bool ContainsNodeId(Guid nodeId) => _nodeIds.Contains(nodeId);
        public void AddConstraints(params IFeatureModelConstraint[] constraints) => _allConstraints = _allConstraints.Concat(constraints);
        public void AddNodes(params IFeatureModelNode[] nodes)
        {
            _allNodes = _allNodes.Concat(nodes);
        }
    }
}
