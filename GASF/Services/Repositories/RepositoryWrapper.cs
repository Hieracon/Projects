using GASF.Data;
using GASF.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Services.Repositories
{
    public class RepositoryWrapper : IRepositoryWrapper
    {
        private GASFContext _repoContext;

        private ICourseAttendanceRepository _courseAttendance;
        private ICourseGradeRepository _courseGrade;
        private ICourseMaterialRepository _courseMaterial;
        private ICourseRepository _course;
        private ISecretaryRepository _secretary;
        private IStudentRepository _student;
        private ITeacherRepository _teacher;

        public ICourseAttendanceRepository CourseAttendance
        {
            get
            {
                if (_courseAttendance == null)
                {
                    _courseAttendance = new CourseAttendanceRepository(_repoContext);
                }

                return _courseAttendance;
            }
        }

        public ICourseGradeRepository CourseGrade
        {
            get
            {
                if (_courseGrade == null)
                {
                    _courseGrade = new CourseGradeRepository(_repoContext);
                }

                return _courseGrade;
            }
        }

        public ICourseMaterialRepository CourseMaterial
        {
            get
            {
                if (_courseMaterial == null)
                {
                    _courseMaterial = new CourseMaterialRepository(_repoContext);
                }

                return _courseMaterial;
            }
        }

        public ICourseRepository Course
        {
            get
            {
                if (_course == null)
                {
                    _course = new CourseRepository(_repoContext);
                }

                return _course;
            }
        }

        public ISecretaryRepository Secretary
        {
            get
            {
                if (_secretary == null)
                {
                    _secretary = new SecretaryRepository(_repoContext);
                }

                return _secretary;
            }
        }

        public IStudentRepository Student
        {
            get
            {
                if (_student == null)
                {
                    _student = new StudentRepository(_repoContext);
                }

                return _student;
            }
        }

        public ITeacherRepository Teacher
        {
            get
            {
                if (_teacher == null)
                {
                    _teacher = new TeacherRepository(_repoContext);
                }

                return _teacher;
            }
        }

        public RepositoryWrapper(GASFContext repositoryContext)
        {
            _repoContext = repositoryContext;
        }

        public void Save()
        {
            _repoContext.SaveChanges();
        }
    }
}
