using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Migrations;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Assessment.TestAssessmentSummary;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace He.PipelineAssessment.UI.Features.Assessments
{
    [Authorize]
    public class AssessmentController : Controller
    {
        private readonly ILogger<AssessmentController> _logger;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;


        public AssessmentController(ILogger<AssessmentController> logger, IMediator mediator, IConfiguration configuration)
        {
            _logger = logger;
            _mediator = mediator;
            _configuration = configuration;
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public IActionResult Index()
        {
           // var listModel = await _mediator.Send(new AssessmentListCommand());
            return View("Index");
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        [HttpPost]
        public async Task<IActionResult> GetAssessmentList()
        {
            //ERROR HANDLING TO DO 
            var draw = Request.Form["draw"].FirstOrDefault();
            var start = Request.Form["start"].FirstOrDefault();
            var length = Request.Form["length"].FirstOrDefault();
            var sortColumn = Request.Form["columns[" + Request.Form["order[0][column]"].FirstOrDefault() + "][name]"].FirstOrDefault();
            var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();
            var searchValue = Request.Form["search[value]"].FirstOrDefault();
            int pageSize = length != null ? Convert.ToInt32(length) : 0;
            int skip = start != null ? Convert.ToInt32(start) : 0;
            int recordsTotal = 0;

            var listModel = await _mediator.Send(new AssessmentListCommand());

            // SEARCH AND ORDER BY TO DO 
            recordsTotal = listModel.Count();
            var data = listModel.Skip(skip).Take(pageSize).ToList();
            data.Select(x=> new { 
                SpId = x.SpId,
                SiteName = x.SiteName,
                Counterparty = x.Counterparty,
                LocalAuthority= x.LocalAuthority,
                FundingAskCurrency = x.FundingAskCurrency,
                NumberOfHomesFormatted = x.NumberOfHomesFormatted,
                ProjectManager = x.ProjectManager,
                LastModifiedDateTime = x.LastModifiedDateTime,
                StatusDisplayTag = x.StatusDisplayTag(),

            });

            return Ok(new
            {
                draw = draw,
                recordsTotal = recordsTotal,
                recordsFiltered = recordsTotal,
                data = data
            });
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        [ResponseCache(NoStore = true)]
        public async Task<IActionResult> Summary(int assessmentid, int correlationId)
        {
            var overviewModel = await _mediator.Send(new AssessmentSummaryRequest(assessmentid, correlationId));
            return View("Summary", overviewModel);
        }

        [Authorize(Policy = Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> TestSummary(int assessmentid, int correlationId)
        {
            var enableTestSummaryPage = _configuration["Environment:EnableTestSummaryPage"];
            if (bool.Parse(enableTestSummaryPage))
            {
                var overviewModel = await _mediator.Send(new TestAssessmentSummaryRequest(assessmentid, correlationId));
                return View("TestSummary", overviewModel);
            }
            else
            {
                return RedirectToAction("Summary", new { assessmentid = assessmentid, correlationId = correlationId });
            }
        }
    }
}
