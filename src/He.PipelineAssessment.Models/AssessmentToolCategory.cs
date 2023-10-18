using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Models
{
    public class AssessmentToolCategory : AuditableEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
