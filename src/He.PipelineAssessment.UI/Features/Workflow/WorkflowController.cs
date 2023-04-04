using Elsa.CustomWorkflow.Sdk;
using FluentValidation;
using He.PipelineAssessment.UI.Extensions;
using He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.LoadCheckYourAnswersScreen;
using He.PipelineAssessment.UI.Features.Workflow.LoadConfirmationScreen;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using JorgeSerrano.Json;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using static He.PipelineAssessment.UI.Extensions.FgaAuthorizationHandler;
using System.Net.Http;
using System.Text.Json;

namespace He.PipelineAssessment.UI.Features.Workflow
{
    [Authorize]
    public class WorkflowController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<WorkflowController> _logger;
        private readonly IMediator _mediator;
        private readonly IValidator<QuestionScreenSaveAndContinueCommand> _validator;


        public WorkflowController(HttpClient httpClient, IValidator<QuestionScreenSaveAndContinueCommand> validator, ILogger<WorkflowController> logger, IMediator mediator)
        {
            _httpClient = httpClient;
            _logger = logger;
            _mediator = mediator;
            _validator = validator;
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartWorkflow([FromForm] StartWorkflowCommand command)
        {

            //var userId = User.FindFirst(ClaimConstants.NameIdentifierId).Value;

            var accessToken = await GetTokenAsync();

            try
            {
                var result = await _mediator.Send(command);

                if (result.IsCorrectBusinessArea)
                {

                    return RedirectToAction("LoadWorkflowActivity",
                        new
                        {
                            WorkflowInstanceId = result?.WorkflowInstanceId,
                            ActivityId = result?.ActivityId,
                            ActivityType = result?.ActivityType
                        });
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Error");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        public async Task<IActionResult> LoadWorkflowActivity(QuestionScreenSaveAndContinueCommandResponse request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ActivityType))
                {
                    //try to get activity type from the server?
                }
                switch (request.ActivityType)
                {
                    case ActivityTypeConstants.QuestionScreen:
                        {
                            var questionScreenRequest = new LoadQuestionScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId,
                                IsReadOnly = false
                            };
                            var result = await this._mediator.Send(questionScreenRequest);

                            if (result.IsCorrectBusinessArea)
                            {
                                return View("MultiSaveAndContinue", result);
                            }
                            else
                            {
                                return RedirectToAction("LoadReadOnlyWorkflowActivity", request);

                            }
                        }
                    case ActivityTypeConstants.CheckYourAnswersScreen:
                        {
                            var checkYourAnswersScreenRequest = new LoadCheckYourAnswersScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId,
                                IsReadOnly = false,
                            };

                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            if (result.IsCorrectBusinessArea)
                            {
                                return View("CheckYourAnswers", result);
                            }
                            else
                            {

                                return RedirectToAction("LoadReadOnlyWorkflowActivity", request);

                            }

                        }
                    case ActivityTypeConstants.ConfirmationScreen:
                        {
                            var checkYourAnswersScreenRequest = new LoadConfirmationScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId
                            };

                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            if (result.IsCorrectBusinessArea)
                            {
                                return View("Confirmation", result);
                            }
                            else
                            {
                                return RedirectToAction("LoadReadOnlyWorkflowActivity", request);
                            }
                        }
                    default:
                        throw new ApplicationException(
                            $"Attempted to load unsupported activity type: {request.ActivityType}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToPipelineViewAssessmentRoleRequired)]
        public async Task<IActionResult> LoadReadOnlyWorkflowActivity(QuestionScreenSaveAndContinueCommandResponse request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.ActivityType))
                {
                    //try to get activity type from the server?
                }
                switch (request.ActivityType)
                {
                    case ActivityTypeConstants.ConfirmationScreen:
                        {
                            var checkYourAnswersScreenRequest = new LoadConfirmationScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId
                            };
                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            return View("Confirmation", result);
                        }

                    default:
                        {
                            var checkYourAnswersScreenRequest = new LoadCheckYourAnswersScreenRequest
                            {
                                WorkflowInstanceId = request.WorkflowInstanceId,
                                ActivityId = request.ActivityId,
                                IsReadOnly = true
                            };
                            var result = await this._mediator.Send(checkYourAnswersScreenRequest);

                            return View("CheckYourAnswersReadOnly", result);
                        }

                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> QuestionScreenSaveAndContinue([FromForm] QuestionScreenSaveAndContinueCommand command)
        {
            try
            {
                var validationResult = _validator.Validate(command);
                if (validationResult.IsValid)
                {
                    var result = await this._mediator.Send(command);

                    if (result.IsCorrectBusinessArea)
                    {

                        return RedirectToAction("LoadWorkflowActivity",
                        new
                        {
                            WorkflowInstanceId = result?.WorkflowInstanceId,
                            ActivityId = result?.ActivityId,
                            ActivityType = result?.ActivityType
                        });
                    }
                    else
                    {
                        return RedirectToAction("AccessDenied", "Error");
                    }

                }
                else
                {
                    command.ValidationMessages = validationResult;

                    return View("MultiSaveAndContinue", command);
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }

        [Authorize(Policy = Authorization.Constants.AuthorizationPolicies.AssignmentToWorkflowExecuteRoleRequired)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CheckYourAnswerScreenSaveAndContinue([FromForm] CheckYourAnswersSaveAndContinueCommand command)
        {
            try
            {
                var result = await _mediator.Send(command);

                if (result.IsCorrectBusinessArea)
                {

                    return RedirectToAction("LoadWorkflowActivity",
                        new
                        {
                            WorkflowInstanceId = result?.WorkflowInstanceId,
                            ActivityId = result?.ActivityId,
                            ActivityType = result?.ActivityType
                        });
                }
                else
                {
                    return RedirectToAction("AccessDenied", "Error");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return RedirectToAction("Index", "Error", new { message = e.Message });
            }
        }


        private async Task<string> GetTokenAsync()
        {
            var fgaCredentials = new FgaCredentials
            {
                ClientId = "D4ZLiiwsBq9tMZXbCx2TwBUiNDcsXuIy",
                ClientSecret = "HsE3sWT3kvmsTiH1nUp1CImz9IRb2YxUN6wNopp1RWyKfrvMzeBkOWe9NkT_CG1L",
                Audience = "https://elsa-server-api",
                GrantType = "client_credentials"
            };
            var options = new JsonSerializerOptions { PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy() };

            var response = await _httpClient.PostAsJsonAsync("https://identity-staging-homesengland.eu.auth0.com/oauth/token",
                                                             fgaCredentials,
                                                             options);
            var responseBody = await response.Content.ReadAsStringAsync();

            var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseBody, options);
            return tokenResponse?.AccessToken;
        }

    }
}
