using Elfskot.Core.Masterdata.FeatureModels.Translator.Nodes;

namespace Elfskot.Core.Masterdata.FeatureModels.Translator.Scripts
{
    public class PythonFeatureModelFunctionCall : IFeatureModelFunctionCall
    {
        public NodeProperty[] Arguments { get; set;  }
        public NodeProperty[] AffectedNodes { get; set; }
        public string Script { get; }

        public PythonFeatureModelFunctionCall(NodeProperty[] arguments, NodeProperty[] affectedNodes, string script)
        {
            Arguments = arguments;
            AffectedNodes = affectedNodes;
            Script = script;
        }
    }
}