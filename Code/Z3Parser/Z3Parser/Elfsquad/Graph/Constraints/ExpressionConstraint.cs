using Elfskot.Core.Models.Entities.FeatureModel;
using System;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public abstract class ExpressionConstraint : IExpressionConstraint
    {
        private IFeatureModelNode affectedNode { get; set; }
        private readonly Expression _expression;
        public Guid Id => _expression.Id;
        public Guid FromNodeId => affectedNode.NodeId;
        public IFeatureModelNode FromNode => affectedNode;
        public Expression Expression => _expression;

        public ExpressionConstraint(IFeatureModelNode affectedNode, Expression expression)
        {
            this.affectedNode = affectedNode;
            this._expression = expression;
        }

        public bool IsCrossTreeConstraint => true;

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            var clone = (ExpressionConstraint)MemberwiseClone();
            clone.affectedNode = newFromNode;
            return clone;
        }
    }
}
