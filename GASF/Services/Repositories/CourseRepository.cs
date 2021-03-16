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
    public class CourseRepository : RepositoryBase<Course>, ICourseRepository
    {
        public CourseRepository(GASFContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool CourseExists(int id)
        {
            var found = RepositoryContext.Courses.Any(e => e.CourseId == id);
            return found;
        }

        public Course FindByCondition(Expression<Func<Course, bool>> expression)
        {
            return this.RepositoryContext.Courses
                .Include(c => c.Teacher)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<Course> FindAll()
        {
            return this.RepositoryContext.Courses
                .Include(c => c.Teacher)
                .ToList();

        }
    }
}
