using NotReddit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Interfaces
{
    public interface IPostRepository : IRepositoryBase<Post>
    {
        public bool PostExists(int id);
    }
}
