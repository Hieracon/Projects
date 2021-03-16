using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Interfaces
{
    public interface IRepositoryWrapper
    {
        public IBadgeRepository Badge { get; }
        public ICommentRepository Comment { get; }
        public ILikeRepository Like { get; }
        public IPostRepository Post { get; }
        public ISubsectionRepository Subsection { get; }
        public IUserRepository User { get; }

        public void Save();
    }
}
