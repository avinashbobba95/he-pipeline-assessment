using Auth0.AuthenticationApi;
using Auth0.ManagementApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.Identity
{
    public interface IAuth0Client
    {
        public string UserConnection { get; }
        Task Authentication(Func<IAuthenticationApiClient, Task> action);
        Task<T> Authentication<T>(Func<IAuthenticationApiClient, Task<T>> action);
        Task Management(Func<ManagementApiClient, Task> action);
        Task<T> Management<T>(Func<ManagementApiClient, Task<T>> action);
    }
}
