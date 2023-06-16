namespace He.Identity.Auth0
{
    public record Auth0ManagementConfig(
        string Domain,
        string ClientId,
        string ClientSecret,
        string Audience,
        string UserConnection);
}
