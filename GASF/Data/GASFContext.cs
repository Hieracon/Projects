using System;
using System.Collections.Generic;
using System.Text;
using GASF.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GASF.Data
{
    public class GASFContext : IdentityDbContext
    {
        public GASFContext(DbContextOptions<GASFContext> options)
            : base(options)
        {
        }

        public DbSet<Course> Courses { get; set; }
        public DbSet<CourseAttendance> CourseAttendances { get; set; }
        public DbSet<CourseGrade> CourseGrades { get; set; }
        public DbSet<CourseMaterial> CourseMaterials { get; set; }
        public DbSet<Secretary> Secretaries { get; set; }
        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }

    }
}
