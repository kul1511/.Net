using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseManagement.Interface;
using CourseManagement.Models;
using CourseManagement.Data;

namespace CourseManagement.Repositories
{
    public class CourseRepository : IRepository<Course>
    {
        private readonly CourseManagementContext c;
        public CourseRepository(CourseManagementContext _c)
        {
            c = _c;
        }
        public IEnumerable<Course> GetAll()
        {
            return c.Courses.ToList();
        }

        public Course GetById(int id)
        {
            return c.Courses.FirstOrDefault(c => c.CourseId == id);

        }

        public bool AddItem(Course newCourse)
        {
            var course = c.Courses.FirstOrDefault(c => c.CourseId == newCourse.CourseId);
            if (course == null)
            {
                c.Courses.Add(newCourse);
                c.SaveChanges();
                return true;
            }
            return false;
        }

        public bool UpdateItem(Course newCourse)
        {
            var course = c.Courses.FirstOrDefault(c => c.CourseId == newCourse.CourseId);
            if (course != null)
            {
                course.CourseName = newCourse.CourseName;
                course.CourseFee = newCourse.CourseFee;
                course.Duration = newCourse.Duration;
                course.InstructorName = newCourse.InstructorName;
                c.SaveChanges();
                return true;
            }
            return false;
        }

        public bool RemoveItem(int id)
        {
            var course = c.Courses.FirstOrDefault(c => c.CourseId == id);
            if (course != null)
            {
                c.Courses.Remove(course);
                c.SaveChanges();
                return true;
            }
            return false;
        }
    }
}