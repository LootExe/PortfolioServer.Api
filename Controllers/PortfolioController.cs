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
    [Route("api/portfolios")]
    public class PortfolioController : UserControllerBase
    {
        public PortfolioController(
            IUserRepository userRepository,
            IPortfolioRepository portfolioRepository) : base(userRepository)
        {
            repository = portfolioRepository;
        }

        private readonly IPortfolioRepository repository;

        [HttpPost]
        public async Task<ActionResult<PortfolioDto>> CreateAsync(PortfolioCreateDto data)
        {
            var portfolio = new Portfolio()
            {
                Name = data.Name!,
                UserId = CurrentUser.Id
            };
            await repository.CreateAsync(portfolio);

            return CreatedAtAction(actionName: nameof(GetAsync),
                                   routeValues: new { id = portfolio.Id },
                                   value: portfolio.ToDto());
        }

        [HttpGet]
        public async Task<ActionResult<IList<PortfolioDto>>> GetAllAsync()
        {
            var portfolios = await repository.GetAllAsync(CurrentUser.Id);
            return Ok(portfolios.ToDtoList());
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<PortfolioDto>> GetAsync(Guid id)
        {
            var portfolio = await repository.FindByUserAsync(CurrentUser.Id, id);
            return portfolio is null ? NotFound() : Ok(portfolio.ToDto()); 
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> UpdateAsync(Guid id, PortfolioUpdateDto data)
        {
            var portfolio = await repository.FindByUserAsync(CurrentUser.Id, id);

            if (portfolio is null)
                return NotFound();

            portfolio.Name = data.Name!;

            await repository.UpdateAsync(portfolio);
            return NoContent();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteAsync(Guid id)
        {
            var portfolio = await repository.FindByUserAsync(CurrentUser.Id, id);

            if (portfolio is null)
                return NotFound();

            await repository.DeleteAsync(portfolio);
            return NoContent();
        }

        // GET api/history => GetSymbolHistory(symbol, starteDate, endDate) = Download history for a specific symbol
        // GET api/symbol => GetSymbolDetails(symbol) = Use as symbol lookup

        // GET api/assets/{id}/history => GetAssetHistory(startDate, endDate) = Download asset history (daily EOD prices with transactions)
        // Example: Buy 10 BTC at $100 on 02/05/2018 => { "assetId": "123", "timestamp": "02.05.2018 12:00:00", "value": "1000" }
    
        // How to include a Benchmark like S&P500
        // How to include raw cash savings

        // Symbol lookup:
        // -> Dropdown (Crypto, ETF, ...)
        // -> search for key pair (BTCUSDT) or WKN / ISN for ETF / Stock
        // -> Returns Asset Data Provider (Data Endpoint = Binance, Symbol = BTCUSDT)
        //
        // AssetCreateDto 
        // -> DataEndpoint = Binance
        // -> SymbolKey = BTCUSDT
        // -> PortfolioId = Guid
        //
        // Verify Asset Create Data
        // Add Symbol object to database
        // Symbol
        // -> Id
        // -> ProviderModule
        // -> ProviderKey
        // Add Asset object to database 
        
        // Asset Table
        // Id, SymbolId, Currency
        // One to many Transactions

        // Symbol Table
        // Id, Module, Key
        // One to many SymbolHistory

        // SymbolHistory Table
        // Id, SymbolId, Timestamp, Price

        // Create Symbol search method => Use https://www.nuget.org/packages/CoinGeckoAsyncApi/
        
        // Endpoints:
        // -> Search for symbol (btcusdt, ethbtc)
        // -> Get daily close price history for given symbol
    }
} 