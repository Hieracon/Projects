using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GASF.Services.Interfaces;
using GASF.Models;

namespace GASF.Services.ImplementationServices
{
    public class CourseGradeService
    {
        private IRepositoryWrapper _repo;

        public CourseGradeService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<CourseGrade> GetAllCourseGrades()
        {
            return this._repo.CourseGrade.FindAll();
        }
        public CourseGrade GetDetailsById(int? id)
        {
            return _repo.CourseGrade.FindByCondition(m => m.CourseGradeId == id);
        }
        public List<Student> GetAllStudents()
        {
            return _repo.Student.FindAll();
        }
        public List<Course> GetAllCourses()
        {
            return _repo.Course.FindAll();
        }
        public void Create(CourseGrade courseGrade)
        {
            _repo.CourseGrade.Create(courseGrade);
            _repo.Save();
        }
        public void UpdateCourseGrade(CourseGrade courseGrade)
        {
            _repo.CourseGrade.Update(courseGrade);
            _repo.Save();
        }
        public bool CourseGradeExists(int id)
        {
            bool found = _repo.CourseGrade.CourseGradeExists(id);
            return found;
        }
        public void DeleteCourseGrade(int id)
        {
            var courseGrade = _repo.CourseGrade.FindByCondition(m => m.CourseGradeId == id);
            _repo.CourseGrade.Delete(courseGrade);

            _repo.Save();
        }
    }
}