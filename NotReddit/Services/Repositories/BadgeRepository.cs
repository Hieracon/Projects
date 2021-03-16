using NotReddit.Data;
using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Repositories
{
    public class BadgeRepository : RepositoryBase<Badge>, IBadgeRepository
    {
        public BadgeRepository(NotRedditContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool BadgeExists(int id)
        {
            var found = RepositoryContext.Badges.Any(e => e.BadgeId == id);
            return found;
        }
    }
}
