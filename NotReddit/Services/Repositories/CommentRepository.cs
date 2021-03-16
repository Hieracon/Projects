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
    public class CommentRepository : RepositoryBase<Comment>, ICommentRepository
    {
        public CommentRepository(NotRedditContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool CommentExists(int id)
        {
            var found = RepositoryContext.Comments.Any(e => e.CommentId == id);
            return found;
        }

        public Comment FindByCondition(Expression<Func<Comment, bool>> expression)
        {
            return this.RepositoryContext.Comments
                .Include(t => t.User)
                .Include(t => t.Post)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<Comment> FindAll()
        {
            return this.RepositoryContext.Comments
                .Include(t => t.User)
                .Include(t => t.Post)
                .ToList();
        }
    }
}
