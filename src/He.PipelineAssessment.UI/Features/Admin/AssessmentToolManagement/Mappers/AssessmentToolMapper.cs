using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentToolCategory;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategories;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolCategoriesForAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;

namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers
{
    public interface IAssessmentToolMapper
    {
        AssessmentToolListData AssessmentToolsToAssessmentToolData(List<AssessmentTool> assessmentTools);
        AssessmentTool CreateAssessmentToolCommandToAssessmentTool(CreateAssessmentToolCommand createAssessmentToolCommand);
        AssessmentToolCategory CreateAssessmentToolCategoryCommandToAssessmentToolCategory(CreateAssessmentToolCategoryCommand createAssessmentToolCategoryCommand);

        List<Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto> AssessmentToolWorkflowsToAssessmentToolDto(List<AssessmentToolWorkflow> toList);
        AssessmentToolCategoryListData AssessmentToolCategoriesToAssessmentToolCategoryData(List<AssessmentToolCategory> assessmentToolCategories);
        AssessmentToolCategoryForAssessmentToolListData AssessmentToolCategoriesForAssessmentToolToAssessmentToolCategoryForAssessmentToolData(List<AssessmentToolCategory> assessmentToolCategories);
    }
    public class AssessmentToolMapper : IAssessmentToolMapper
    {
        public AssessmentToolListData AssessmentToolsToAssessmentToolData(List<AssessmentTool> assessmentTools)
        {
            return new AssessmentToolListData
            {
                AssessmentTools = assessmentTools.Select(x => new AssessmentToolDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Order = x.Order,
                    AssessmentToolWorkFlows = x.AssessmentToolWorkflows != null ? x.AssessmentToolWorkflows.Select(y => new AssessmentToolWorkflowDto
                    {
                        Id = y.Id,
                        Name = y.Name,
                        AssessmentToolId = y.AssessmentToolId,
                        IsFirstWorkflow = y.IsFirstWorkflow,
                        IsEconomistWorkflow = y.IsEconomistWorkflow,
                        IsLatest = y.IsLatest,
                        WorkflowDefinitionId = y.WorkflowDefinitionId,
                        Version = y.Version,

                    }).ToList() : new List<AssessmentToolWorkflowDto> { }
                }).ToList(),
            };
        }

        public AssessmentTool CreateAssessmentToolCommandToAssessmentTool(CreateAssessmentToolCommand createAssessmentToolCommand)
        {
            return new AssessmentTool
            {
                Name = createAssessmentToolCommand.Name,
                Order = createAssessmentToolCommand.Order,
                IsVisible = true
            };
        }

        public List<Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto> AssessmentToolWorkflowsToAssessmentToolDto(List<AssessmentToolWorkflow> assessmentToolWorkflows)
        {
            return assessmentToolWorkflows.Where(x => x.Status != AssessmentToolStatus.Deleted).Select(x =>
                new Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    AssessmentToolId = x.AssessmentToolId,
                    IsFirstWorkflow = x.IsFirstWorkflow,
                    IsEconomistWorkflow = x.IsEconomistWorkflow,
                    IsLatest = x.IsLatest,
                    Version = x.Version,
                    WorkflowDefinitionId = x.WorkflowDefinitionId
                }).ToList();
        }

        public AssessmentToolCategory CreateAssessmentToolCategoryCommandToAssessmentToolCategory(CreateAssessmentToolCategoryCommand createAssessmentToolCategoryCommand)
        {
            return new AssessmentToolCategory
            {
                Name = createAssessmentToolCategoryCommand.Name,
            };
        }

        public AssessmentToolCategoryListData AssessmentToolCategoriesToAssessmentToolCategoryData(List<AssessmentToolCategory> assessmentToolCategories)
        {
            return new AssessmentToolCategoryListData
            {
                AssessmentToolCategories = assessmentToolCategories.Select(x => new AssessmentToolCategoryDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
        }

        public AssessmentToolCategoryForAssessmentToolListData AssessmentToolCategoriesForAssessmentToolToAssessmentToolCategoryForAssessmentToolData(List<AssessmentToolCategory> assessmentToolCategories)
        {
            return new AssessmentToolCategoryForAssessmentToolListData
            {
                AssessmentToolCategories = assessmentToolCategories.Select(x => new AssessmentToolCategoryForAssessmentToolDto
                {
                    Id = x.Id,
                    Name = x.Name
                }).ToList()
            };
        }
    }


}
