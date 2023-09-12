namespace PortfolioServer.Api.Entity
{
    /// <summary>
    /// Creates a new instance of <see cref="UserLogin"/>
    /// </summary>
    /// <param name="LoginProvider">The provider associated with this login information.</param>
    /// <param name="ProviderKey">The unique identifier for this user provided by the login provider.</param>
    /// <param name="ProviderDisplayName">The display name for this user provided by the login provider.</param>
    /// <remarks>
    /// Examples of the provider may be Local, Facebook, Google, etc.
    /// Key would be unique per provider, examples may be @microsoft as a Twitter provider key.
    /// Examples of the display name may be local, FACEBOOK, Google, etc.
    /// </remarks>
    public record UserLogin(string LoginProvider, string ProviderKey, string ProviderDisplayName) { }
}