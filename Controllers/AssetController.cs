using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PortfolioServer.Api.Dto;
using PortfolioServer.Api.Entity;
using PortfolioServer.Api.Repository;

namespace PortfolioServer.Api.Controller
{
    [ApiController]
    [Route("api/assets")]
    public class AssetController : UserControllerBase
    {
        public AssetController(
            IUserRepository userRepository,
            IAssetRepository assetRepository,
            IPortfolioRepository portfolioRepository) : base(userRepository)
        {
            repository = assetRepository;
            this.portfolioRepository = portfolioRepository;
        }

        private readonly IAssetRepository repository;
        private readonly IPortfolioRepository portfolioRepository;

        public const string QueryParameterError = "query parameter is missing. parameter: ";
        public const string ParentNotFoundError = "parent portfolio not found. id: ";

        [HttpPost]
        public async Task<ActionResult<AssetDto>> CreateAsync(AssetCreateDto data)
        {
            var exists = await portfolioRepository.ExistsForUserAsync(data.PortfolioId!.Value,
                                                                      CurrentUser.Id);

            if (exists is false)
                return NotFound($"{ParentNotFoundError}{data.PortfolioId!.Value}");

            var asset = new Asset()
            {
                Symbol = data.Symbol!,
                Name = data.Name!,
                Currency = data.Currency!,
                PortfolioId = data.PortfolioId!.Value,
                UserId = CurrentUser.Id
            };
            await repository.CreateAsync(asset);

            return CreatedAtAction(actionName: nameof(GetAsync),
                                   routeValues: new { id = asset.Id },
                                   value: asset.ToDto());
        }

        [HttpGet]
        public async Task<ActionResult<IList<AssetDto>>> GetAllAsync([FromQuery] Guid portfolioId)
        {
            if (portfolioId.Equals(Guid.Empty))
                return BadRequest($"{QueryParameterError}{nameof(portfolioId)}");
            
            var exists = await portfolioRepository.ExistsForUserAsync(portfolioId,
                                                                      CurrentUser.Id);

            if (exists is false)
                return NotFound($"{ParentNotFoundError}{portfolioId}");

            var assets = await repository.GetAllAsync(CurrentUser.Id, portfolioId);
            return Ok(assets.ToDtoList());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<AssetDto>> GetAsync(Guid id)
        {
            var asset = await repository.FindByUserAsync(CurrentUser.Id, id);
            return asset is null ? NotFound() : Ok(asset.ToDto()); 
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, AssetUpdateDto data)
        {
            var asset = await repository.FindByUserAsync(CurrentUser.Id, id);

            if (asset is null)
                return NotFound();

            asset.Symbol = data.Symbol!;
            asset.Name = data.Name!;
            asset.Currency = data.Currency!;

            await repository.UpdateAsync(asset);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var asset = await repository.FindByUserAsync(CurrentUser.Id, id);

            if (asset is null)
                return NotFound();

            await repository.DeleteAsync(asset);
            return NoContent();
        }
    }
}