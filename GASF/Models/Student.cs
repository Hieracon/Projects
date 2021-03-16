using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Models
{
    public class Student
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string StudentId { get; set; }
        public string CNP { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MailAddress { get; set; }
        public string Password { get; set; }
        public string DadInitials { get; set; }
        public string Address { get; set; }
        public string Birthday { get; set; }
        public string Credits { get; set; }
        public int CurrentYear { get; set; }
        public string Section { get; set; }

        public ICollection<CourseAttendance> CourseAttendances { get; set; }
        public ICollection<CourseGrade> CourseGrades { get; set; }
    }
}
