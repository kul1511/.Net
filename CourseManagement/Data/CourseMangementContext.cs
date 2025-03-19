using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using CourseManagement.Models;
using System.Threading.Tasks;

namespace CourseManagement.Data
{
    public class CourseManagementContext : DbContext
    {
        public CourseManagementContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Course> Courses { get; set; }

    }
}