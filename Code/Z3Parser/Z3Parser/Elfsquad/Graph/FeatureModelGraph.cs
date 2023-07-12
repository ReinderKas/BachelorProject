using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator
{
    public class FeatureModelGraph
    {
        private List<IFeatureModelNode> _featureModelNodes;
        private List<IFeatureModelConstraint> _featureModelConstraints;
            
        // Lookup lists
        private Dictionary<Guid, IFeatureModelNode> _nodesByNodeId;
        private Dictionary<Guid, List<IFeatureModelConstraint>> _constraintsByFromNodeId;
        private Dictionary<Guid, List<IFeatureModelConstraint>> _constraintsByToNodeId;

        public FeatureModelGraph(
            List<IFeatureModelNode> nodes, 
            List<IFeatureModelConstraint> constraints)
        {
            _featureModelNodes = nodes;
            _featureModelConstraints = constraints;

            InitializeLookupLists();
        }

        public override string ToString()
        {
            var result = "";
            result += $"Nodes: {_featureModelNodes.Count()} \n";
            result += $"Constraints: {_featureModelConstraints.Count()} \n";
            return result;
        }

        private void InitializeLookupLists()
        {
            _nodesByNodeId = _featureModelNodes.ToDictionary(n => n.NodeId, n => n);

            _constraintsByFromNodeId = new Dictionary<Guid, List<IFeatureModelConstraint>>();
            _constraintsByToNodeId = new Dictionary<Guid, List<IFeatureModelConstraint>>();
            foreach (var constraint in _featureModelConstraints)
            {
                if (!_constraintsByFromNodeId.ContainsKey(constraint.FromNodeId))
                    _constraintsByFromNodeId[constraint.FromNodeId] = new List<IFeatureModelConstraint>();
                
                _constraintsByFromNodeId[constraint.FromNodeId].Add(constraint);

                if (constraint is IRelationshipConstraint relationshipConstraint)
                {
                    foreach (var toNodeId in relationshipConstraint.ToNodeIds())
                    {
                        if (!_constraintsByToNodeId.ContainsKey(toNodeId))
                            _constraintsByToNodeId[toNodeId] = new List<IFeatureModelConstraint>();
                        
                        _constraintsByToNodeId[toNodeId].Add(constraint);
                    }
                }
            }
        }
        
        public List<IFeatureModelNode> GetNodes() => _featureModelNodes;
        public List<IFeatureModelConstraint> GetConstraints() => _featureModelConstraints;
        public List<IFeatureModelConstraint> GetConstraints(Guid parentId) => 
            _constraintsByFromNodeId.TryGetValue(parentId, out var constraints)
            ? constraints
            : new List<IFeatureModelConstraint>();

        public List<IFeatureModelConstraint> GetConstraintsTo(Guid childId) =>
            _constraintsByToNodeId.TryGetValue(childId, out var constraints)
            ? constraints
            : new List<IFeatureModelConstraint>();

        public IFeatureModelNode GetNodeById(Guid id) => _nodesByNodeId[id];
        
        public IFeatureModelNode GetParent(IFeatureModelNode node)
        {
            var constraint = GetRelationshipConstraint(node);
            return constraint == null ? null : _nodesByNodeId[constraint.FromNodeId];
        }

        public IFeatureModelNode[] GetChildren(IFeatureModelNode node)
        {
            return _featureModelConstraints.OfType<IRelationshipConstraint>()
                .Where(c => !c.IsCrossTreeConstraint && c.FromNodeId == node.NodeId)
                .SelectMany(c => c.ToNodeIds().Select(nId => _nodesByNodeId[nId]))
                .ToArray();
        }

        /// <summary>
        /// Retrieves the node's structural (non-cross tree) relationship
        /// </summary>
        public IRelationshipConstraint GetRelationshipConstraint(IFeatureModelNode node)
        {
            if (!_constraintsByToNodeId.TryGetValue(node.NodeId, out var featureModelConstraints)) return null;
            
            return featureModelConstraints.OfType<IRelationshipConstraint>()
                .FirstOrDefault(c => !c.IsCrossTreeConstraint && c.ToNodeIds().Contains(node.NodeId));
        }
        
        public IFeatureModelNode[] GetSiblingsOfType<TRType>(IFeatureModelNode node) where TRType : IRelationshipConstraint
        {
            var parent = GetParent(node);
            return _featureModelConstraints.OfType<TRType>()
                .Where(r => r.FromNodeId == parent.NodeId && r.ToNodeIds().Contains(node.NodeId))
                .SelectMany(r => r.ToNodeIds().Select(nodeId => _nodesByNodeId[nodeId]))
                .Where(n => n != node)
                .ToArray();
        }

        public IExpressionConstraint[] GetExpressionsOnNode(IFeatureModelNode node)
        {
            return _featureModelConstraints
                .OfType<IExpressionConstraint>()
                .Where(c => c.FromNodeId == node.NodeId)
                .ToArray();
        }

        public IFeatureModelNode[] GetFilteredChildren(IFeatureModelNode current)
        {
            var subgroupingConstraints = _featureModelConstraints
                .OfType<SubgroupingFeatureModelConstraint>()
                .Where(c => c.FromNodeId == current.NodeId);

            var selectedNodeIds = subgroupingConstraints.Select(c => c.ToNodeId).ToHashSet();

            var allParents = subgroupingConstraints
                .Select(c => GetParent(c.ToNode))
                .Distinct();

            return allParents
                .SelectMany(p => GetChildren(p))
                .Where(c => !selectedNodeIds.Contains(c.NodeId))
                .ToArray();
        }

        public NodeProperty[] TopologicalSortNodes()
        {
            var stack = new Stack<NodeProperty>();
            var visitedNodes = new HashSet<NodeProperty>();

            foreach (var node in _featureModelNodes)
            {
                var selectedProperty = new NodeProperty(node, "Selected", NodePropertyType.Boolean);
                if (!visitedNodes.Contains(selectedProperty))
                    TopologicalSortUtilSelected(selectedProperty, visitedNodes, stack);
                
                var valueProperty = new NodeProperty(node, "Value", 
                    node.ArcherVariableType == Elfsquad.Core.Archer.ArcherVariableType.Int
                        ? NodePropertyType.Int
                        : NodePropertyType.Float);
                if (!visitedNodes.Contains(valueProperty))
                    TopologicalSortUtilValue(valueProperty, visitedNodes, stack);
            }
                
            return stack.ToArray();
        }

        public string GetPath(IFeatureModelNode node)
        {   
            var path = new List<int>();

            while (node != null)
            {
                var rel = GetRelationshipConstraint(node);
                if (rel == null)
                {
                    path.Add(0);
                    break;
                }

                path.Add(Array.IndexOf(GetChildren(rel.FromNode), node));
                node = rel.FromNode;
            }

            path.Reverse();
            return string.Join('.', path);
        }

        private void TopologicalSortUtilSelected(NodeProperty nodeProperty, ISet<NodeProperty> visited, Stack<NodeProperty> stack)
        {
            visited.Add(nodeProperty);
        
            var parent = GetParent(nodeProperty.Node);
            if (parent != null)
            {
                var parentSelection = new NodeProperty(parent, "Selected", NodePropertyType.Boolean);
                if (!visited.Contains(parentSelection))
                    TopologicalSortUtilSelected(parentSelection, visited, stack);    
            }

            if (_constraintsByToNodeId.TryGetValue(nodeProperty.Node.NodeId, out var featureModelConstraints))
            {
                foreach (var constraint in featureModelConstraints.OfType<IRelationshipConstraint>()
                             .Where(c => c.IsCrossTreeConstraint))
                {
                    if (constraint is SimpleCalculatedValueFeatureModelConstraint) continue;
                
                    var fromNodeSelected = new NodeProperty(constraint.FromNode, "Selected", NodePropertyType.Boolean);
                    if (visited.Contains(fromNodeSelected)) continue;
                    TopologicalSortUtilSelected(fromNodeSelected, visited, stack);
                }
            }            
            
            stack.Push(nodeProperty);   
        }
        
        private void TopologicalSortUtilValue(NodeProperty nodeProperty, ISet<NodeProperty> visited, Stack<NodeProperty> stack)
        {
            visited.Add(nodeProperty);

            if (_constraintsByToNodeId.TryGetValue(nodeProperty.Node.NodeId, out var featureModelConstraints))
            {
                foreach (var constraint in featureModelConstraints.OfType<SimpleCalculatedValueFeatureModelConstraint>()
                             .Where(c => c.ToNodes().Contains(nodeProperty.Node)))
                {
                    var fromNodeSelected = new NodeProperty(constraint.FromNode, "Selected", NodePropertyType.Boolean);
                    if (visited.Contains(fromNodeSelected)) continue;
                    TopologicalSortUtilSelected(fromNodeSelected, visited, stack);
                } 
            }

            if (_constraintsByFromNodeId.TryGetValue(nodeProperty.Node.NodeId, out var fromConstraints))
            {
                foreach (var constraint in fromConstraints.OfType<IExpressionConstraint>())
                {
                    foreach (var toNode in ToNodes(constraint))
                    {
                        var toNodeValue = new NodeProperty(toNode, "Value", toNode.ArcherVariableType == Elfsquad.Core.Archer.ArcherVariableType.Float ? NodePropertyType.Float : NodePropertyType.Int);
                        if (visited.Contains(toNodeValue)) continue;
                        TopologicalSortUtilValue(toNodeValue, visited, stack);
                    }
                }
            }
            
            stack.Push(nodeProperty);
            
        }

        private IEnumerable<IFeatureModelNode> ToNodes(IExpressionConstraint expressionConstraint)
        {
            return expressionConstraint.Expression.Variables
                .SelectMany(v => v.Children ? GetChildren(GetNodeById(v.NodeId)) : new[] { GetNodeById(v.NodeId) })
                .ToArray();
        }

        public IFeatureModelNode DuplicateNode(IFeatureModelNode nodeToCopy, int index, IFeatureModelNode parentNode = null)
        {
            var newNode = new Node(nodeToCopy);
            _featureModelNodes.Add(newNode);

            var constraintsTo = _constraintsByToNodeId[nodeToCopy.NodeId];
            
            var rel = constraintsTo.First(r => r is IRelationshipConstraint);
            if (parentNode == null)
            {
                parentNode = rel.FromNode;
            }

            if (rel is BinaryRelationshipConstraint binaryRel)
            {
                var newRel = binaryRel.Clone(parentNode, newNode);
                _featureModelConstraints.Insert(index, newRel);
            }
            else if (rel is NaryRelationshipConstraint naryRel)
            {
                naryRel.AddToNode(newNode);
            }

            parentNode.InsertChildren(index, newNode);

            if (_constraintsByFromNodeId.TryGetValue(nodeToCopy.NodeId, out var constraintsFrom))
            {
                foreach (var constraint in constraintsFrom.Where(c => c is not IRelationshipConstraint))
                    _featureModelConstraints.Add(constraint.Clone(newNode));
            }
            
            InitializeLookupLists();

            var childNodes = GetChildren(nodeToCopy);
            for (var i = 0; i < childNodes.Length; i++)
            {
                DuplicateNode(childNodes[i], i, newNode);
            }

            return newNode;
        }

        public IFeatureModelNode GetRoot()
        {
            return GetNodes().OfType<RootNode>().First();
        }

        public IEnumerable<IFeatureModelNode> DepthFirstTraverse()
        {
            var stack = new Stack<IFeatureModelNode>();
            stack.Push(GetRoot());

            while(stack.Any())
            {
                var node = stack.Pop();
                yield return node;
                
                foreach (var child in GetChildren(node).Reverse())
                    stack.Push(child);
            }
        }
    }
    
    public struct NodeProperty
    {
        public readonly IFeatureModelNode Node;
        public readonly string Property;
        public readonly NodePropertyType Type;

        public NodeProperty(IFeatureModelNode node, string property, NodePropertyType type)
        {
            Node = node;
            Property = property;
            Type = type;
        }
    }
        
    public enum NodePropertyType
    {
        Boolean,
        Float,
        Int,
        VariableReference
    }
}