using DiagnosticsApi.Models;
using Elfskot.Core.Masterdata.FeatureModels.Translator;
using Elfsquad.Core.Archer;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Z3;
using Z3Parser;

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


        [HttpGet("proof")]
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
        

        [HttpGet("unsatCore")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<UnsatCoreResult>> GetUnsatCore([FromBody] string model = null)
        {
            if (model == null)
                return BadRequest("No model provided!");

            var solver = new DiagnosticsParser(model);
            solver.ProveModel();

            // Doesn't work for solveable model?
            if (!solver.HasSolution)
            {
                solver.PrintProof();

                // TODO: Make class, not string
                var result = new UnsatCoreResult(solver.UnsatCore(),
                                                solver.UnsatCoreArguments(),
                                                solver.ModelSolver.Variables,
                                                Array.Empty<ConstraintResult>());

                return Ok(result);
            }

            return NotFound(new UnsatCoreResult(Array.Empty<Expr>(),
                                                Array.Empty<Expr>(),
                                                Array.Empty<NodeProperty>(),
                                                Array.Empty<ConstraintResult>()));
        }

        [HttpGet("featureModel")]
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

            Console.WriteLine("Returning FM Graph Result Object: " + result.ToString());

            return Ok(result);
        }
    }
}