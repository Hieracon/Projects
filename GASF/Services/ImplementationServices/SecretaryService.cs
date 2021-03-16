using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GASF.Services.Interfaces;
using GASF.Models;

namespace GASF.Services.ImplementationServices
{
    public class SecretaryService
    {
        private IRepositoryWrapper _repo;

        public SecretaryService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Secretary> GetAllSecretaries()
        {
            return _repo.Secretary.FindAll();
        }
        public Secretary GetDetailsById(string? id)
        {
            return _repo.Secretary.FindByCondition(m => m.SecretaryId == id);
        }
        public void Create(Secretary secretary)
        {
            _repo.Secretary.Create(secretary);
            _repo.Save();
        }

        public void UpdateSecretary(Secretary secretary)
        {
            _repo.Secretary.Update(secretary);
            _repo.Save();
        }
        public bool SecretaryExists(string id)
        {
            bool found = _repo.Secretary.SecretaryExists(id);
            return found;
        }
        public void DeleteSecretary(string id)
        {
            var secretary = _repo.Secretary.FindByCondition(m => m.SecretaryId == id);
            _repo.Secretary.Delete(secretary);

            _repo.Save();
        }
    }
}