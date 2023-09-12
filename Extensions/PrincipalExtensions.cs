using PortfolioServer.Api.Entity;

namespace System.Security.Claims
{
    public static class PrincipalExtensions
    {
         /// <summary>
        /// Gets the NameIdentifier associated with the <see cref="ClaimsPrincipal"/>.
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> from which to retrieve the <c>NameIdentifierId</c> claim.</param>
        /// <returns>Name identifier ID of the identity, or <c>null</c> if it cannot be found.</returns>
        public static string GetNameIdentifier(this ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, ClaimTypes.NameIdentifier);
        }

        public static string GetEmail(this ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, ClaimTypes.Email);
        }

        public static string GetGivenName(this ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, ClaimTypes.GivenName);
        }

        public static string GetLastName(this ClaimsPrincipal principal)
        {
            return GetClaimValue(principal, ClaimTypes.Surname);
        }

        public static UserLogin? GetLoginProvider(this ClaimsPrincipal principal)
        {
            if (principal.Identity is null)
                return null;
            
            var identity = principal.Identity as ClaimsIdentity;
            var provider = identity?.Label ?? string.Empty;
            var key = principal.GetNameIdentifier();

            if (string.IsNullOrEmpty(provider) || string.IsNullOrEmpty(key))
                return null;

            return new(provider, key, provider);
        }

        private static string GetClaimValue(ClaimsPrincipal principal, string claimType)
        {
            if (principal is null)
                throw new ArgumentNullException(nameof(principal));

            var claim = principal.FindFirst(claimType);
            return claim is not null ? claim.Value : string.Empty;
        }
    }
}