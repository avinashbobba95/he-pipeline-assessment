﻿using Elsa.ActivityResults;
using Elsa.Attributes;
using Elsa.Expressions;
using Elsa.Services;
using Elsa.Services.Models;
using He.PipelineAssessment.Data.VFM;

namespace Elsa.CustomActivities.Activities.VFMDataSource
{
    [Action(
        Category = "Homes England Data",
        Description = "Get VFM LA Calculations Data Source",
        Outcomes = new[] { OutcomeNames.Done }
    )]
    public class VFMDataSource : Activity
    {
        private readonly IEsriVFMClient _client;
        private readonly IEsriVFMDataJsonHelper _jsonHelper;
        public VFMDataSource(IEsriVFMClient client, IEsriVFMDataJsonHelper jsonHelper)
        {
            _client = client;
            _jsonHelper = jsonHelper;
        }

        [ActivityInput(Hint = "Gss Code of the record to get", Name = "Gss Code", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string GssCode { get; set; } = null!;
        [ActivityInput(Hint = "Name of Local Authority", Name = "Local Authority Name", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string LocalAuthority { get; set; } = null!;
        [ActivityInput(Hint = "Alternative Name of Local Authority", Name = "Local Authority Alternative Name", SupportedSyntaxes = new[] { SyntaxNames.Literal, SyntaxNames.Json, SyntaxNames.JavaScript })]
        public string LocalAuthorityAlt { get; set; } = null!;

        [ActivityOutput] public VFMCalculationData? Output { get; set; }

        protected override async ValueTask<IActivityExecutionResult> OnExecuteAsync(ActivityExecutionContext context)
        {
            context.JournalData.Add(nameof(GssCode), GssCode);
            context.JournalData.Add(nameof(LocalAuthority), LocalAuthority);
            context.JournalData.Add(nameof(LocalAuthorityAlt), LocalAuthorityAlt);

            var data = await _client.GetVFMCalculationData(GssCode, LocalAuthority, LocalAuthorityAlt);

            if (data != null)
            {
                var dataResult = _jsonHelper.JsonToVFMCalculationData(data);
                this.Output = dataResult;

            }
            else
            {
                context.JournalData.Add("Error", "Call to GetVFMCalculationData returned null");
                return new SuspendResult();
            }

            return Done();
        }
    }
}
