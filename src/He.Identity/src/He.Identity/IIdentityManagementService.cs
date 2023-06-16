using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.Identity
{
    public interface IIdentityManagementService
    {
        Task<bool> IdentityExists(string email);
        Task CreateIdentity(string email, string password, string role);
        Task ResendVerificationEmail(string email);
    }
}
