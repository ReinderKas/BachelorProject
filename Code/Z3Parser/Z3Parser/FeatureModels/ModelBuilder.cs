using System.Reflection.Metadata.Ecma335;

namespace Z3Parser.FeatureModels
{
    public static class ModelBuilder
    {
        public static async Task<Z3Solver> CreateModel(string model)
        {
            var solver = new Z3Solver();
            await solver.InitializeFeatureModelGraph(model);
            return solver;
        }
    }
}
