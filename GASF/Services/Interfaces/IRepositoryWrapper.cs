using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Services.Interfaces
{
    public interface IRepositoryWrapper
    {
        public ICourseAttendanceRepository CourseAttendance { get; }
        public ICourseGradeRepository CourseGrade { get; }
        public ICourseMaterialRepository CourseMaterial { get; }
        public ICourseRepository Course { get; }
        public ISecretaryRepository Secretary { get; }
        public IStudentRepository Student { get; }
        public ITeacherRepository Teacher { get; }

        public void Save();
    }
}
