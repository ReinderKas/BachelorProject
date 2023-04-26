using Elfskot.Core.Models.Entities.FeatureModels;
using Elfsquad.Core.Archer;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes
{
    public class RootNode : IFeatureModelNode
    {
        private FeatureModelNode _featureModelNode;
        private IEnumerable<IFeatureModelNode> _childNodes;
        private Dictionary<string, object> _properties;
        private bool _isDefault;
        private ArcherVariableType _type;
        private int? _cardinalityMin;
        private int? _cardinalityMax;

        public bool IsRoot { get => true; }
        public Guid NodeId { get => _featureModelNode.Id; }
        public FeatureModelNode FeatureModelNode { get => _featureModelNode; }
        public IEnumerable<IFeatureModelNode> ChildNodes { get => _childNodes; }
        public Dictionary<string, object> Properties => _properties;
        public bool IsDefault => _isDefault;
        public int? CardinalityMin => _cardinalityMin;
        public int? CardinalityMax => _cardinalityMax;

        public ArcherVariableType ArcherVariableType => _type;

        public RootNode(FeatureModelNode node)
        {
            Init(node);
        }

        public RootNode(FeatureModelNode node, ArcherVariable variable)
        {
            Init(node);
            _type = variable.Type;
            _cardinalityMin = variable.CardinalityMin;
            _cardinalityMax = variable.CardinalityMax;
        }

        private void Init(FeatureModelNode node)
        {
            _featureModelNode = node;
            _childNodes = new List<IFeatureModelNode>();
            _isDefault = false;
            _properties = new Dictionary<string, object>();
            _type = ArcherVariableType.Float;
            _cardinalityMin = null;
            _cardinalityMax = null;
        }

        public void AddChildren(params IFeatureModelNode[] children) => _childNodes = _childNodes.Concat(children);
        public void InsertChildren(int index, params IFeatureModelNode[] children)
        {
            var childNodes = _childNodes.ToList();
            for (int i = 0; i < children.Length; i++)
            {
                childNodes.Insert(index + i, children[i]);
            }
            _childNodes = childNodes.ToArray();
        }
        public void SetDefault(bool value) => _isDefault = value;
    }
}