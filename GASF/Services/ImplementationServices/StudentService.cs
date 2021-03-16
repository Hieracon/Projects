using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GASF.Services.Interfaces;
using GASF.Models;
using GASF.Data;

namespace GASF.Services.ImplementationServices
{
    public class StudentService
    {
        private IRepositoryWrapper _repo;
        protected GASFContext RepositoryContext { get; set; }

        public StudentService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Student> GetAllStudents()
        {
            return _repo.Student.FindAll();
        }
        public Student GetDetailsById(string? id)
        {
            return _repo.Student.FindByCondition(m => m.StudentId == id);
        }
        public void Create(Student student)
        {
            _repo.Student.Create(student);
            _repo.Save();
        }

        public void UpdateStudent(Student student)
        {
            _repo.Student.Update(student);
            _repo.Save();
        }
        public bool StudentExists(string id)
        {
            bool found = _repo.Student.StudentExists(id);
            return found;
        }
        public void DeleteStudent(string id)
        {
            var student = _repo.Student.FindByCondition(m => m.StudentId == id);
            _repo.Student.Delete(student);

            _repo.Save();
        }
    }
}