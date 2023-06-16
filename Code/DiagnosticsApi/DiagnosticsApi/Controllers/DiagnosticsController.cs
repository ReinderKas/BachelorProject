using Microsoft.AspNetCore.Mvc;
using Microsoft.Z3;
using System.Text.Json.Serialization;
using Z3Parser;
using Z3Parser.FeatureModels;

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


        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> Diagnose([FromBody] string model = null)
        {
            Console.WriteLine("Model: \n" + model);

            if (model == null)
                return BadRequest("No model provided!");

            var solver = new DiagnosticsParser(model);
            solver.ProveModel();

            if (!solver.HasSolution)
            {
                solver.PrintProof();

                var result = solver.Proof.ToString();

                return Ok(result);
            }

            return NotFound($"No proof found for the model {model}");
        }


        /*  
         *  Models:
         *   ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [mandatory1] - excludes -> [mandatory1]
                }");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [mandatory1] - excludes -> [mandatory2]
                }");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [root] - alternative -> [alt1]
                    [root] - alternative -> [alt2]

                    [mandatory1] - excludes -> [alt1]
                    [mandatory2] - excludes -> [alt2]
                }");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [root] - mandatory -> [mandatory3]
                    [root] - alternative -> [alt1]
                    [root] - alternative -> [alt2]
                    [root] - alternative -> [alt3]

                    [mandatory1] - excludes -> [alt1]
                    [mandatory1] - excludes -> [alt2]
                    [mandatory1] - excludes -> [alt3]
                }");

        ProveModel(@" 
                model [root] {
                    [root] - mandatory -> [mandatory1]
                    [root] - mandatory -> [mandatory2]
                    [root] - mandatory -> [mandatory3]
                    [root] - mandatory -> [mandatory4]
                    [root] - mandatory -> [mandatory5]
                    [root] - mandatory -> [mandatory6]
                    [root] - alternative -> [alt1]
                    [root] - alternative -> [alt2]
                    [root] - alternative -> [alt3]
                    [root] - alternative -> [alt4]
                    [root] - alternative -> [alt5]
                    [root] - alternative -> [alt6]

                    [mandatory1] - excludes -> [alt1]
                    [mandatory2] - excludes -> [alt2]
                    [mandatory3] - excludes -> [alt3]
                    [mandatory4] - excludes -> [alt4]
                    [mandatory5] - excludes -> [alt5]
                    [mandatory6] - excludes -> [alt6]
                }");


        ProveModel(@" 
                model [root] {
                    [root] - optional -> [opt1]
                    [root] - mandatory -> [mand1]
                    [root] - mandatory -> [mand2]


                    [mand1] - requires -> [opt1]
                    [opt1] - excludes -> [mand2]
                }");
         */
    }
}
