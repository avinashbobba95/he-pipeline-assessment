using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace He.PipelineAssessment.UI.Features.Integration
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]/[action]")]
    public class IntegrationController : Controller
    {
        private readonly ILogger<IntegrationController> _logger;
        private readonly IMediator _mediator;


        public IntegrationController(ILogger<IntegrationController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProject([FromBody] ProjectDTO data)
        {
            try
            {
                var command = new CreatePipelineCommand()
                {
                    ProjectData = data
                };

                var result = await _mediator.Send(command);
                return new OkResult();
            }
            catch (Exception ex)
            {
                var badRequestResult = new BadRequestObjectResult(ex.Message);
                return badRequestResult;
            }
        }

        [HttpGet]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> GetAssessments([FromBody] ProjectDTO data)
        {
            var command = new CreatePipelineCommand()
            {
                ProjectData = data
            };

            var result = await _mediator.Send(command);
            return new OkResult();
        }
    }
}
