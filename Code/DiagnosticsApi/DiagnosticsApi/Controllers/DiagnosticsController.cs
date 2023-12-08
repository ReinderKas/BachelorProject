using Castle.Core.Resource;
using DiagnosticsApi.Models;
using Elfskot.Core.Masterdata.FeatureModels.Translator.Constraints;
using Elfskot.Core.Models.Entities.FeatureModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Z3;
using Newtonsoft.Json.Linq;
using Z3Parser;
using Z3Parser.FeatureModels;
using Z3Parser.Z3Constraints;

namespace DiagnosticsApi.Controllers
{
    [Route("diagnose")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        private ILogger<DiagnosticsController> _logger;

        public DiagnosticsController(ILogger<DiagnosticsController> logger)
        {
            _logger = logger;
        }


        [HttpPut("proof")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> GetProof([FromBody] string model = null)
        {
            if (model == null)
                return BadRequest("No model provided!");

            var solver = new DiagnosticsParser(model);
            solver.ProveModel();

            if (!solver.HasSolution)
            {
                solver.PrintProof();

                var result = solver.Proof().ToString();

                return Ok(result);
            }

            return NotFound($"The model seems satisfiable.");
        }


        [HttpPut("unsatCore")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UnsatCoreResult>> GetUnsatCore([FromBody] string model = null)
        {
            if (model == null)
                return BadRequest("No model provided!");

            var solver = new DiagnosticsParser(model);
            solver.ProveModel();

            if (!solver.HasSolution)
            {
                var diagnoseResult = solver.FastDiagnose();

                Console.WriteLine($"\nDiagnostics Result: (Count: {diagnoseResult.Count()})");
                foreach(var diag in diagnoseResult)
                    Console.WriteLine($"{diag.GetType()} - {diag.Expression}");

                var result = new UnsatCoreResult(solver.UnsatCore(),
                                                solver.UnsatCoreArguments(),
                                                diagnoseResult);

                return Ok(result);
            }

            return NotFound(new UnsatCoreResult(Array.Empty<Expr>(),
                                                Array.Empty<Expr>(),
                                                Array.Empty<AConstraint>()));
        }


        [HttpPut("featureModel")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<FmGraphResult>> FeatureModel([FromBody] string model = null)
        {
            if (model == null)
                return BadRequest("No model provided!");

            var solver = new DiagnosticsParser(model);
            Console.WriteLine(solver.ModelSolver.FmGraph.ToString());


            var result = new FmGraphResult(
                solver.ModelSolver.FmGraph.GetNodes(),
                solver.ModelSolver.FmGraph.GetConstraints()
            );

            return Ok(result);
        }

        [HttpPut("options/{nodeName}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<DiagnoseOptions[]>> Options(string nodeName, [FromBody] string model)
        {
            // TODO: Very naive atm.
            var result = new List<DiagnoseOptions>() { DiagnoseOptions.Optional };

            var solver = new DiagnosticsParser(model);
            solver.ProveModel();

            if (solver.HasSolution)
                return BadRequest("This model should not have a solution if this end-point is reached.");

            var relXTreeConstraints = solver.ModelSolver.FmGraph.GetConstraints()
                                                            .OfType<IRelationshipConstraint>()
                                                            .Where(r => r.IsCrossTreeConstraint);

            var connectedConstraints = relXTreeConstraints.Where(r => r.FromNode.FeatureModelNode.Name == nodeName
                                                                    || r.ToNodes().Any(toNode => toNode.FeatureModelNode.Name == nodeName));


            var nodeNames = solver.ModelSolver.FmGraph.GetNodes().Select(n => n.FeatureModelNode.Name);

            foreach (var node in nodeNames) {
                Console.WriteLine(node);
            }





            Console.WriteLine($"Count of X-Tree Constraints for {nodeName} --> {connectedConstraints.Count()}");


            /*  TODO: Very naive,
             *      Right now if any cross tree rels are found, it's able to delete.
             */
            if (connectedConstraints.Any())
                result.Add(DiagnoseOptions.Delete);
            

            return Ok(result);
        }
    }
}