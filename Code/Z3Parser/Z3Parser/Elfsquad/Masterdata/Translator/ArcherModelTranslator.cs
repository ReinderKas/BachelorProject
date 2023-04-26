using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints.Relationships;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Scripts;
using Elfskot.Core.Models.Configurator.Graph.Constraints;
using Elfskot.Core.Models.Entities;
using Elfskot.Core.Models.Entities.FeatureModels;
using Elfsquad.Core.Archer;
using Node = Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes.Node;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator
{
    public class ArcherModelTranslator
    {
        private ArcherModel _archerModel;
        private List<IFeatureModelNode> featureModelNodes;
        private List<IFeatureModelConstraint> featureModelConstraints;
        private List<IFeatureModelFunctionCall> _featureModelFunctionCalls;
        private Dictionary<ArcherVariable, IFeatureModelNode> _nodesByVariable = new ();


        public ArcherModelTranslator()
        {
        }
        
        public async Task<FeatureModelGraph> BuildGraphAsync(ArcherModel archerModel)
        {
            _archerModel = archerModel;
            featureModelNodes = new List<IFeatureModelNode>();
            featureModelConstraints = new List<IFeatureModelConstraint>();
            _featureModelFunctionCalls = new List<IFeatureModelFunctionCall>();

            InitializeNodes();
            InitializeConstraints();
            InitializeConditions();
            InitializeBoundsConstraints();
            IntializeConstraints();
            InitializeObjectives();

            return new FeatureModelGraph(featureModelNodes, featureModelConstraints, _featureModelFunctionCalls);
        }

        private void InitializeNodes()
        {
            foreach (var variable in _archerModel.Variables)
            {
                var featureId = variable.Object.GetValue("FeatureId") as string;
                var fmNode = new FeatureModelNode
                {
                    Id = Guid.NewGuid(),
                };
                IFeatureModelNode node;
                if (variable == _archerModel.RootVariable)
                    node = new RootNode(fmNode, variable);
                else 
                    node = new Node(fmNode, variable);

                foreach (var pair in variable.Object.Pairs)
                {
                    node.Properties[pair.Key] = pair.Value;
                }
                
                featureModelNodes.Add(node);
                _nodesByVariable[variable] = node;
            }
        }
        
        private void InitializeConstraints()
        {
            InitializeBinaryConstraints();
            InitializeNaryConstraints();
        }

        private void InitializeBinaryConstraints()
        {
            var relationships = _archerModel.Relationships.Where(r => !r.IsNary).ToList();
            foreach (var relationship in relationships)
            {
                var parentNode = _nodesByVariable[relationship.Parent];
                var childNode = _nodesByVariable[relationship.Child];

                switch (relationship.Type)
                {
                    case ArcherRelationshipType.Mandatory:
                        featureModelConstraints.Add(new MandatoryFeatureModelConstraint(Guid.NewGuid(), parentNode, childNode));
                        break;
                    case ArcherRelationshipType.Optional:
                        featureModelConstraints.Add(new OptionalFeatureModelConstraint(Guid.NewGuid(), parentNode, childNode));
                        break;
                    case ArcherRelationshipType.Excludes:
                        featureModelConstraints.Add(new ExcludesFeatureModelConstraint(Guid.NewGuid(), parentNode, childNode, false));
                        break;
                    case ArcherRelationshipType.Requires:
                        featureModelConstraints.Add(new RequiresFeatureModelConstraint(Guid.NewGuid(), parentNode, childNode, false));
                        break;
                    case ArcherRelationshipType.Suggests:
                        featureModelConstraints.Add(new RequiresFeatureModelConstraint(Guid.NewGuid(), parentNode, childNode, true));
                        break;
                    case ArcherRelationshipType.Removes:
                        featureModelConstraints.Add(new ExcludesFeatureModelConstraint(Guid.NewGuid(), parentNode, childNode, true));
                        break;
                    case ArcherRelationshipType.Filters:
                        // Filters relationships are converted to Excludes constraint in the InitializeConditions method.
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        private void InitializeNaryConstraints()
        {
            var relationships = _archerModel.Relationships.Where(r => r.IsNary).ToList();
            foreach (var group in relationships.GroupBy(r => r.Parent))
            {
                var parentNode = _nodesByVariable[group.Key];
                foreach (var subgroup in group.GroupBy(g => g.Type))
                {
                    var children = subgroup.Select(s => _nodesByVariable[s.Child]).ToArray();
                    switch (subgroup.Key)
                    {
                        case ArcherRelationshipType.Alternative:
                            featureModelConstraints.Add(new AlternativeFeatureModelConstraint(Guid.NewGuid(), parentNode, children));
                            break;
                        case ArcherRelationshipType.Or:
                            featureModelConstraints.Add(new OrFeatureModelConstraint(Guid.NewGuid(), parentNode, children));
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                }
            }
        }

        private void InitializeBoundsConstraints()
        {
            foreach (var variable in _archerModel.Variables)
            {
                var node = _nodesByVariable[variable];

                foreach (var lowerBound in variable.LowerBoundExpressions)
                    featureModelConstraints.Add(new LowerBoundConstraint(lowerBound, node));
                
                foreach (var upperBound in variable.UpperBoundExpressions)
                    featureModelConstraints.Add(new UpperBoundConstraint(upperBound, node));
            }
        }

        private void InitializeConditions()
        {
            foreach (var variable in _archerModel.Variables)
            {
                var node = _nodesByVariable[variable];
                foreach (var condition in variable.Conditions)
                    featureModelConstraints.Add(new ArcherConditionConstraint(condition, node));
            }

            var filterRelationships = _archerModel.Relationships
                .Where(r => r.Type == ArcherRelationshipType.Filters)
                .ToArray();
            
            foreach (var group in filterRelationships.GroupBy(r => r.Parent))
            {
                var excludedNodes = group
                    .SelectMany(g => _archerModel.Children(_archerModel.Parent(g.Child)))
                    .Distinct()
                    .Except(group.Select(g => g.Child))
                    .ToArray();
                
                foreach (var excludedNode in excludedNodes)
                {
                    featureModelConstraints.Add(new ExcludesFeatureModelConstraint(Guid.NewGuid(), _nodesByVariable[group.Key], _nodesByVariable[excludedNode], true));
                }
            }
        }

        private void IntializeConstraints()
        {
            foreach (var constraint in _archerModel.Constraints)
            {
                featureModelConstraints.Add(new ArcherFeatureModelConstraint(constraint));
            }
        }

        private void InitializeObjectives()
        {
            foreach (var objective in _archerModel.Objectives)
            {
                featureModelConstraints.Add(new ArcherObjectiveConstraint(objective));
            }
        }
    }
}
