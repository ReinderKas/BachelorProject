using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public abstract class BinaryRelationshipConstraint : IRelationshipConstraint
    {
        private Guid _id;
        private IFeatureModelNode _fromNode;
        private IFeatureModelNode _toNode;
        public IFeatureModelNode FromNode => _fromNode;
        public IFeatureModelNode ToNode => _toNode;
        public Guid FromNodeId => _fromNode.NodeId;
        public Guid ToNodeId => _toNode.NodeId;
        public Guid Id => _id;

        public BinaryRelationshipConstraint(Guid id, IFeatureModelNode fromNode, IFeatureModelNode toNode)
        {
            this._id = id;
            this._fromNode = fromNode;
            this._toNode = toNode;
        }

        public abstract bool IsCrossTreeConstraint { get; }

        public IEnumerable<Guid> ToNodeIds()
        {
            yield return ToNodeId;
        }

        public IEnumerable<IFeatureModelNode> ToNodes()
        {
            return new[] { ToNode };
        }

        public IRelationshipConstraint Clone(IFeatureModelNode fromNode, IFeatureModelNode toNode)
        {
            var clone = (BinaryRelationshipConstraint)MemberwiseClone();
            clone._id = Guid.NewGuid();          
            clone._fromNode = fromNode;
            clone._toNode = toNode;
            return clone;
        }

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            var clone = (BinaryRelationshipConstraint)MemberwiseClone();
            clone._id = Guid.NewGuid();
            clone._fromNode = newFromNode ?? _fromNode;
            clone._toNode = _toNode;
            return clone;
        }
    }
}
