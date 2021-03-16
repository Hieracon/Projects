using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.ImplementationServices
{
    public class CommentService
    {
        private IRepositoryWrapper _repo;

        public CommentService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Comment> GetAllComments()
        {
            return this._repo.Comment.FindAll();
        }
        public Comment GetDetailsById(int? id)
        {
            return _repo.Comment.FindByCondition(m => m.CommentId == id);
        }
        public List<Post> GetAllPosts()
        {
            return _repo.Post.FindAll();
        }
        public List<User> GetAllUsers()
        {
            return _repo.User.FindAll();
        }
        public void Create(Comment comment)
        {
            _repo.Comment.Create(comment);
            _repo.Save();
        }
        public void UpdateComment(Comment comment)
        {
            _repo.Comment.Update(comment);
            _repo.Save();
        }
        public bool CommentExists(int id)
        {
            bool found = _repo.Comment.CommentExists(id);
            return found;
        }
        public void DeleteComment(int id)
        {
            var comment = _repo.Comment.FindByCondition(m => m.CommentId == id);
            _repo.Comment.Delete(comment);

            _repo.Save();
        }
    }
}
