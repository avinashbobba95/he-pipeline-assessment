using He.Identity;
using He.Identity.Auth0;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace He.Identity.Auth0
{
    public static class IdentityManagementOptionsExtensions
    {
        public static void UseAuth0(
            this IdentityManagementOptionsBuilder builder,
            Auth0Config config,
            Auth0ManagementConfig managementConfig)
        {
            builder.AddConfiguration(services => services.AddSingleton<IAuth0Client>(provider =>
            {
                var http = provider.GetRequiredService<HttpClient>();
                return new Auth0Client(http, managementConfig);
            }));


            builder.AddConfiguration(services => services.AddSingleton<IIdentityManagementService>(provider =>
            {
                var client = provider.GetRequiredService<IAuth0Client>();
                return new Auth0ManagementService(client, config);
            }));
        }
    }
}
