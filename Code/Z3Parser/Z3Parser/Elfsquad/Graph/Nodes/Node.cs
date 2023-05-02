using Elfskot.Core.Models.Entities;
using Elfskot.Core.Models.Entities.FeatureModels;
using Elfsquad.Core.Archer;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes
{
    public class Node : IFeatureModelNode
    {
        private FeatureModelNode _featureModelNode;
        private IEnumerable<IFeatureModelNode> _childNodes;
        private bool _isDefault;
        private Dictionary<string, object> _properties;
        private ArcherVariableType _type;
        private int? _cardinalityMin;
        private int? _cardinalityMax;

        public Node(FeatureModelNode node)
        {
            Init(node);
        }

        public Node(FeatureModelNode node, ArcherVariable variable)
        {
            Init(node);
            _type = variable.Type;
            _cardinalityMin = variable.CardinalityMin;
            _cardinalityMax = variable.CardinalityMax;
        }

        public Node(IFeatureModelNode nodeToCopy)
        {
            _featureModelNode = new FeatureModelNode { Id = Guid.NewGuid() };
            _childNodes = new List<IFeatureModelNode>();
            _isDefault = nodeToCopy.IsDefault;
            _properties = nodeToCopy.Properties.ToDictionary(v => v.Key, v => v.Value);
            _type = nodeToCopy.ArcherVariableType;
            _cardinalityMin = nodeToCopy.CardinalityMin;
            _cardinalityMax = nodeToCopy.CardinalityMax;
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

        public bool IsRoot => false;
        public Guid NodeId => _featureModelNode.Id;
        public FeatureModelNode FeatureModelNode => _featureModelNode;
        public IEnumerable<IFeatureModelNode> ChildNodes => _childNodes;
        public bool IsDefault => _isDefault;
        public void AddChildren(params IFeatureModelNode[] children) => _childNodes = _childNodes.Concat(children).ToArray();
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
        public Dictionary<string, object> Properties => _properties;
        public ArcherVariableType ArcherVariableType => _type;

        public int? CardinalityMin => _cardinalityMin;
        public int? CardinalityMax => _cardinalityMax;
    }
}
