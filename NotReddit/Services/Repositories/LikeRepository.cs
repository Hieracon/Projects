using Microsoft.EntityFrameworkCore;
using NotReddit.Data;
using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace NotReddit.Services.Repositories
{
    public class LikeRepository : RepositoryBase<Like>, ILikeRepository
    {
        public LikeRepository(NotRedditContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool LikeExists(int id)
        {
            var found = RepositoryContext.Likes.Any(e => e.LikeId == id);
            return found;
        }

        public Like FindByCondition(Expression<Func<Like, bool>> expression)
        {
            return this.RepositoryContext.Likes
                .Include(c => c.Post)
                .Include(c => c.Comment)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<Like> FindAll()
        {
            return this.RepositoryContext.Likes
                .Include(c => c.Post)
                .Include(c => c.Comment)
                .ToList();

        }
    }
}
