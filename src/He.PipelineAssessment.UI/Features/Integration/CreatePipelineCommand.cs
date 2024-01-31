using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Integration
{
    public class CreatePipelineCommand : IRequest<int>
    {
        public ProjectDTO ProjectData { get; set; }
    }
}
