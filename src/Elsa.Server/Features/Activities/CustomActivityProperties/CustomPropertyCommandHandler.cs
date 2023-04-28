using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using MediatR;
using Newtonsoft.Json;

namespace Elsa.Server.Features.Activities.CustomActivityProperties
{
    public class CustomPropertyCommandHandler : IRequestHandler<CustomPropertyCommand, Dictionary<string, string>>
    {
        private readonly ICustomPropertyDescriber _describer;
        private readonly IElsaCustomRepository _elsaContext;
        private readonly ILogger<CustomPropertyCommandHandler> _logger;
        public CustomPropertyCommandHandler(ICustomPropertyDescriber describer, IElsaCustomRepository repository, ILogger<CustomPropertyCommandHandler> logger)
        {
            _describer = describer;
            _elsaContext = repository;
            _logger = logger;
        }

        public async Task<Dictionary<string, string>> Handle(CustomPropertyCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Dictionary<string, string> propertyResponses = new Dictionary<string, string>();
                string propertyJson = JsonConvert.SerializeObject(_describer.DescribeInputProperties(typeof(Question)));
                string dictionaryJson = JsonConvert.SerializeObject(await _elsaContext.GetQuestionDataDictionaryGroupsAsync(cancellationToken));
                propertyResponses.Add("QuestionProperties", propertyJson);
                propertyResponses.Add("DataDictionaryGroups", dictionaryJson);
                return propertyResponses;
            }
            catch(Exception e)
            {
                _logger.LogError(e, "Error thrown whilst obtaining custom properties");
                return new Dictionary<string, string>();
            }
        }
    }
}
