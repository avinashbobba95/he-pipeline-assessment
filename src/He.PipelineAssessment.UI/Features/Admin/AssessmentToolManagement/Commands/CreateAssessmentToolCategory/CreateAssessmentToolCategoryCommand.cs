using MediatR;
using FluentValidation.Results;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolCategory
{
    public class CreateAssessmentToolCategoryDto
    {
        public CreateAssessmentToolCategoryCommand CreateAssessmentToolCategoryCommand { get; set; } = new();
        public ValidationResult? ValidationResult { get; set; }
    }

    public class CreateAssessmentToolCategoryCommand : IRequest
    {
         public int Id { get; set; }
         public string Name { get; set; } = string.Empty;
     }
}
