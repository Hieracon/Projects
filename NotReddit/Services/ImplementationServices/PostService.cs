using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.ImplementationServices
{
    public class PostService
    {
        private IRepositoryWrapper _repo;

        public PostService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Post> GetAllPosts()
        {
            return this._repo.Post.FindAll();
        }
        public Post GetDetailsById(int? id)
        {
            return _repo.Post.FindByCondition(m => m.PostId == id);
        }
        public List<User> GetAllUsers()
        {
            return _repo.User.FindAll();
        }
        public List<Subsection> GetAllSubsections()
        {
            return _repo.Subsection.FindAll();
        }
        public void Create(Post post)
        {
            _repo.Post.Create(post);
            _repo.Save();
        }
        public void UpdatePost(Post post)
        {
            _repo.Post.Update(post);
            _repo.Save();
        }
        public bool PostExists(int id)
        {
            bool found = _repo.Post.PostExists(id);
            return found;
        }
        public void DeletePost(int id)
        {
            var post = _repo.Post.FindByCondition(m => m.PostId == id);
            _repo.Post.Delete(post);

            _repo.Save();
        }
    }
}
