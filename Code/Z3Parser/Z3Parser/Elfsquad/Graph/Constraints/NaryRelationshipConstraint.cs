using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public abstract class NaryRelationshipConstraint : IRelationshipConstraint
    {
        private Guid _id;
        private IFeatureModelNode _fromNode;
        private List<IFeatureModelNode> _toNodes;
        public IFeatureModelNode FromNode => _fromNode;
        public IEnumerable<IFeatureModelNode> ToNodes => _toNodes;
        public Guid FromNodeId => _fromNode.NodeId;
        public IEnumerable<Guid> ToNodeIds => ToNodes.Select(n => n.NodeId);
        public Guid Id => _id;


        public NaryRelationshipConstraint(Guid id, IFeatureModelNode fromNode, IEnumerable<IFeatureModelNode> toNodes)
        {
            this._id = id;
            this._fromNode = fromNode;
            this._toNodes = toNodes.ToList();
        }


        public abstract bool IsCrossTreeConstraint { get; }

        IEnumerable<Guid> IRelationshipConstraint.ToNodeIds()
        {
            return ToNodeIds;
        }

        IEnumerable<IFeatureModelNode> IRelationshipConstraint.ToNodes()
        {
            return _toNodes;
        }
        
        public void AddToNode(IFeatureModelNode node)
        {
            _toNodes.Add(node);
        }

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            var clone = MemberwiseClone() as NaryRelationshipConstraint;
            clone._fromNode = newFromNode ?? _fromNode;
            return clone;
        }
    }
}
