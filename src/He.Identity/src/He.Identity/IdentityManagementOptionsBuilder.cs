using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace He.Identity
{
    public class IdentityManagementOptionsBuilder
    {
        private List<Action<IServiceCollection>> configurations = new();

        public void AddConfiguration(Action<IServiceCollection> configuration)
        {
            configurations.Add(configuration);
        }

        public void Apply(IServiceCollection services)
        {
            foreach (var configuration in configurations)
            {
                configuration(services);
            }
        }
    }
}
