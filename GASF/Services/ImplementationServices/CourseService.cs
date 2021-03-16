using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GASF.Services.Interfaces;
using GASF.Models;

namespace GASF.Services.ImplementationServices
{
    public class CourseService
    {
        private IRepositoryWrapper _repo;

        public CourseService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<Course> GetAllCourses()
        {
            return this._repo.Course.FindAll();
        }
        public Course GetDetailsById(int? id)
        {
            return _repo.Course.FindByCondition(m => m.CourseId == id);
        }
        public List<Teacher> GetAllTeachers()
        {
            return _repo.Teacher.FindAll();
        }
        public void Create(Course course)
        {
            _repo.Course.Create(course);
            _repo.Save();
        }
        public void UpdateCourse(Course course)
        {
            _repo.Course.Update(course);
            _repo.Save();
        }
        public bool CourseExists(int id)
        {
            bool found = _repo.Course.CourseExists(id);
            return found;
        }
        public void DeleteCourse(int id)
        {
            var course = _repo.Course.FindByCondition(m => m.CourseId == id);
            _repo.Course.Delete(course);

            _repo.Save();
        }
    }
}