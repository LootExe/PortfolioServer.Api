using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PortfolioServer.Api.Entity;

namespace PortfolioServer.Api.Repository
{
    public interface IAssetRepository
    {
        /// <summary>
        /// Creates the specified <paramref name="asset"/> in the database.
        /// </summary>
        /// <param name="asset">The asset to create.</param>
        Task CreateAsync(Asset asset);
        /// <summary>
        /// Updates the specified <paramref name="asset"/> in the database.
        /// </summary>
        /// <param name="asset">The asset to update.</param>
        Task UpdateAsync(Asset asset);
         /// <summary>
        /// Deletes the specified <paramref name="asset"/> from database.
        /// </summary>
        /// <param name="asset">The asset to delete.</param>
        Task DeleteAsync(Asset asset);
        /// <summary>
        /// Finds and returns an asset, if any, who has the specified <paramref name="assetId"/>.
        /// </summary>
        /// <param name="assetId">The asset ID to search for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the asset matching the specified <paramref name="assetId"/> if it exists.
        /// </returns>
        Task<Asset?> FindByIdAsync(Guid assetId, CancellationToken cancel = default);
        /// <summary>
        /// Finds and returns an asset, if any, who has the specified <paramref name="userId"/> and <paramref name="assetId"/>.
        /// </summary>
        /// <param name="userId">The user ID to search for.</param>
        /// <param name="assetId">The asset ID to search for.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the asset matching the specified <paramref name="assetId"/> if it exists.
        /// </returns>
        Task<Asset?> FindByUserAsync(Guid userId, Guid assetId, CancellationToken cancel = default);
        /// <summary>
        /// Retrieves the associated assets for the specified <paramref name="userId"/>.
        /// </summary>
        /// <param name="userId">The user whose associated assets to retrieve.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="asset"/> for the specified <paramref name="userId"/>, if any.
        /// </returns>
        Task<IList<Asset>> GetAllAsync(Guid userId, CancellationToken cancel = default);
        /// <summary>
        /// Retrieves the associated assets for the specified <paramref name="userId"/> and <paramref name="portfolioId"/>.
        /// </summary>
        /// <param name="userId">The user whose associated assets to retrieve.</param>
        /// <param name="portfolioId">The portfolio whose associated assets to retrieve.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a list of <see cref="asset"/> for the specified <paramref name="userId"/> and  and <paramref name="portfolioId"/>, if any.
        /// </returns>
        Task<IList<Asset>> GetAllAsync(Guid userId, Guid portfolioId, CancellationToken cancel = default);
        /// <summary>
        /// Determines whether the database contains an element that
        /// match the defined <paramref name="assetId"/> and <paramref name="userId"/>.
        /// </summary>
        /// <param name="assetId">The asset id that needs to match.</param>
        /// <param name="userId">The user id that needs to match.</param>
        /// <param name="cancel">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> for the asynchronous operation, containing a bool if the asset exists.
        /// </returns>
        Task<bool> ExistsForUserAsync(Guid assetId, Guid userId, CancellationToken cancel = default);
    }
}