using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace DiagnosticsApi.Models
{
    [Serializable]
    public class NodeResult
    {
        public string Name;
        public Guid Id;

        public NodeResult(IFeatureModelNode node)
        {
            Name = node.FeatureModelNode.Name;
            Id = node.NodeId;
        }

        public NodeResult(string name)
        {
            Name = name;
            Id = Guid.NewGuid();
        }
    }
}
