using GASF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Services.Interfaces
{
    public interface ITeacherRepository : IRepositoryBase<Teacher>
    {
        public bool TeacherExists(string id);
    }
}
