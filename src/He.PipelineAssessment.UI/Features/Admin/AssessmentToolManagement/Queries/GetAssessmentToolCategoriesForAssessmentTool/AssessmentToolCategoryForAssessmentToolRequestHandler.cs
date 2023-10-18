using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategoriesForAssessmentTool
{
    public class AssessmentToolCategoryForAssessmentToolRequestHandler : IRequestHandler<AssessmentToolCategoryForAssessmentToolQuery, AssessmentToolCategoryForAssessmentToolListData>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;

        public AssessmentToolCategoryForAssessmentToolRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
        }

        public async Task<AssessmentToolCategoryForAssessmentToolListData> Handle(AssessmentToolCategoryForAssessmentToolQuery query,
            CancellationToken cancellationToken)
        {
            var assessmentToolCategories = new List<AssessmentToolCategory>() {new AssessmentToolCategory() { Name = "BIL"} };
            var assessmentToolCategoriesData =
                _assessmentToolMapper.AssessmentToolCategoriesForAssessmentToolToAssessmentToolCategoryForAssessmentToolData(assessmentToolCategories.ToList());
            return assessmentToolCategoriesData;
        }
    }
}
