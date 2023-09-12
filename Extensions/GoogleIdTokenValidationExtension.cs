using System;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using PortfolioServer.Api.Configuration;
using PortfolioServer.Api.Helper;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class GoogleIdTokenValidationExtension
    {
        public static AuthenticationBuilder AddGoogleIdTokenValidation(
            this AuthenticationBuilder builder,
            IConfiguration configuration)
        {
            // TODO: Add Configuration validator => https://github.com/dotnet/runtime/issues/36391#issuecomment-536547483
            var config = configuration.GetSection(GoogleIdTokenConfiguration.Section)
                                      .Get<GoogleIdTokenConfiguration>();
            
            ValidateConfiguration(config);

            builder.AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters    
                {    
                    ValidIssuers = config.Issuers!,  
                    ValidAudience = config.ClientId!,   
                    ValidateIssuer = true,
                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,    
                    ValidateLifetime = true,
                    NameClaimType = "name",
                };  

                options.Events = new JwtBearerEvents()
                {
                    OnTokenValidated = context =>
                    {
                        if (context.Principal is not null)
                        {
                            if (context.Principal.Identity is ClaimsIdentity claimsIdentity)
                                claimsIdentity.Label = "Google";
                        }
                        return Task.CompletedTask;
                    }
                };

                var httpClient = new HttpClient(options.BackchannelHttpHandler ?? new HttpClientHandler())
                {
                    Timeout = options.BackchannelTimeout,
                    MaxResponseContentBufferSize = 1024 * 1024 * 10 // 10 MB 
                };

                options.ConfigurationManager = new ConfigurationManager<OpenIdConnectConfiguration>(
                    new Uri(config.JwksUrl!).OriginalString,
                    new JwksRetriever(),
                    new HttpDocumentRetriever(httpClient) 
                    { 
                        RequireHttps = options.RequireHttpsMetadata 
                    });
            });
            
            return builder;
        }

        private static void ValidateConfiguration(GoogleIdTokenConfiguration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException(nameof(configuration));

            if (string.IsNullOrWhiteSpace(configuration.ClientId))
                throw new MissingMemberException(nameof(configuration.ClientId));

            if (string.IsNullOrWhiteSpace(configuration.JwksUrl))
                throw new MissingMemberException(nameof(configuration.JwksUrl));

            if (configuration.Issuers is null || configuration.Issuers.Length is 0)
                throw new MissingMemberException(nameof(configuration.Issuers));
        }
    }
}