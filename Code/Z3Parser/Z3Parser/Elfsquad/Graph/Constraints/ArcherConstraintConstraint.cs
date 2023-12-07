using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;
using Elfsquad.Core.Archer;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints
{
    public class ArcherFeatureModelConstraint : IFeatureModelConstraint
    {
        private readonly ArcherConstraint _archerConstraint;
        public Guid Id => Guid.Empty;
        public Guid FromNodeId => Guid.Empty;
        public IFeatureModelNode FromNode => null;
        public bool IsCrossTreeConstraint => true;
        public ArcherConstraint ArcherConstraint => _archerConstraint;
        
        public ArcherFeatureModelConstraint(ArcherConstraint archerConstraint)
        {
            _archerConstraint = archerConstraint;
        }

        public IFeatureModelConstraint Clone(IFeatureModelNode newFromNode = null)
        {
            return new ArcherFeatureModelConstraint(_archerConstraint);
        }
    }
}