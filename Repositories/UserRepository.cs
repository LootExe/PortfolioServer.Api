using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public class UserRepository : IUserRepository
    {
        public UserRepository(UserDbContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            users = context.Set<User>();
            userLogins = context.Set<LoginProvider>();
        }

        private readonly UserDbContext context;
        private readonly DbSet<User> users;
        private readonly DbSet<LoginProvider> userLogins;

        /// <summary>
        /// Creates the specified <paramref name="user"/> in the database.
        /// </summary>
        /// <param name="user">The user to create.</param>
        public async Task CreateAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            users.Add(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Updates the specified <paramref name="user"/> in the database.
        /// </summary>
        /// <param name="user">The user to update.</param>
        public async Task UpdateAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            users.Attach(user);
            users.Update(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the specified <paramref name="user"/> from database.
        /// </summary>
        /// <param name="user">The user to delete.</param>
        public async Task DeleteAsync(User user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
                
            users.Remove(user);
            await context.SaveChangesAsync();
        }

        /// <summary>
        /// Finds and returns a user, if any, who has the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the user matching the specified <paramref name="userId"/> if it exists.
        /// </returns>
        public async Task<User?> FindByIdAsync(Guid userId, CancellationToken cancel = default)
        {   
            cancel.ThrowIfCancellationRequested();
            return await users.FindAsync(new object[] { userId }, cancel);
        }

         /// <summary>
        /// Return a user login with a provider and key if it exists.
        /// </summary>
        /// <param name="provider">The login provider name.</param>
        /// <param name="key">The key provided by the <paramref name="provider"/> to identify a user.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The user login if it exists.</returns>
        public async Task<LoginProvider?> FindUserLoginAsync(string provider, string key, CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();
            return await userLogins.SingleOrDefaultAsync(login => login.Provider == provider
                                                                  && login.Key == key, 
                                                                  cancel);
        }

        /// <summary>
        /// Return a user login with the matching userId, provider, key if it exists.
        /// </summary>
        /// <param name="userId">The user's id.</param>
        /// <param name="provider">The login provider name.</param>
        /// <param name="key">The key provided by the <paramref name="provider"/> to identify a user.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The user login if it exists.</returns>
        public async Task<LoginProvider?> FindUserLoginAsync(Guid userId, string provider, string key, CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();
            return await userLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) 
                                                                      && userLogin.Provider == provider 
                                                                      && userLogin.Key == key,
                                                                      cancel);
        }

        /// <summary>
        /// Retrieves the user associated with the specified login provider and login provider key.
        /// </summary>
        /// <param name="loginProvider">The login provider who provided the <paramref name="providerKey"/>.</param>
        /// <param name="providerKey">The key provided by the <paramref name="loginProvider"/> to identify a user.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing the user, if any which matched the specified login provider and key.
        /// </returns>
        public async Task<User?> FindByLoginAsync(string loginProvider, string providerKey, CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();
            var login = await FindUserLoginAsync(loginProvider, providerKey, cancel);

            if (login is null)
                return null;

            return await FindByIdAsync(login.UserId, cancel);
        }

        /// <summary>
        /// Gets the user, if any, associated with the specified, normalized email address.
        /// </summary>
        /// <param name="normalizedEmail">The normalized email address to return the user for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The task object containing the results of the asynchronous lookup operation, the user if any associated with the specified normalized email address.
        /// </returns>
        public async Task<User?> FindByEmailAsync(string email, CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();
            return await users.SingleOrDefaultAsync(user => user.Email == email, 
                                                    cancel);
        }

        /// <summary>
        /// Adds the <paramref name="login"/> given to the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to add the login to.</param>
        /// <param name="login">The login to add to the user.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public Task AddLoginAsync(User user, UserLogin login)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));
            
            if (login is null)
                throw new ArgumentNullException(nameof(login));

            userLogins.Add(login.ToLoginProvider(user));
            return Task.CompletedTask;
        }

        /// <summary>
        /// Removes the <paramref name="provider"/> given from the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user to remove the login from.</param>
        /// <param name="provider">The login to remove from the user.</param>
        /// <param name="key">The key provided by the <paramref name="provider"/> to identify a user.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async Task RemoveLoginAsync(User user, string provider, string key)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            var entry = await FindUserLoginAsync(user.Id, provider, key);

            if (entry is not null)
                userLogins.Remove(entry);
        }

        /// <summary>
        /// Retrieves the associated logins for the specified <param ref="user"/>.
        /// </summary>
        /// <param name="user">The user whose associated logins to retrieve.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="UserLogin"/> for the specified <paramref name="user"/>, if any.
        /// </returns>
        public async Task<IList<UserLogin>> GetLoginsAsync(User user, CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();
            
            if (user is null)
                throw new ArgumentNullException(nameof(user));

            return await userLogins.Where(login => login.UserId.Equals(user.Id))
                                   .Select(login => new UserLogin(login.Provider, login.Key, login.DisplayName))
                                   .ToListAsync(cancel);
        }

        public async Task<User?> GetUserAsync(ClaimsPrincipal principal, CancellationToken cancel = default)
        {
            cancel.ThrowIfCancellationRequested();

            if (principal is null)
                throw new ArgumentNullException(nameof(principal));

            var login = principal.GetLoginProvider();

            if (login is null)
                return null;

            return await FindByLoginAsync(login.LoginProvider, login.ProviderKey, cancel);
        }
    }
}