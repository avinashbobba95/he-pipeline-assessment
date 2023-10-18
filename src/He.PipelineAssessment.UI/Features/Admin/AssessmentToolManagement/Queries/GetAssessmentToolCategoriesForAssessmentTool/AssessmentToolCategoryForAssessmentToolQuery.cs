using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategoriesForAssessmentTool
{
    public class AssessmentToolCategoryForAssessmentToolQuery : IRequest<AssessmentToolCategoryForAssessmentToolListData>
    {
        public int SelectedCategory { get; set; }
        public List<AssessmentToolCategory> AssessmentToolCategories {get; set;}
    }
}
