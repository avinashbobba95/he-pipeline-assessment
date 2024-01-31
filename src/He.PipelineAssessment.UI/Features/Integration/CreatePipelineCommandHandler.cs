using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using MediatR;


namespace He.PipelineAssessment.UI.Features.Integration
{
    public class CreatePipelineCommandHandler : IRequestHandler<CreatePipelineCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ISyncCommandHandlerHelper _syncCommandHandlerHelper;
        private readonly ILogger<SyncCommandHandler> _logger;

        public CreatePipelineCommandHandler(
            IAssessmentRepository assessmentRepository, 
           ISyncCommandHandlerHelper syncCommandHandlerHelper, 
            ILogger<SyncCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _syncCommandHandlerHelper = syncCommandHandlerHelper;
            _logger = logger;
        }

        public string GetValueOrDefault(string value, string defaultValue = "-")
        {
            string result = string.IsNullOrWhiteSpace(value) ? defaultValue : value;
            return result;
        }

        public async Task<int> Handle(CreatePipelineCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var destinationAssessments = await _assessmentRepository.GetAssessments();
                bool canAddRecord = destinationAssessments.FirstOrDefault(p => p.SpId == request.ProjectData.ProjectId) == null;

                if (canAddRecord)
                {
                    var assessment = new Models.Assessment()
                    {
                        SpId = request.ProjectData.ProjectId,
                        BusinessArea = GetValueOrDefault(request.ProjectData.BusinessArea),
                        Counterparty = GetValueOrDefault(request.ProjectData.Counterparty),
                        FundingAsk = request.ProjectData.FundingAsk,
                        LandType = request.ProjectData.LandType,
                        LocalAuthority = GetValueOrDefault(request.ProjectData.LocalAuthority),
                        NumberOfHomes = request.ProjectData.NumberOfHomes,
                        ProjectManager = GetValueOrDefault(request.ProjectData.ProjectManager),
                        ProjectManagerEmail = GetValueOrDefault(request.ProjectData.ProjectManagerEmail),
                        Reference = GetValueOrDefault(request.ProjectData.Reference),
                        SensitiveStatus = GetValueOrDefault(request.ProjectData.SensitiveStatus),
                        SiteName = GetValueOrDefault(request.ProjectData.SiteName),
                        Status = "New"
                    };

                    var assessmentsToBeAdded = new List<Models.Assessment>() {
                        assessment
                    };
                    
                    await _assessmentRepository.CreateAssessments(assessmentsToBeAdded);

                    var insertedRecord = _assessmentRepository.GetAssessments().Result.Single(p => p.SpId == request.ProjectData.ProjectId);

                    return insertedRecord.Id;
                }
                else
                {
                    throw new ApplicationException("Cannot add assessment/pipeline: The record with same SPID is already in the database");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Failed to start workflow");
            }
        }
    }
}
