using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ProductEFAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ProductEFAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeDatabaseContext context;
        public EmployeeController(EmployeeDatabaseContext _Context)
        {
            context = _Context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var employee = context.Employees.ToList();
            return Ok(employee);
        }
    }
}