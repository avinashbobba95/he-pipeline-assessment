﻿using Elsa.CustomActivities.Activities.Shared;
using Elsa.CustomModels;
using Elsa.Models;
using Elsa.Scripting.JavaScript.Events;
using Elsa.Scripting.JavaScript.Messages;
using MediatR;

namespace Elsa.CustomActivities.Activities.SingleChoice
{
    public class GetSingleChoiceQuestionScriptHandler :GetScriptHandler
    {
        public override string JavascriptElementName { get; set; } = "SingleChoiceQuestionResponse";
    }
}
