using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Models
{
    public class Course
    {
        public int CourseId { get; set; }
        public string TeacherId { get; set; }
        public string Name { get; set; }
        public int NrCredits { get; set;}
        public int CourseYear { get; set; }
        public string Section { get; set; }
        public string CourseType { get; set; }
        public int Semester { get; set; }
        public string GradingMethod { get; set; }
        public bool HasLaboratory { get; set; }
        public bool HasSeminar { get; set; }

        public Teacher Teacher { get; set; }

        public CourseMaterial CourseMaterial { get; set; }
        public ICollection<CourseAttendance> CourseAttendances { get; set; }
        public ICollection<CourseGrade> CourseGrades { get; set; }
    }
}
