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
    public class SecretaryRepository : RepositoryBase<Secretary>, ISecretaryRepository
    {
        public SecretaryRepository(GASFContext repositoryContext)
            : base(repositoryContext)
        {
        }

        public bool SecretaryExists(string id)
        {
            var found = RepositoryContext.Secretaries.Any(e => e.SecretaryId == id);
            return found;
        }
    }
}
