using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GASF.Services.Interfaces;
using GASF.Models;

namespace GASF.Services.ImplementationServices
{
    public class CourseAttendanceService
    {
        private IRepositoryWrapper _repo;

        public CourseAttendanceService(IRepositoryWrapper repo)
        {
            this._repo = repo;
        }
        public List<CourseAttendance> GetAllCourseAttendances()
        {
            return this._repo.CourseAttendance.FindAll();
        }
        public CourseAttendance GetDetailsById(int? id)
        {
            return _repo.CourseAttendance.FindByCondition(m => m.CourseAttendanceId == id);
        }
        public List<Student> GetAllStudents()
        {
            return _repo.Student.FindAll();
        }
        public List<Course> GetAllCourses()
        {
            return _repo.Course.FindAll();
        }
        public void Create(CourseAttendance courseAttendance)
        {
            _repo.CourseAttendance.Create(courseAttendance);
            _repo.Save();
        }
        public void UpdateCourseAttendance(CourseAttendance courseAttendance)
        {
            _repo.CourseAttendance.Update(courseAttendance);
            _repo.Save();
        }
        public bool CourseAttendanceExists(int id)
        {
            bool found = _repo.CourseAttendance.CourseAttendanceExists(id);
            return found;
        }
        public void DeleteCourseAttendance(int id)
        {
            var courseAttendance = _repo.CourseAttendance.FindByCondition(m => m.CourseAttendanceId == id);
            _repo.CourseAttendance.Delete(courseAttendance);

            _repo.Save();
        }
    }
}