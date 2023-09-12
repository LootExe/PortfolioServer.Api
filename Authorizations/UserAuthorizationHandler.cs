using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using PortfolioServer.Api.Repository;

namespace PortfolioServer.Api.Authorization
{
    public class UserAuthorizationHandler : AuthorizationHandler<UserExistsRequirement>
    {
        public UserAuthorizationHandler(IUserRepository repository)
        {
            userRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IUserRepository userRepository;
        
        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            UserExistsRequirement requirement)
        {
            var user = await userRepository.GetUserAsync(context.User);

            if (user is null)
                context.Fail();
            else
                context.Succeed(requirement);
        }
    }
}