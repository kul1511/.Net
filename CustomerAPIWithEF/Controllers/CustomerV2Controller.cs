using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomerAPIWithEF.Models;

namespace CustomerAPIWithEF
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerV2Controller : ControllerBase
    {
        private readonly EmployeeDatabaseContext context;
        public CustomerV2Controller(EmployeeDatabaseContext _Context)
        {
            context = _Context;
        }

        [HttpGet("GetAllV2")]
        public IActionResult GetAllV2()
        {
            var customerV2 = context.CustomerV2new.ToList();
            return Ok(customerV2);
        }

        [HttpPost("AddCustomerV2")]
        public IActionResult AddCustomerV2([FromBody] CustomerV2new c)
        {
            foreach (var v in context.CustomerV2new)
            {
                if (v.Id == c.Id) return BadRequest();
            }
            if (c == null)
            {
                return BadRequest();
            }
            context.CustomerV2new.Add(c);
            context.SaveChanges();
            return Ok(context.CustomerV2new.ToList());
        }

        [HttpPut("UpdateCustomerV2/{id}")]
        public IActionResult UpdateCustomerV2(int id, [FromBody] CustomerV2new c)
        {
            var existing = context.CustomerV2new.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.Name = c.Name;
                existing.Address = c.Address;
                context.SaveChanges();
                return Ok(existing);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteCustomerV2/{id}")]
        public IActionResult DeleteCustomerV2(int id)
        {
            var delete = context.CustomerV2new.FirstOrDefault(c => c.Id == id);
            if (delete != null)
            {
                context.CustomerV2new.Remove(delete);
                context.SaveChanges();
                return Ok("Successfully Deleted");
            }
            return BadRequest();
        }
    }
}