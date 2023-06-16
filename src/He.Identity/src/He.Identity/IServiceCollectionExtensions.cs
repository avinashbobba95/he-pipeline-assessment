using He.Identity;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IServiceCollectionExtensions
    {
        public static IServiceCollection ConfigureIdentityManagementService(
            this IServiceCollection services,
            Action<IdentityManagementOptionsBuilder> build)
        {
            var builder = new IdentityManagementOptionsBuilder();
            build(builder);
            builder.Apply(services);
            return services;
        }
    }
}
