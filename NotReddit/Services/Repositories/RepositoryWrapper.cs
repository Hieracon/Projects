using NotReddit.Data;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private NotRedditContext _repoContext;

        private IBadgeRepository _badge;
        private ICommentRepository _comment;
        private ILikeRepository _like;
        private IPostRepository _post;
        private ISubsectionRepository _subsection;
        private IUserRepository _user;

        public IBadgeRepository Badge
        {
            get
            {
                if (_badge == null)
                {
                    _badge = new BadgeRepository(_repoContext);
                }

                return _badge;
            }
        }

        public ICommentRepository Comment
        {
            get
            {
                if (_comment == null)
                {
                    _comment = new CommentRepository(_repoContext);
                }

                return _comment;
            }
        }

        public ILikeRepository Like
        {
            get
            {
                if (_like == null)
                {
                    _like = new LikeRepository(_repoContext);
                }

                return _like;
            }
        }

        public IPostRepository Post
        {
            get
            {
                if (_post == null)
                {
                    _post = new PostRepository(_repoContext);
                }

                return _post;
            }
        }

        public ISubsectionRepository Subsection
        {
            get
            {
                if (_subsection == null)
                {
                    _subsection = new SubsectionRepository(_repoContext);
                }

                return _subsection;
            }
        }

        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_repoContext);
                }

                return _user;
            }
        }

        public RepositoryWrapper(NotRedditContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
