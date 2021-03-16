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
    public class StudentRepository : RepositoryBase<Student>, IStudentRepository
    {
        public StudentRepository(GASFContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool StudentExists(string id)
        {
            var found = RepositoryContext.Students.Any(e => e.StudentId == id);
            return found;
        }

    }
}
