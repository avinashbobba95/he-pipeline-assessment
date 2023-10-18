using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.AddAssessmentToolCategory
{
    public class AddAssessmentToolCategoryCommandHandler : IRequestHandler<AddAssessmentToolCategoryCommand>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;
        private readonly ILogger<AddAssessmentToolCategoryCommandHandler> _logger;

        public AddAssessmentToolCategoryCommandHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper, ILogger<AddAssessmentToolCategoryCommandHandler> logger)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
            _logger = logger;
        }

        public async Task Handle(AddAssessmentToolCategoryCommand command, CancellationToken cancellationToken)
        {
            try
            {

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Unable to create assessment tool.");
            }
        }

    }
}

