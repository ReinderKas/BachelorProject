using DiagnosticsApi.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace DiagnosticsApi.Controllers
{
    [Route("diagnose")]
    [ApiController]
    public class DiagnosticsController : ControllerBase
    {
        private ILogger<DiagnosticsController> _logger;
        private ApiContext _apiContext;

        public DiagnosticsController(
            ILogger<DiagnosticsController> logger,
            ApiContext apiContext
            )
        {
            _logger = logger;
            _apiContext = apiContext;
        }


        [HttpGet]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [ProducesResponseType(500)]
        public async Task<ActionResult<string>> Diagnose([FromQuery] string id)
        {



            return Ok();
        }



    }
}
