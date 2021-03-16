using NotReddit.Data;
using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Repositories
{
    public class SubsectionRepository : RepositoryBase<Subsection>, ISubsectionRepository
    {
        public SubsectionRepository(NotRedditContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool SubsectionExists(int id)
        {
            var found = RepositoryContext.Subsections.Any(e => e.SubsectionId == id);
            return found;
        }
    }
}
