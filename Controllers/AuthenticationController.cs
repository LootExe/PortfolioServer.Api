using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PortfolioServer.Api.Dto;
using PortfolioServer.Api.Entity;
using PortfolioServer.Api.Repository;

namespace PortfolioServer.Api.Controller
{
    [ApiController]
    [Authorize]
    [Route("api/auth")]
    public class AuthenticationController : ControllerBase
    {
        public AuthenticationController(IUserRepository repository)
        {
            userRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        private readonly IUserRepository userRepository;

        public const string LoginProviderError = "Login Provider error";
        public const string DuplicateUserError = "User already exists";

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateAsync()
        {  
            var login = User.GetLoginProvider();

            if (login is null)
                return BadRequest(LoginProviderError);

            var existingUser = await userRepository.FindByLoginAsync(login.LoginProvider,
                                                                     login.ProviderKey);

            if (existingUser is not null)
                return BadRequest(DuplicateUserError);
            
            var user = new User()
            {
                GivenName = User.GetGivenName(),
                Surname = User.GetLastName(),
                Email = User.GetEmail()
            };
            
            await userRepository.AddLoginAsync(user, login);
            await userRepository.CreateAsync(user);

            return CreatedAtAction(actionName: nameof(GetAsync),
                                   value: user.ToDto());
        }

        [HttpGet]
        public async Task<ActionResult<UserDto>> GetAsync()
        {
            var user = await userRepository.GetUserAsync(User);
            return user is null ? NotFound() : Ok(user.ToDto());
        }

        [HttpPut]
        public async Task<ActionResult> UpdateAsync() 
        {
            var user = await userRepository.GetUserAsync(User);

            if (user is null)
                return NotFound();
            
            user.GivenName = User.GetGivenName();
            user.Surname = User.GetLastName();
            user.Email = User.GetEmail();

            await userRepository.UpdateAsync(user);
            return NoContent();
        }
    }
}