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
    public class CommentRepository : ICommentRepository
    {
        private readonly ApplicationDBContext _context;
        public CommentRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Comment> CreateCommentAsync(Comment comment)
        {
            await _context.comments.AddAsync(comment);
            await _context.SaveChangesAsync();
            return comment;
        }

        public async Task<Comment?> DeleteAsync(int id)
        {
            var commentModel = await _context.comments.FirstOrDefaultAsync(c => c.Id == id);
            if (commentModel == null)
            {
                return null;
            }
            _context.comments.Remove(commentModel);
            await _context.SaveChangesAsync();
            return commentModel;
        }

        public async Task<List<Comment>> GetAllAsync()
        {
            return await _context.comments.Include(c => c.AppUser).ToListAsync();
        }

        public async Task<Comment?> GetById(int id)
        {
            return await _context.comments.Include(c => c.AppUser).FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Comment?> UpdateAsync(int id, Comment comment)
        {
            var commentModel = await _context.comments.FindAsync(id);
            if(commentModel == null)
            return null;

            commentModel.Content = comment.Content;
            commentModel.Title = comment.Title;
            await _context.SaveChangesAsync();

            return commentModel;

        }
    }
}