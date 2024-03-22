using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stocks;
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

        public async Task<List<Stock>> GetAllAsync()
        {
            return await _context.Stocks.Include(c => c.Comment).ToListAsync();
        }

        public async Task<Stock?> GetById(int id)
        {
            return await _context.Stocks.Include(c => c.Comment).FirstOrDefaultAsync(x => x.Id == id);
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