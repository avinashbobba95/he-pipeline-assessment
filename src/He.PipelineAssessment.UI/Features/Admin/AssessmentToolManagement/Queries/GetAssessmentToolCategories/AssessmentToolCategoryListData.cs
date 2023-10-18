using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategories
{
    public class AssessmentToolCategoryListData
    {
        public string AssessmentToolName { get; set; }
        public List<AssessmentToolCategoryDto> AssessmentToolCategories { get; set; } = new();
    }

    public class AssessmentToolCategoryDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
