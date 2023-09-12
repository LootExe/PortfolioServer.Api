using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
 
namespace PortfolioServer.Api.Helper
{
    public class JwksRetriever : IConfigurationRetriever<OpenIdConnectConfiguration>
    {
        public Task<OpenIdConnectConfiguration> GetConfigurationAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            return GetAsync(address, retriever, cancel);
        }
 
        /// <summary>
        /// Retrieves a populated <see cref="OpenIdConnectConfiguration"/> given an address and an <see cref="IDocumentRetriever"/>.
        /// </summary>
        /// <param name="address">address of the jwks uri.</param>
        /// <param name="retriever">the <see cref="IDocumentRetriever"/> to use to read the jwks</param>
        /// <param name="cancel"><see cref="CancellationToken"/>.</param>
        /// <returns>A populated <see cref="OpenIdConnectConfiguration"/> instance.</returns>
        public static async Task<OpenIdConnectConfiguration> GetAsync(string address, IDocumentRetriever retriever, CancellationToken cancel)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentNullException(nameof(address));

            if (retriever is null)
                throw new ArgumentNullException(nameof(retriever));
            
            var doc = await retriever.GetDocumentAsync(address, cancel);
            var jwks = new JsonWebKeySet(doc);
            var configuration = new OpenIdConnectConfiguration()
            {
                JsonWebKeySet = jwks,
                JwksUri = address,
            };

            foreach (var key in jwks.GetSigningKeys())
            {
                configuration.SigningKeys.Add(key);
            }
            
            return configuration;
        }
    }
}