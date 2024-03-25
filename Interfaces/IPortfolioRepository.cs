using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Interfaces
{
    public interface IPortfolioRepository
    {
        public Task<List<Stock>> GetUserPortfolio(AppUser user);
        public Task<Portfolios> CreateAsync(Portfolios portfolios);
        public Task<Portfolios?> Delete(AppUser appUser, string symbol);
    }
}