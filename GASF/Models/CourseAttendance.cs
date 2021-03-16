using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Models
{
    public class CourseAttendance
    {
        public int CourseAttendanceId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public int NrCourseAttendances { get; set; }
        public int NrLaboratoryAttendances { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
