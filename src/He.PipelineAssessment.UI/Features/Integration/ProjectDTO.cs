using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Integration
{
    public class ProjectDTO
    {
        public int ProjectId { get; set; }
        public string SiteName { get; set; } = null!;
        public string ProjectManager { get; set; } = null!;
        public string ProjectManagerEmail { get; set; } = null!;
        public string Counterparty { get; set; } = null!;
        public string Reference { get; set; } = null!;
        public string LocalAuthority { get; set; } = null!;
        public decimal? FundingAsk { get; set; }
        public int NumberOfHomes { get; set; }
        public string BusinessArea { get; set; } = string.Empty;
        public string LandType { get; set; } = string.Empty;
        public string SensitiveStatus { get; set; }
    }
}
