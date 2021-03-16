using GASF.Data;
using GASF.Models;
using GASF.Services.Interfaces;
using GASF.Services.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GASF.Services.Repositories
{
    public class CourseAttendanceRepository : RepositoryBase<CourseAttendance>, ICourseAttendanceRepository
    {
        public CourseAttendanceRepository(GASFContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool CourseAttendanceExists(int id)
        {
            var found = RepositoryContext.CourseAttendances.Any(e => e.CourseAttendanceId == id);
            return found;
        }

        public CourseAttendance FindByCondition(Expression<Func<CourseAttendance, bool>> expression)
        {
            return this.RepositoryContext.CourseAttendances
                .Include(c => c.Student)
                .Include(c => c.Course)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<CourseAttendance> FindAll()
        {
            return this.RepositoryContext.CourseAttendances
                .Include(c => c.Student)
                .Include(c => c.Course)
                .ToList();

        }
    }
}
