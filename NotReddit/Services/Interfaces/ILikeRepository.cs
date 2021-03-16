using NotReddit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Interfaces
{
    public interface ILikeRepository : IRepositoryBase<Like>
    {
        public bool LikeExists(int id);
    }
}
