using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Scripts;
using Elfskot.Core.Models.Entities.FeatureModel;
using Elfskot.Core.Models.Entities.FeatureModels;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator
{
    public class FeatureModelTranslator
    {
        private FeatureModel featureModel;
        private IFeatureModelNode rootNode;
        public List<IFeatureModelNode> featureModelNodes { get; private set; }
        public List<IFeatureModelConstraint> featureModelConstraints { get; private set; }

        private List<FeatureModelRelationship> brokenRelationships;
        private List<SubGroupingFeatureModelRelationshipCondition> brokenSubgroupings;

        private List<Guid> MissingNodeIds;
        private Dictionary<Guid, IFeatureModelNode> NodesByNodeId;
        private Dictionary<Guid, IFeatureModelConstraint> ConstraintsById;
        private Dictionary<Guid, FeatureModelRelationship> RelationshipsById;

        private Dictionary<Guid, List<IFeatureModelConstraint>> ConnectedConstraintsByNodeId;

        public FeatureModelTranslator(FeatureModel featureModel)
        {
            this.featureModel = featureModel;
            featureModelNodes = new List<IFeatureModelNode>();
            featureModelConstraints = new List<IFeatureModelConstraint>();
            brokenRelationships = new List<FeatureModelRelationship>();
            brokenSubgroupings = new List<SubGroupingFeatureModelRelationshipCondition>();
            NodesByNodeId = new Dictionary<Guid, IFeatureModelNode>();
            ConstraintsById = new Dictionary<Guid, IFeatureModelConstraint>();
            RelationshipsById = new Dictionary<Guid, FeatureModelRelationship>();
            MissingNodeIds = new List<Guid>();
            ConnectedConstraintsByNodeId = new Dictionary<Guid, List<IFeatureModelConstraint>>();
            InitializeNodes();
            InitializeConstraints();
        }

        public FeatureModelGraph BuildGraph()
        {
            return new FeatureModelGraph(featureModelNodes, featureModelConstraints, new List<IFeatureModelFunctionCall>());
        }

        public static FeatureModelGraph BuildGraph(FeatureModel featureModel)
        {
            return new FeatureModelTranslator(featureModel).BuildGraph();
        }
        
        private void InitializeNodes()
        {
            var nodes = new List<IFeatureModelNode>();

            var root = new RootNode(featureModel.RootNode);
            rootNode = root;
            nodes.Add(root);
            NodesByNodeId[root.NodeId] = root;
            ConnectedConstraintsByNodeId[root.NodeId] = new List<IFeatureModelConstraint>();
            foreach (var node in featureModel.AllNodes)
            {
                if (node.Id != root.NodeId)
                {
                    var fmNode = new Node(node);
                    nodes.Add(fmNode);
                    NodesByNodeId[node.Id] = fmNode;
                    ConnectedConstraintsByNodeId[node.Id] = new List<IFeatureModelConstraint>();
                }
            }
            featureModelNodes = nodes;
        }

        private void InitializeConstraints()
        {
            // We can initialize Expressions at this stage without knowing the children.
            // All that is necessary is the affected Node and its Expression.

            InitializeNodeExpressionConstraints();
            InitializeBinaryConstraints();

            var alternatives = featureModel.AllRelationships.Where(r => r.Type == FeatureModelRelationshipTypes.Alternative);
            var ors = featureModel.AllRelationships.Where(r => r.Type == FeatureModelRelationshipTypes.Or);

            InitializeNaryConstraints(alternatives);
            InitializeNaryConstraints(ors);

        }

        private void InitializeBinaryConstraints()
        {
            brokenRelationships = new List<FeatureModelRelationship>();
            foreach (var relationship in featureModel.AllRelationships)
            {
                try
                {
                    // TODO: Default
                    //// this can be done for each individual Relationship
                    //if (relationship.Default)
                    //    NodesByNodeId[(Guid)relationship.ToNodeId].SetDefault(true);

                    RelationshipsById[relationship.Id] = relationship;
                    BinaryRelationshipConstraint constraint;

                    // Only initialize Binary types here.
                    switch (relationship.Type)
                    {
                        case FeatureModelRelationshipTypes.Optional:
                            featureModelConstraints.Add(InitializeOptionalConstraint(relationship));
                            featureModelConstraints.AddRange(InitializeRelationshipConditions(relationship));
                            break;
                        case FeatureModelRelationshipTypes.Mandatory:
                            featureModelConstraints.Add(InitializeMandatoryConstraint(relationship));
                            featureModelConstraints.AddRange(InitializeRelationshipConditions(relationship));
                            break;
                        case FeatureModelRelationshipTypes.Required:
                            featureModelConstraints.Add(InitializeRequiresConstraints(relationship));
                            break;
                        case FeatureModelRelationshipTypes.Excludes:
                            featureModelConstraints.Add(InitializeExcludeConstraints(relationship));
                            break;
                        case FeatureModelRelationshipTypes.Implies:
                            if (relationship.Expression == null)
                                featureModelConstraints.Add(InitializeImpliesConstraint(relationship));
                            else
                                if(relationship.FromNodeId == null || !NodesByNodeId.ContainsKey((Guid)relationship.FromNodeId))
                                    brokenRelationships.Add(relationship);
                                else 
                                    featureModelConstraints.Add(InitializeCalculatedValueConstraint(NodesByNodeId[(Guid)relationship.FromNodeId], relationship.Expression));
                            break;
                    }
                }
                catch (Exception e)
                {
                    /*  Cannot translate the relationship to a CSP Constraint for some reason.                     *  
                     *  If a Condition or Subgrouping fails to be created, it will be caught in InitializeRelationshipConditions() instead 
                     */
                    brokenRelationships.Add(relationship);
                }
            }
        }

        private void InitializeNaryConstraints(IEnumerable<FeatureModelRelationship> relationships)
        {
            foreach (var group in relationships.GroupBy(r => (Guid)r.FromNodeId))
            {
                try
                {
                    if (group.First().Type == FeatureModelRelationshipTypes.Alternative)
                        featureModelConstraints.Add(InitializeAlternativeConstraint(group.ToArray()));
                    else
                        featureModelConstraints.Add(InitializeOrConstraint(group.ToArray()));


                    featureModelConstraints.AddRange(InitializeRelationshipConditions(group.ToArray()));
                }
                catch
                {
                    brokenRelationships.AddRange(group);
                }
            }
        }

        private IEnumerable<IFeatureModelConstraint> InitializeRelationshipConditions(params FeatureModelRelationship[] relationships)
        {
            var result = new List<IFeatureModelConstraint>();
            foreach (var rel in relationships)
            {
                RelationshipsById[rel.Id] = rel;

                if (rel.Conditions.Any())
                {
                    var subGroupings = rel.Conditions.OfType<SubGroupingFeatureModelRelationshipCondition>();
                    var conditions = rel.Conditions.Where(c => c.ConditionExpression != null);

                    result.AddRange(InitializeSubgroupingConstraint(subGroupings));
                    result.AddRange(InitializeConditionConstraint(rel, conditions));
                }
            }
            return result;
        }

        private void InitializeNodeExpressionConstraints()
        {
            //TODO: Expressions
        }

        private BinaryRelationshipConstraint AddBinaryConnection(BinaryRelationshipConstraint binary)
        {
            SetConnection(binary.FromNodeId, binary);
            SetConnection(binary.ToNodeId, binary);
            ConstraintsById[binary.Id] = binary;

            return binary;
        }

        private NaryRelationshipConstraint AddNaryConnection(NaryRelationshipConstraint nary)
        {
            SetConnection(nary.FromNodeId, nary);
            foreach (var node in nary.ToNodeIds)
                SetConnection(node, nary);
            
            ConstraintsById[nary.Id] = nary;

            return nary;
        }

        private ExpressionConstraint AddExpressionConnection(ExpressionConstraint expression)
        {
            SetConnection(expression.FromNodeId, expression);
            foreach (var argument in expression.Expression.Variables)
            {
                if (argument.Children)
                {
                    if (!NodesByNodeId.ContainsKey(argument.NodeId))
                    {
                        MissingNodeIds.Add(argument.NodeId);
                        continue;
                    }

                    foreach (var child in NodesByNodeId[argument.NodeId].ChildNodes)
                    {
                        SetConnection(child.NodeId, expression);
                    }                        
                }
                else
                {
                    SetConnection(argument.NodeId, expression);
                }
            }

            ConstraintsById[expression.Id] = expression;

            return expression;
        }

        private void SetConnection(Guid nodeId, IFeatureModelConstraint constraint)
        {
            try
            {
                if (ConnectedConstraintsByNodeId.ContainsKey(nodeId))
                    ConnectedConstraintsByNodeId[nodeId].Add(constraint);
                else 
                    MissingNodeIds.Add(nodeId);
            }
            catch
            {
                // Something went horribly wrong if this gets hit...
            }
        }

        private NaryRelationshipConstraint InitializeOrConstraint(FeatureModelRelationship[] rels)
        {
            var parent = NodesByNodeId[(Guid)rels.First().FromNodeId];
            var children = rels.Select(r => NodesByNodeId[(Guid)r.ToNodeId]);

            return AddNaryConnection(new OrFeatureModelConstraint(parent.NodeId, parent, children));
        }

        private NaryRelationshipConstraint InitializeAlternativeConstraint(FeatureModelRelationship[] rels)
        {
            var parent = NodesByNodeId[(Guid)rels.First().FromNodeId];
            var children = rels.Select(r => NodesByNodeId[(Guid)r.ToNodeId]);

            return AddNaryConnection(new AlternativeFeatureModelConstraint(parent.NodeId, parent, children));
        }

        private BinaryRelationshipConstraint InitializeMandatoryConstraint(FeatureModelRelationship rel)
            => AddBinaryConnection(new MandatoryFeatureModelConstraint(rel.Id, NodesByNodeId[(Guid)rel.FromNodeId], NodesByNodeId[(Guid)rel.ToNodeId]));

        private BinaryRelationshipConstraint InitializeOptionalConstraint(FeatureModelRelationship rel)
            => AddBinaryConnection(new OptionalFeatureModelConstraint(rel.Id, NodesByNodeId[(Guid)rel.FromNodeId], NodesByNodeId[(Guid)rel.ToNodeId]));


        // TODO: Hide in View
        private BinaryRelationshipConstraint InitializeExcludeConstraints(FeatureModelRelationship rel)
            => AddBinaryConnection(new ExcludesFeatureModelConstraint(rel.Id, NodesByNodeId[(Guid)rel.FromNodeId], NodesByNodeId[(Guid)rel.ToNodeId], false));


        // TODO: Default Selector
        private BinaryRelationshipConstraint InitializeRequiresConstraints(FeatureModelRelationship rel)
            => AddBinaryConnection(new RequiresFeatureModelConstraint(rel.Id, NodesByNodeId[(Guid)rel.FromNodeId], NodesByNodeId[(Guid)rel.ToNodeId], false));


        // TODO: Simple Calculated Value 
        private BinaryRelationshipConstraint InitializeImpliesConstraint(FeatureModelRelationship rel)
            => AddBinaryConnection(new SimpleCalculatedValueFeatureModelConstraint(
                                        rel.Id,
                                        NodesByNodeId[(Guid)rel.FromNodeId],
                                        NodesByNodeId[(Guid)rel.ToNodeId],
                                        1,
                                        1,
                                        ImpliesRelationshipTypes.Ratio));
        private ExpressionConstraint InitializeCalculatedValueConstraint(IFeatureModelNode affectedNode, Expression expression)
            => AddExpressionConnection(new AdvancedCalculatedValueFeatureModelConstraint(affectedNode, expression));

        private ExpressionConstraint InitializeMaxValueConstraint(IFeatureModelNode affectedNode, Expression expression)
            => AddExpressionConnection(new MaxValueFeatureModelConstraint(affectedNode, expression));

        private ExpressionConstraint InitializeDefaultValueConstraint(IFeatureModelNode affectedNode, Expression expression)
            => AddExpressionConnection(new DefaultValueFeatureModelConstraint(affectedNode, expression));

        private IEnumerable<ExpressionConstraint> InitializeConditionConstraint(FeatureModelRelationship relationship, IEnumerable<FeatureModelRelationshipCondition> conditions)
        {
            if (relationship.ToNodeId == null || !NodesByNodeId.TryGetValue((Guid)relationship.ToNodeId, out var node))
                return Array.Empty<ExpressionConstraint>();
            
            var result = new List<ExpressionConstraint>();
            foreach (var con in conditions)
                result.Add(AddExpressionConnection(new ConditionFeatureModelConstraint(Guid.NewGuid(), node, con.ConditionExpression)));
            
            return result;
        }

        private IEnumerable<BinaryRelationshipConstraint> InitializeSubgroupingConstraint(IEnumerable<SubGroupingFeatureModelRelationshipCondition> subgroupings)
        {
            var result = new List<BinaryRelationshipConstraint>();
            foreach (var sg in subgroupings)
            {
                try
                {
                    var relationship = RelationshipsById[sg.FeatureModelRelationshipId];
                    var fromNode = NodesByNodeId[(Guid)sg.FromNodeId];
                    var toNode = NodesByNodeId[(Guid)relationship.ToNodeId];

                    result.Add(AddBinaryConnection(new SubgroupingFeatureModelConstraint(Guid.NewGuid(), fromNode, toNode)));
                }
                catch
                {
                    brokenSubgroupings.Add(sg);
                }
            }
            return result;
        }
    }
}