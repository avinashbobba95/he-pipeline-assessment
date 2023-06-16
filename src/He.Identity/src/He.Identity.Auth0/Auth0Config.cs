namespace He.Identity.Auth0
{
    public record Auth0Config(
        string Domain,
        string ClientId,
        string ClientSecret);
}
