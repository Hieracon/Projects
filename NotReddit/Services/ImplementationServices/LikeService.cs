using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.ImplementationServices
{
    public class LikeService
    {
        private IRepositoryWrapper _repo;

        public LikeService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Like> GetAllLikes()
        {
            return this._repo.Like.FindAll();
        }
        public Like GetDetailsById(int? id)
        {
            return _repo.Like.FindByCondition(m => m.LikeId == id);
        }
        public List<Post> GetAllPosts()
        {
            return _repo.Post.FindAll();
        }
        public List<Comment> GetAllComments()
        {
            return _repo.Comment.FindAll();
        }
        public void Create(Like like)
        {
            _repo.Like.Create(like);
            _repo.Save();
        }
        public void UpdateLike(Like like)
        {
            _repo.Like.Update(like);
            _repo.Save();
        }
        public bool LikeExists(int id)
        {
            bool found = _repo.Like.LikeExists(id);
            return found;
        }
        public void DeleteLike(int id)
        {
            var like = _repo.Like.FindByCondition(m => m.LikeId == id);
            _repo.Like.Delete(like);

            _repo.Save();
        }
    }
}
