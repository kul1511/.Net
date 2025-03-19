using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CourseManagement.Data;
using CourseManagement.Interface;
using CourseManagement.Models;
using CourseManagement.Repositories;

namespace CourseManagement.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CourseController : ControllerBase
    {
        private readonly IRepository<Course> c;
        public CourseController(IRepository<Course> _c)
        {
            c = _c;
        }

        [HttpGet("GetAllCourses")]
        public IActionResult GetAllCourses()
        {
            var courses = c.GetAll();
            return Ok(courses);
        }

        [HttpGet("GetCourse/{courseId}")]
        public IActionResult GetCourse(int courseId)
        {
            var course = c.GetById(courseId);
            if (course == null)
            {
                return NotFound();
            }
            return Ok(course);
        }

        [HttpPost("AddCourse")]
        public IActionResult AddCourse([FromBody] Course newCourse)
        {
            if (c.AddItem(newCourse))
            {
                return Ok("New Course Added");
            }
            return BadRequest();
        }

        [HttpPut("UpdateCourse")]
        public IActionResult UpdateCourse([FromBody] Course newCourse)
        {
            if (c.UpdateItem(newCourse))
            {
                return Ok("Updated");
            }
            return NotFound();
        }

        [HttpDelete("DeleteCourse/{courseId}")]
        public IActionResult DeleteCourse(int courseId)
        {
            if (c.RemoveItem(courseId))
            {
                return Ok("Deleted");
            }
            return NotFound();
        }
    }
}