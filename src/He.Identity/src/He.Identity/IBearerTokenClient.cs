using System.Threading.Tasks;

namespace He.Identity
{
    public interface IBearerTokenClient
    {
        Task<BearerToken> GetToken();
    }
}
