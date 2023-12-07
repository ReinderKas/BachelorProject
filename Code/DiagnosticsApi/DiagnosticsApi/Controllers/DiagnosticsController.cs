using DiagnosticsApi.Models;
using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Elfsquad.Core.Archer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Z3;
using Z3Parser;
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

            //Console.WriteLine("Returning FM Graph Result Object: " + result.ToString());

            return Ok(result);
        }
    }
}