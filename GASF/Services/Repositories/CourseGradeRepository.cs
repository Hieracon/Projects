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
    public class CourseGradeRepository : RepositoryBase<CourseGrade>, ICourseGradeRepository
    {
        public CourseGradeRepository(GASFContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool CourseGradeExists(int id)
        {
            var found = RepositoryContext.CourseGrades.Any(e => e.CourseGradeId == id);
            return found;
        }

        public CourseGrade FindByCondition(Expression<Func<CourseGrade, bool>> expression)
        {
            return this.RepositoryContext.CourseGrades
                .Include(c => c.Student)
                .Include(c => c.Course)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<CourseGrade> FindAll()
        {
            return this.RepositoryContext.CourseGrades
                .Include(c => c.Student)
                .Include(c => c.Course)
                .ToList();

        }
    }
}
