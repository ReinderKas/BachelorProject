using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfskot.Core.Models.Entities.FeatureModel;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public interface IFeatureModelConstraint
    {
        Guid Id { get; }
        Guid FromNodeId { get; }
        IFeatureModelNode FromNode { get; }
        bool IsCrossTreeConstraint { get; }
        IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null);
    }

    public interface IExpressionConstraint : IFeatureModelConstraint
    {
        Expression Expression { get; }
    }

    public interface IRelationshipConstraint : IFeatureModelConstraint
    {
        public IEnumerable<Guid> ToNodeIds();
        public IEnumerable<IFeatureModelNode> ToNodes();
    }
}
