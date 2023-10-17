using Elsa.Server.Features.Activities.DataDictionary;
using MediatR;

namespace Elsa.Server.Features.Activities.WorkflowDictionary
{
    public class WorkflowDictionaryCommandHandler : IRequestHandler<DataDictionaryCommand, string>
    {
        public Task<string> Handle(DataDictionaryCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
