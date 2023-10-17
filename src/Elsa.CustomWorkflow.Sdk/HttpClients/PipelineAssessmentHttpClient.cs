using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.Json;
using System.Net.Http.Headers;
using JsonSerializer = System.Text.Json.JsonSerializer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Elsa.CustomWorkflow.Sdk.HttpClients
{
    public interface IPipelineAssessmentHttpClient
    {
        Task<string?> LoadWorkflowDictionary(string elsaServer);
    }

    public class PipelineAssessmentHttpClient : IPipelineAssessmentHttpClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<PipelineAssessmentHttpClient> _logger;
        private readonly IConfiguration _configuration;
        private readonly IHostEnvironment _hostEnvironment;
        private readonly ITokenProvider _tokenProvider;

        public PipelineAssessmentHttpClient(IHttpClientFactory httpClientFactoryFactory, ILogger<PipelineAssessmentHttpClient> logger, IConfiguration configuration, IHostEnvironment hostEnvironment, ITokenProvider provider)
        {
            _httpClientFactory = httpClientFactoryFactory;
            _logger = logger;
            _configuration = configuration;
            _hostEnvironment = hostEnvironment;
            _tokenProvider = provider;
        }

        public async Task<string?> LoadWorkflowDictionary(string pipelineUrl)
        {
            string data;
            string fullUri = $"{pipelineUrl}/admin/workflows" + "?t=" + DateTime.UtcNow.Ticks;
            var client = _httpClientFactory.CreateClient("PipelineClient");
            AddAccessTokenToRequest(client);
            using (var response = await client
                       .GetAsync(fullUri)
                       .ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError($"StatusCode='{response.StatusCode}'," +
                                     $"\n Message= '{data}'," +
                                     $"\n Url='{fullUri}'");

                    return default;
                }
            }
            return data;
        }



        private void AddAccessTokenToRequest(HttpClient client)
        {
            var accessToken = GetAuth0AccessToken();
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", accessToken);
        }

        private string GetAuth0AccessToken()
        {
            try
            {
                var token = _tokenProvider.GetToken(true, TokenProviderKeys.Pipeline);
                if (token != null)
                {
                    return token.AccessToken;
                }
                else return string.Empty;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return string.Empty;
            }
        }


    }
}
