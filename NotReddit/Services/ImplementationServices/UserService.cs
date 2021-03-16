using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.ImplementationServices
{
    public class UserService
    {
        private IRepositoryWrapper _repo;

        public UserService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<User> GetAllUsers()
        {
            return this._repo.User.FindAll();
        }
        public User GetDetailsById(string? id)
        {
            return _repo.User.FindByCondition(m => m.Id == id);
        }
        public List<Badge> GetAllBadges()
        {
            return _repo.Badge.FindAll();
        }
        public void Create(User user)
        {
            _repo.User.Create(user);
            _repo.Save();
        }
        public void UpdateUser(User user)
        {
            _repo.User.Update(user);
            _repo.Save();
        }
        public bool UserExists(string id)
        {
            bool found = _repo.User.UserExists(id);
            return found;
        }
        public void DeleteUser(string id)
        {
            var user = _repo.User.FindByCondition(m => m.Id == id);
            _repo.User.Delete(user);

            _repo.Save();
        }
    }
}
