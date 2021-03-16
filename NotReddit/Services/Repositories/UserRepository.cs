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
    public class UserRepository : RepositoryBase<User>, IUserRepository
    {
        public UserRepository(NotRedditContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool UserExists(string id)
        {
            var found = RepositoryContext.Users.Any(e => e.Id == id);
            return found;
        }

        public User FindByCondition(Expression<Func<User, bool>> expression)
        {
            return this.RepositoryContext.Users
                .Include(t => t.UserBadge)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<User> FindAll()
        {
            return this.RepositoryContext.Users
                .Include(t => t.UserBadge)
                .ToList();
        }
    }
}
