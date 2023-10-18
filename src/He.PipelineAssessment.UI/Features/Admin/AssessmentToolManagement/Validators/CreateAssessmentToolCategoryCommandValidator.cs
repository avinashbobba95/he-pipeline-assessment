using FluentValidation;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolCategory;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Validators
{
    public class CreateAssessmentToolCategoryCommandValidator: AbstractValidator<CreateAssessmentToolCategoryCommand>
    {
        public CreateAssessmentToolCategoryCommandValidator()
        {
            RuleFor(c => c.Name).NotEmpty().WithMessage("The {PropertyName} cannot be empty");
        }
    }
}
