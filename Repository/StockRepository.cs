using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
using api.Helpers;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : IStockRepository
    {
        private readonly ApplicationDBContext _context;
        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Stock> CreateAsync(Stock stock)
        {
           await _context.Stocks.AddAsync(stock);
           await _context.SaveChangesAsync();
           return stock;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(existingStock == null)
            {
                return null;
            }
            _context.Remove(existingStock);
            await _context.SaveChangesAsync();
            return existingStock;
        }

        public async Task<List<Stock>> GetAllAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(c => c.Comment).ThenInclude(c => c.AppUser).AsQueryable();

            if(! string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(c => c.CompanyName.Contains(query.CompanyName));
            }

            if(! string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if(! string.IsNullOrWhiteSpace(query.SortBy))
            {
                if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
                {
                    stocks = query.IsDescending ? stocks.OrderByDescending(x => x.Symbol) : stocks.OrderBy(x => x.Symbol);
                }
            }

            var skipNumber = (query.PageNumber - 1) * query.PageSize;
            return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
        }

        public async Task<Stock?> GetById(int id)
        {
            return await _context.Stocks.Include(c => c.Comment).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Stock?> GetBySymbol(string symbol)
        {
            return await _context.Stocks.FirstOrDefaultAsync(x => x.Symbol.ToLower() == symbol.ToLower());
        }

        public async Task<bool> IsStockExist(int id)
        {
            bool isExist = await _context.Stocks.AnyAsync(s => s.Id == id);
            return isExist;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
        {
            var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
            if(stockModel == null)
            {
                return null;
            }
            stockModel.Symbol = stockDto.Symbol; 
            stockModel.CompanyName = stockDto.CompanyName;
            stockModel.Purchase = stockDto.Purchase;
            stockModel.LastDiv = stockDto.LastDiv;
            stockModel.Industry = stockDto.Industry;
            stockModel.MarketCap = stockDto.MarketCap;

            await _context.SaveChangesAsync();
            return stockModel;
        }
    }
}