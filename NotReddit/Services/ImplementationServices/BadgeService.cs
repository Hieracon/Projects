using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.ImplementationServices
{
    public class BadgeService
    {
        private IRepositoryWrapper _repo;

        public BadgeService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Badge> GetAllBadges()
        {
            return _repo.Badge.FindAll();
        }
        public Badge GetDetailsById(int? id)
        {
            return _repo.Badge.FindByCondition(m => m.BadgeId == id);
        }
        public void Create(Badge badge)
        {
            _repo.Badge.Create(badge);
            _repo.Save();
        }

        public void UpdateBadge(Badge badge)
        {
            _repo.Badge.Update(badge);
            _repo.Save();
        }
        public bool BadgeExists(int id)
        {
            bool found = _repo.Badge.BadgeExists(id);
            return found;
        }
        public void DeleteBadge(int id)
        {
            var badge = _repo.Badge.FindByCondition(m => m.BadgeId == id);
            _repo.Badge.Delete(badge);

            _repo.Save();
        }
    }
}
