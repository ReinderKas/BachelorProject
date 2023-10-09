using Elfskot.Core.Models.Entities;
using Elfskot.Core.Models.Entities.FeatureModels;
using Elfsquad.Core.Archer;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes
{
    public interface IFeatureModelNode
    {
        bool IsRoot { get; }
        bool IsDefault { get; }
        Guid NodeId { get; }
        FeatureModelNode FeatureModelNode { get; }
        IEnumerable<IFeatureModelNode> ChildNodes { get; }
        public Dictionary<string, object> Properties { get; }

        public int? CardinalityMin { get; }
        public int? CardinalityMax { get; }

        public ArcherVariableType ArcherVariableType { get; }

        void AddChildren(params IFeatureModelNode[] children);
        void InsertChildren(int index, params IFeatureModelNode[] children);
        void SetDefault(bool value);
    }
}
