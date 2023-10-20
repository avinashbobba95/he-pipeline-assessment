using He.PipelineAssessment.UI.Features.Admin;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using He.PipelineAssessment.UI.Authorization;

namespace He.PipelineAssessment.UI.Features.WorkflowRegistry
{
    [Authorize]
    public class WorkflowRegistryController : BaseController<WorkflowRegistryController>
    {
        public WorkflowRegistryController(IMediator mediator, ILogger<WorkflowRegistryController> logger) : base(mediator, logger)
        {

        }

        [HttpGet]
        [Authorize(Policy = Constants.AuthorizationPolicies.JWTBearerToken)]
        public async Task<IActionResult> Workflows()
        {
            //TODO - Need to return a new DTO Object
            var assessmentTools = await _mediator.Send(new AssessmentToolQuery());

            return View("AssessmentTool", assessmentTools);
        }
    }
}
