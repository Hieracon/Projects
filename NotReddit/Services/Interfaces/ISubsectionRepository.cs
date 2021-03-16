using NotReddit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Interfaces
{
    public interface ISubsectionRepository : IRepositoryBase<Subsection>
    {
        public bool SubsectionExists(int id);
    }
}
