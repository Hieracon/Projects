using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GASF.Services.Interfaces;
using GASF.Models;

namespace GASF.Services.ImplementationServices
{
    public class CourseMaterialService
    {
        private IRepositoryWrapper _repo;

        public CourseMaterialService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<CourseMaterial> GetAllCourseMaterials()
        {
            return this._repo.CourseMaterial.FindAll();
        }
        public CourseMaterial GetDetailsById(int? id)
        {
            return _repo.CourseMaterial.FindByCondition(m => m.CourseMaterialId == id);
        }
        public List<Course> GetAllCourses()
        {
            return _repo.Course.FindAll();
        }
        public void Create(CourseMaterial courseMaterial)
        {
            _repo.CourseMaterial.Create(courseMaterial);
            _repo.Save();
        }
        public void UpdateCourseMaterial(CourseMaterial courseMaterial)
        {
            _repo.CourseMaterial.Update(courseMaterial);
            _repo.Save();
        }
        public bool CourseMaterialExists(int id)
        {
            bool found = _repo.CourseMaterial.CourseMaterialExists(id);
            return found;
        }
        public void DeleteCourseMaterial(int id)
        {
            var courseMaterial = _repo.CourseMaterial.FindByCondition(m => m.CourseMaterialId == id);
            _repo.CourseMaterial.Delete(courseMaterial);

            _repo.Save();
        }
    }
}