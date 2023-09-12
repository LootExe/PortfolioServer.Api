using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public interface IPortfolioRepository
    {
        /// <summary>
        /// Creates the specified <paramref name="portfolio"/> in the database.
        /// </summary>
        /// <param name="portfolio">The portfolio to create.</param>
        Task CreateAsync(Portfolio portfolio);
        /// <summary>
        /// Updates the specified <paramref name="portfolio"/> in the database.
        /// </summary>
        /// <param name="portfolio">The portfolio to update.</param>
        Task UpdateAsync(Portfolio portfolio);
         /// <summary>
        /// Deletes the specified <paramref name="portfolio"/> from database.
        /// </summary>
        /// <param name="portfolio">The portfolio to delete.</param>
        Task DeleteAsync(Portfolio portfolio);
        /// <summary>
        /// Finds and returns a portfolio, if any, who has the specified <paramref name="portfolioId"/>.
        /// </summary>
        /// <param name="portfolioId">The portfolio ID to search for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the portfolio matching the specified <paramref name="portfolioId"/> if it exists.
        /// </returns>
        Task<Portfolio?> FindByIdAsync(Guid portfolioId, CancellationToken cancel = default);
        /// <summary>
        /// Finds and returns a portfolio, if any, who has the specified <paramref name="userId"/> and <paramref name="portfolioId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="portfolioId">The portfolio ID to search for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the portfolio matching the specified <paramref name="portfolioId"/> if it exists.
        /// </returns>
        Task<Portfolio?> FindByUserAsync(Guid userId, Guid portfolioId, CancellationToken cancel = default);
        /// <summary>
        /// Retrieves the associated portfolios for the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user whose associated portfolios to retrieve.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="Portfolio"/> for the specified <paramref name="userId"/>, if any.
        /// </returns>
        Task<IList<Portfolio>> GetAllAsync(Guid userId, CancellationToken cancel = default);
        /// <summary>
        /// Determines whether the database contains an element that
        /// match the defined <paramref name="portfolioId"/> and <paramref name="userId"/>.
        /// </summary>
        /// <param name="portfolioId">The portfolio id that needs to match.</param>
        /// <param name="userId">The user id that needs to match.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a bool if the portfolio exists.
        /// </returns>
        Task<bool> ExistsForUserAsync(Guid portfolioId, Guid userId, CancellationToken cancel = default);
    }
}