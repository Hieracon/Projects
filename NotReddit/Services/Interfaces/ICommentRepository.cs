using NotReddit.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Interfaces
{
    public interface ICommentRepository : IRepositoryBase<Comment>
    {
        public bool CommentExists(int id);
    }
}
