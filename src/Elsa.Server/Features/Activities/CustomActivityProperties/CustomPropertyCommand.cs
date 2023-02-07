﻿using Elsa.Metadata;
using MediatR;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyCommand : IRequest<IDictionary<string, IEnumerable<ActivityInputDescriptor>>>
    {
    }
}