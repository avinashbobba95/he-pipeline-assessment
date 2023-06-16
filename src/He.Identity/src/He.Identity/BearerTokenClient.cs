using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.Identity
{
    public class BearerTokenClient : IBearerTokenClient
    {
        private BearerToken token;
        private readonly Func<Task<BearerToken>> getToken;

        public BearerTokenClient(Func<Task<BearerToken>> getToken)
        {
            this.token = new BearerToken();
            this.getToken = getToken;
        }

        public async Task<BearerToken> GetToken()
        {
            if (token.HasExpired || token.WillExpireInLessThan(TimeSpan.FromMinutes(2)))
            {
                token = await getToken();
            }

            return token;
        }
    }
}
