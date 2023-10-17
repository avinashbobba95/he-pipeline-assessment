using Elsa.CustomWorkflow.Sdk.Models.Auth;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elsa.CustomWorkflow.Sdk.TokenProvider;

namespace Elsa.CustomWorkflow.Sdk
{

    public interface ITokenProvider
    {
        public Token? GetToken(bool forceRefresh = false, string? key = null);
    }
    public class TokenProvider : ITokenProvider
    {
        private readonly Dictionary<string, TokenConfig> _config;
        private readonly string _defaultKey;
        private Token? currentToken;

        public TokenProvider(string domain, string clientId, string clientSecret, string audience, string defaultKey = TokenProviderKeys.Default)
        {
            TokenConfig config = new TokenConfig(domain, clientId, clientSecret, audience);
            Dictionary<string, TokenConfig> dict = new Dictionary<string, TokenConfig>();
            dict.Add(defaultKey, config);
            _defaultKey = defaultKey;
            _config = dict;
        }

        public TokenProvider(Dictionary<string, TokenConfig> config)
        {
            _config = config;
        }

        public Dictionary<string, TokenConfig> Config => this._config;
        public string DefaultKey => this._defaultKey;


        public Token? GetToken(bool forceRefresh = false, string? key = null)
        {
            key = key ?? DefaultKey;
            if (this.currentToken == null || forceRefresh)
            {
                if (_config.TryGetValue(key, out TokenConfig? tokenConfig) && tokenConfig != null)
                {
                    var client = new RestClient($"https://{tokenConfig.Domain}/oauth/token");
                    var request = new RestRequest("", Method.Post);
                    request.AddHeader("content-type", "application/x-www-form-urlencoded");
                    request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={tokenConfig.ClientId}&client_secret={tokenConfig.ClientSecret}&audience={tokenConfig.Audience}", ParameterType.RequestBody);
                    RestResponse response = client.Execute(request);
                    if (response != null && response.Content != null)
                    {
                        var token = JsonConvert.DeserializeObject<Token>(response.Content);
                        this.currentToken = token;
                    }
                }
                else throw new KeyNotFoundException($"The config in this token provider did not contain any value matching the key:{key}");

            }

            return this.currentToken;
        }

    }

    public class TokenConfig
    {
        public string Domain { get; set; } = null!;
        public string ClientId { get; set; } = null!;
        public string ClientSecret { get; set; } = null!;
        public string Audience { get; set;} = null!;
        public TokenConfig(string domain, string clientId, string clientSecret, string audience)
        {
            this.Domain = domain;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;
            this.Audience = audience;
        }
    }
}
