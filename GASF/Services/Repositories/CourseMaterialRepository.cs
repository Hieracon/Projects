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
    public class CourseMaterialRepository : RepositoryBase<CourseMaterial>, ICourseMaterialRepository
    {
        public CourseMaterialRepository(GASFContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool CourseMaterialExists(int id)
        {
            var found = RepositoryContext.CourseMaterials.Any(e => e.CourseMaterialId == id);
            return found;
        }

        public CourseMaterial FindByCondition(Expression<Func<CourseMaterial, bool>> expression)
        {
            return this.RepositoryContext.CourseMaterials
                .Include(c => c.Course)
                .Where(expression)
                .FirstOrDefault();
        }

        public List<CourseMaterial> FindAll()
        {
            return this.RepositoryContext.CourseMaterials
                .Include(c => c.Course)
                .ToList();

        }
    }
}
