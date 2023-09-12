using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task UpdateAsync(User user);  
        Task DeleteAsync(User user);  
        Task<User?> GetUserAsync(ClaimsPrincipal principal, CancellationToken cancel = default);
        Task<User?> FindByIdAsync(Guid userId, CancellationToken cancel = default);
        Task<User?> FindByEmailAsync(string email, CancellationToken cancel = default);
        Task<User?> FindByLoginAsync(string provider, string key, CancellationToken cancel = default);
        Task<LoginProvider?> FindUserLoginAsync(string provider, string key, CancellationToken cancel = default);
        Task<LoginProvider?> FindUserLoginAsync(Guid userId, string provider, string key, CancellationToken cancel = default);
        Task AddLoginAsync(User user, UserLogin login);
        Task RemoveLoginAsync(User user, string provider, string key);
        Task<IList<UserLogin>> GetLoginsAsync(User user, CancellationToken cancel = default);
    }
}