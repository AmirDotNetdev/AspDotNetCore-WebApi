using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Comments;
using api.Models;

namespace api.Interfaces
{
    public interface ICommentRepository
    {
        Task<List<Comment>> GetAllAsync();
        Task<Comment?> GetById(int id);
        Task<Comment> CreateCommentAsync(Comment comment);
        Task<Comment?> DeleteAsync(int id);
        Task<Comment?> UpdateAsync(int id, Comment comment);
    }
}