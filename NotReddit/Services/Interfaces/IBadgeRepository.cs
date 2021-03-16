using NotReddit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Interfaces
{
    public interface IBadgeRepository : IRepositoryBase<Badge>
    {
        public bool BadgeExists(int id);
    }
}
