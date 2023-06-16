using Auth0.ManagementApi.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace He.Identity.Auth0
{
    public class Auth0ManagementService : IIdentityManagementService
    {
        private readonly IAuth0Client auth0;
        private readonly Auth0Config config;

        public Auth0ManagementService(IAuth0Client auth0Client, Auth0Config config)
        {
            this.auth0 = auth0Client;
            this.config = config;
        }

        public async Task<bool> IdentityExists(string email)
        {
            var matchingUsers = await auth0.Management(x => x.Users.GetUsersByEmailAsync(email));
            return matchingUsers.Any();
        }

        public async Task CreateIdentity(string email, string password, string role)
        {
            // we must check if the user exists as the auth0 API will error if it already does
            var matchingUsers = await auth0.Management(x => x.Users.GetUsersByEmailAsync(email));
            var user = matchingUsers.SingleOrDefault();

            if (user == null)
            {
                user = await auth0.Management(x => x.Users.CreateAsync(new UserCreateRequest
                {
                    Connection = auth0.UserConnection,
                    Email = email,
                    Password = password,
                    EmailVerified = true
                }));
            }

            // however, we do not need to check if the user already has the relevant role
            var allRoles = await auth0.Management(x => x.Roles.GetAllAsync(new GetRolesRequest()));
            var roleId = allRoles.SingleOrDefault(x => x.Name == role)?.Id;

            if (roleId == null)
            {
                throw new Exception($"No role with the name '{role}' exists in Auth0.");
            }

            await auth0.Management(x => x.Users.AssignRolesAsync(user.UserId, new AssignRolesRequest
            {
                Roles = new string[] { roleId }
            }));
        }

        public async Task ResendVerificationEmail(string email)
        {
            var matchingUsers = await auth0.Management(x => x.Users.GetUsersByEmailAsync(email));
            var user = matchingUsers.SingleOrDefault();

            await auth0.Management(x => x.Jobs.SendVerificationEmailAsync(new VerifyEmailJobRequest
            {
                ClientId = this.config.ClientId,
                UserId = user.UserId
            }));
        }
    }
}
