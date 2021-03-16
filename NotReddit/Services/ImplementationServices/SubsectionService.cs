using NotReddit.Models;
using NotReddit.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotReddit.Services.ImplementationServices
{
    public class SubsectionService
    {
        private IRepositoryWrapper _repo;

        public SubsectionService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Subsection> GetAllSubsections()
        {
            return this._repo.Subsection.FindAll();
        }
        public Subsection GetDetailsById(int? id)
        {
            return _repo.Subsection.FindByCondition(m => m.SubsectionId == id);
        }
        public void Create(Subsection subsection)
        {
            _repo.Subsection.Create(subsection);
            _repo.Save();
        }
        public void UpdateSubsection(Subsection subsection)
        {
            _repo.Subsection.Update(subsection);
            _repo.Save();
        }
        public bool SubsectionExists(int id)
        {
            bool found = _repo.Subsection.SubsectionExists(id);
            return found;
        }
        public void DeleteSubsection(int id)
        {
            var subsection = _repo.Subsection.FindByCondition(m => m.SubsectionId == id);
            _repo.Subsection.Delete(subsection);

            _repo.Save();
        }
    }
}
