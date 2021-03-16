using GASF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Services.Interfaces
{
    public interface ISecretaryRepository : IRepositoryBase<Secretary>
    {
        public bool SecretaryExists(string id);
    }
}
