using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Integration
{
    public class CreatePipelineCommand : IRequest<int>
    {
        public CreatePipelineCommand(ProjectDTO projectData) 
        {
            this.ProjectData = projectData;
        }

        public ProjectDTO ProjectData { get; set; }
    }
}
