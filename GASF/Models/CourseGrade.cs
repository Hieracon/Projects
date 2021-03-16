using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Models
{
    public class CourseGrade
    {
        public int CourseGradeId { get; set; }
        public string StudentId { get; set; }
        public int CourseId { get; set; }
        public double ExamGrade { get; set; }
        public double LabGrade { get; set; }
        public double BonusPoints { get; set; }
        public bool IsGraduated { get; set; }

        public Student Student { get; set; }
        public Course Course { get; set; }
    }
}
