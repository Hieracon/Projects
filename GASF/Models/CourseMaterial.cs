using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GASF.Models
{
    public class CourseMaterial
    {
        public int CourseMaterialId { get; set; }
        public int CourseId { get; set; }
        public string LinkCourseMaterial { get; set; }
        public string LinkSeminarMaterial { get; set; }
        public string LinkLaboratoryMaterial { get; set; }
        
        public Course Course { get; set; }
    }
}
