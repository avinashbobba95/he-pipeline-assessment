using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategories;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategoriesForAssessmentTool
{
    public class AssessmentToolCategoryForAssessmentToolListData
    {
        public string AssessmentToolName { get; set; }
        public List<AssessmentToolCategoryForAssessmentToolDto> AssessmentToolCategories { get; set; } = new();
    }

    public class AssessmentToolCategoryForAssessmentToolDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
