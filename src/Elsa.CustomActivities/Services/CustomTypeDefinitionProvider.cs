﻿using Elsa.CustomActivities.Activites.MultipleChoice;
using Elsa.Scripting.JavaScript.Services;

namespace MyActivityLibrary.JavaScript
{
    public class CustomTypeDefinitionProvider : TypeDefinitionProvider
    {
        public override ValueTask<IEnumerable<Type>> CollectTypesAsync(TypeDefinitionContext context, CancellationToken cancellationToken = default)
        {
            var types = new[] { typeof(MultipleChoiceQuestionModel) };
            return new ValueTask<IEnumerable<Type>>(types);
        }
    }
}