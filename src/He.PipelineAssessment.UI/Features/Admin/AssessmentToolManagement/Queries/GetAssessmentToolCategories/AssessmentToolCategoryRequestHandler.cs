using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategories
{
    public class AssessmentToolCategoryRequestHandler : IRequestHandler<AssessmentToolCategoryQuery, AssessmentToolCategoryListData>
    {
        private readonly IAdminAssessmentToolRepository _adminAssessmentToolRepository;
        private readonly IAssessmentToolMapper _assessmentToolMapper;

        public AssessmentToolCategoryRequestHandler(IAdminAssessmentToolRepository adminAssessmentToolRepository, IAssessmentToolMapper assessmentToolMapper)
        {
            _adminAssessmentToolRepository = adminAssessmentToolRepository;
            _assessmentToolMapper = assessmentToolMapper;
        }

        public async Task<AssessmentToolCategoryListData> Handle(AssessmentToolCategoryQuery query,
            CancellationToken cancellationToken)
        {
            var assessmentToolCategories = _adminAssessmentToolRepository.GetAssessmentToolCategories();
            var assessmentToolCategoriesData =
                _assessmentToolMapper.AssessmentToolCategoriesToAssessmentToolCategoryData(assessmentToolCategories.ToList());
            assessmentToolCategoriesData.AssessmentToolName = query.AssessmentToolName;
            return assessmentToolCategoriesData;
        }
    }
}
