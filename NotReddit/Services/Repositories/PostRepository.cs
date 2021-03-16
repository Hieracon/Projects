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
    public class PostRepository : RepositoryBase<Post>, IPostRepository
    {
        public PostRepository(NotRedditContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool PostExists(int id)
        {
            var found = RepositoryContext.Posts.Any(e => e.PostId == id);
            return found;
        }

        public Post FindByCondition(Expression<Func<Post, bool>> expression)
        {
            return this.RepositoryContext.Posts
                .Include(t => t.User)
                .Include(t => t.Subsection)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<Post> FindAll()
        {
            return this.RepositoryContext.Posts
                .Include(t => t.User)
                .Include(t => t.Subsection)
                .ToList();
        }
    }
}
