using GASF.Data;
using GASF.Models;
using GASF.Services.Interfaces;
using GASF.Services.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace GASF.Services.Repositories
{
    public class TeacherRepository : RepositoryBase<Teacher>, ITeacherRepository
    {
        public TeacherRepository(GASFContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool TeacherExists(string id)
        {
            var found = RepositoryContext.Teachers.Any(e => e.TeacherId == id);
            return found;
        }
    }
}
