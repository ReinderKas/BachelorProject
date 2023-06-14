using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DiagnosticsApi.Controllers
{
    public class ErrorsController : ControllerBase
    {
        public ILogger<ErrorsController> _logger;


        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }


        // This endpoint is called when an uncaught Exception is thrown in the application
        // The configuring of this is done in the 'Startup.cs' file by calling 'app.UseExceptionHandler("/error");'
        [Route("error")]
        [ApiExplorerSettings(IgnoreApi = true)] // Don't show on the Swagger page.
        public IActionResult Error()
        {

            // Following context can apparently be null.
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            if (context != null)
            {
                _logger.LogCritical(
                    context.Error,
                    $"Error thrown in {context.Path}");
            }


            return StatusCode(500);
        }
    }
}