using System;

namespace PortfolioServer.Api.Entity
{
    /// <summary>
    /// Represents a login and its associated provider for a user.
    /// </summary>
    public record LoginProvider(string Provider, string Key, string DisplayName, Guid UserId) { }
}