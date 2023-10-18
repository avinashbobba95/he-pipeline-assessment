using FluentValidation.Results;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.AddAssessmentToolCategory
{
    public class AddAssessmentToolCategoryDataDto
    {
        public AddAssessmentToolCategoryCommand AddAssessmentToolCategoryCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
    }

    public class AddAssessmentToolCategoryCommand : IRequest
    {
        public int Id { get; set; }
    }
}
