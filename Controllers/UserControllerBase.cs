using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using PortfolioServer.Api.Authorization;
using PortfolioServer.Api.Entity;
using PortfolioServer.Api.Repository;

namespace PortfolioServer.Api.Controller
{
    [ApiController]
    [Authorize]
    [Authorize(Policies.UserExists)]
    public abstract class UserControllerBase : ControllerBase, IAsyncActionFilter
    {
        public UserControllerBase(IUserRepository repository)
        {
            userRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IUserRepository userRepository;

        [AllowNull]
        public User CurrentUser { get; private set; }

        [NonAction]
        public async Task OnActionExecutionAsync(ActionExecutingContext context,
                                                 ActionExecutionDelegate next)
        {
            CurrentUser = await userRepository.GetUserAsync(context.HttpContext.User);

            if (CurrentUser is null)
            {
                context.Result = new BadRequestResult();
                return;
            }
            
            await next();
            CurrentUser = null;
        }
    }
}