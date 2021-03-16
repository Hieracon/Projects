using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GASF.Services.Interfaces;
using GASF.Models;

namespace GASF.Services.ImplementationServices
{
    public class TeacherService
    {
        private IRepositoryWrapper _repo;

        public TeacherService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Teacher> GetAllTeachers()
        {
            return _repo.Teacher.FindAll();
        }
        public Teacher GetDetailsById(string? id)
        {
            return _repo.Teacher.FindByCondition(m => m.TeacherId == id);
        }
        public void Create(Teacher teacher)
        {
            _repo.Teacher.Create(teacher);
            _repo.Save();
        }

        public void UpdateTeacher(Teacher teacher)
        {
            _repo.Teacher.Update(teacher);
            _repo.Save();
        }
        public bool TeacherExists(string id)
        {
            bool found = _repo.Teacher.TeacherExists(id);
            return found;
        }
        public void DeleteTeacher(string id)
        {
            var teacher = _repo.Teacher.FindByCondition(m => m.TeacherId == id);
            _repo.Teacher.Delete(teacher);

            _repo.Save();
        }
    }
}