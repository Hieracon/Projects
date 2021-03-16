using GASF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Services.Interfaces
{
    public interface ICourseGradeRepository : IRepositoryBase<CourseGrade>
    {
        public bool CourseGradeExists(int id);
    }
}
