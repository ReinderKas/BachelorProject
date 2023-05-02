using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elfskot.Core.Models.Configurator.Graph.Constraints
{
    public class ArcherObjectiveConstraint : IFeatureModelConstraint
    {
        public Guid Id => Guid.Empty;

        public Guid FromNodeId => Guid.Empty;

        public IFeatureModelNode FromNode => null;

        public bool IsCrossTreeConstraint => true;

        private readonly ArcherObjectiveExpression archerObjectiveExpression;

        public ArcherObjectiveExpression ArcherObjectiveExpression => archerObjectiveExpression;

        public ArcherObjectiveConstraint(ArcherObjectiveExpression archerObjectiveExpression)
        {
            this.archerObjectiveExpression = archerObjectiveExpression;
        }

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            return new ArcherObjectiveConstraint(archerObjectiveExpression);
        }
    }
}
