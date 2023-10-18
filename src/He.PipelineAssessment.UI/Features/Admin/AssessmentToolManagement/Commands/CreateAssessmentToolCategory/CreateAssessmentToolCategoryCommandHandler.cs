using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolCategory
{
    public class CreateAssessmentToolCategoryCommandHandler : IRequestHandler<CreateAssessmentToolCategoryCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;
        private readonly ILogger<CreateAssessmentToolCategoryCommandHandler> _logger;

        public CreateAssessmentToolCategoryCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper, ILogger<CreateAssessmentToolCategoryCommandHandler> logger)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
            _logger = logger;
        }

        public async Task Handle(CreateAssessmentToolCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var assessmentToolCategory = _assessmentToolMapper.CreateAssessmentToolCategoryCommandToAssessmentToolCategory(command);

                // TODO: create assessment tool category in DB
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create assessment tool category.");
            }
        }

    }
}
