using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Extentions;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controller
{
    [Route("api/portfolio/")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IStockRepository _stockRepo;
        private readonly IPortfolioRepository _portfolioRepository;
        public PortfolioController(UserManager<AppUser> userManager, IStockRepository stockRepo, IPortfolioRepository portfolioRepository)
        {
            _stockRepo = stockRepo;
            _userManager = userManager;
            _portfolioRepository = portfolioRepository;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetUserPortfolio()
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var portfolioUser = await _portfolioRepository.GetUserPortfolio(appUser);
            return Ok(portfolioUser);

        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> CreatePortfolio(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var stock = await _stockRepo.GetBySymbol(symbol);
            if(stock == null) return BadRequest("stock Not found");
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            if(userPortfolio.Any(x => x.Symbol.ToLower() == symbol.ToLower())) return BadRequest("cannot add same stock to portfolio ");
            var portfolioModel = new Portfolios
            {
                AppUserId = appUser.Id,
                StockId = stock.Id
            };
            var createdPortfolio = await _portfolioRepository.CreateAsync(portfolioModel);
            if (createdPortfolio==null) return StatusCode(500,"could not create");
            return Created();


        }

        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(string symbol)
        {
            var userName = User.GetUsername();
            var appUser = await _userManager.FindByNameAsync(userName);
            var userPortfolio = await _portfolioRepository.GetUserPortfolio(appUser);
            var filteredStock = userPortfolio.Where(s => s.Symbol.ToLower() == symbol.ToLower()).ToList();
            if(filteredStock == null) return BadRequest("User doesnt have this stock");

            if(filteredStock.Count() == 1)
            {
               await _portfolioRepository.Delete(appUser, symbol);

            }
            return Ok();

        }
    }
}