using System;

namespace He.Identity
{
    public class BearerToken
    {
        public BearerToken()
            : this("", "", "", 0)
        {
        }

        public BearerToken(string idToken, string accessToken, string refreshToken, int expiresIn)
        {
            IdToken = idToken;
            AccessToken = accessToken;
            RefreshToken = refreshToken;
            ExpiresIn = expiresIn;
            ExpiresOn = DateTime.UtcNow.AddSeconds(expiresIn);
        }

        public string IdToken { get; }
        public string AccessToken { get; }
        public string RefreshToken { get; }
        public int ExpiresIn { get; }
        public DateTime ExpiresOn { get; }
        public bool HasExpired => ExpiresOn >= DateTime.UtcNow;
        public bool WillExpireInLessThan(TimeSpan timeSpan)
        {
            return DateTime.UtcNow + timeSpan >= ExpiresOn;
        }
    }
}
