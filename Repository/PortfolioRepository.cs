using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class PortfolioRepository : IPortfolioRepository
    {
        private readonly ApplicationDBContext _context;
        public PortfolioRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Portfolios> CreateAsync(Portfolios portfolios)
        {
            await _context.Portfolios.AddAsync(portfolios);
            await _context.SaveChangesAsync();
            return portfolios;

        }

        public async Task<Portfolios?> Delete(AppUser appUser, string symbol)
        {
            var portfolioModel = await _context.Portfolios.FirstOrDefaultAsync(s => s.AppUserId == appUser.Id && s.Stock.Symbol.ToLower() == symbol.ToLower());
            if(portfolioModel == null) return null;
            _context.Remove(portfolioModel);
            await _context.SaveChangesAsync();
            return portfolioModel;

        }

        public async Task<List<Stock>> GetUserPortfolio(AppUser user)
        {
            return await _context.Portfolios.Where(u => u.AppUserId == user.Id).Select(stock => 
                new Stock
                {
                    Id = stock.StockId,
                    Symbol = stock.Stock.Symbol,
                    CompanyName = stock.Stock.CompanyName,
                    Purchase = stock.Stock.Purchase,
                    LastDiv = stock.Stock.LastDiv,
                    Industry = stock.Stock.Industry,
                    MarketCap = stock.Stock.MarketCap,
                }
            ).ToListAsync();
        }
    }
}