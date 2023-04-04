using He.Identity;
using IdentityModel.Client;
using JorgeSerrano.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.IO;
using System.Net.Http;
using System.Text.Json;

namespace Elsa.Dashboard
{
  [Route("elsaserver")]
  public class ElsaServerController :Controller
    {

    private readonly ILogger<ElsaServerController> _logger;
    private readonly HttpClient _httpClient;

    public ElsaServerController(HttpClient httpClient, ILogger<ElsaServerController> logger)
    {
      _logger = logger;
      _httpClient = httpClient;
    }

    [Route("{**catchall}")]
    [Authorize]
    public async Task<IActionResult> CatchAll()
    {
      var fgaCredentials = new FgaCredentials
      {
        ClientId = "D4ZLiiwsBq9tMZXbCx2TwBUiNDcsXuIy",
        ClientSecret = "HsE3sWT3kvmsTiH1nUp1CImz9IRb2YxUN6wNopp1RWyKfrvMzeBkOWe9NkT_CG1L",
        Audience = "https://elsa-server-api",
        GrantType = "client_credentials"
      };
      var options = new JsonSerializerOptions { PropertyNamingPolicy = new JsonSnakeCaseNamingPolicy() };



      var response = await _httpClient.PostAsJsonAsync("https://identity-staging-homesengland.eu.auth0.com/oauth/token",
      fgaCredentials,
      options);
      var responseBody = await response.Content.ReadAsStringAsync();
      var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseBody, options);

      var bearerToken = tokenResponse?.AccessToken;
      var baseUrl = "https://localhost:7227";
      var restRequest = new RestRequest("workflow/StartWorkflow", Method.Post);
      restRequest.AddHeader("authorization", $"Bearer {bearerToken}");
      var restClient = new RestClient(baseUrl);
      var result = await restClient.GetAsync(restRequest);
      Console.WriteLine("I am in the controller");
      return Ok();
    }

    public class TokenResponse
    {
      public string AccessToken { get; set; }
    }


  }
}
