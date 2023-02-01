﻿namespace He.PipelineAssessment.Models
{
    public class AssessmentTool
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastModified { get; set; }
        public int Order { get; set; }

        public virtual List<AssessmentToolWorkflow>? AssessmentToolWorkflows { get; set; }
    }
}