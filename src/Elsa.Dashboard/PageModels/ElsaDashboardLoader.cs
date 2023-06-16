using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.Dashboard.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using System.Net;

namespace Elsa.Dashboard.PageModels
{
  [Authorize]
  public class ElsaDashboardLoader : PageModel
  {
    public string _serverUrl { get; set; }
    private IElsaServerHttpClient _client { get; set; }

    private ILogger<ElsaDashboardLoader> _logger { get; set; }

    public string? JsonResponse { get; set; }

    public ElsaDashboardLoader(IElsaServerHttpClient client, IOptions<Urls> options, ILogger<ElsaDashboardLoader> logger)
    {

      _serverUrl = options.Value.ElsaServer ?? string.Empty;
      _client = client;
      _logger = logger;
    }

    public async Task OnGetAsync()
    {
      CookieContainer cookies = new CookieContainer(); //this container saves cookies from responses and send them in requests
      string concatCookie = "";
      foreach (var cookie in HttpContext.Request.Cookies)
      {

        if (cookie.Key.ToLower().Contains("he_identity"))
        {
          concatCookie += string.Format("{0}={1}; ", cookie.Key, cookie.Value);
        }

        cookies.Add(new Cookie(cookie.Key, cookie.Value, "/", "localhost"));
      }

        if (!string.IsNullOrEmpty(_serverUrl))
      {
        JsonResponse = await _client.LoadCustomActivities(_serverUrl, concatCookie);
      }
      else
      {
        HttpContext.Response.StatusCode = (int)HttpStatusCode.FailedDependency;
        throw new NullReferenceException("No Configuration value found for Urls:ElsaServer");
      }
    }
  }
}
