using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategories
{
    public class AssessmentToolCategoryQuery : IRequest<AssessmentToolCategoryListData>
    {
        public string AssessmentToolName { get; set; }
    }
}
