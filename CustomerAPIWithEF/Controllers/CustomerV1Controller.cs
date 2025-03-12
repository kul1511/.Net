using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomerAPIWithEF.Models;

namespace CustomerAPIWithEF
{
    [ApiController]
    [Route("api/customer")]
    public class CustomerV1Controller : ControllerBase
    {
        private readonly EmployeeDatabaseContext context;
        public CustomerV1Controller(EmployeeDatabaseContext _Context)
        {
            context = _Context;
        }

        [HttpGet("GetAllV1")]
        public IActionResult GetAllV1()
        {
            var cust1 = context.CustomerV1new.ToList();
            return Ok(cust1);
        }

        [HttpPost("AddCustomerV1")]
        public IActionResult AddCustomerV1([FromBody] CustomerV1new c)
        {
            foreach (var v in context.CustomerV1new)
            {
                if (v.Id == c.Id) return BadRequest();
            }
            if (c == null)
            {
                return BadRequest();
            }
            context.CustomerV1new.Add(c);
            context.SaveChanges();
            return Ok(context.CustomerV1new.ToList());
        }

        [HttpPut("UpdateCustomerV1/{id}")]
        public IActionResult UpdateCustomerV1(int id, [FromBody] CustomerV1new c)
        {
            var existing = context.CustomerV1new.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.Name = c.Name;
                existing.Address = c.Address;
                context.SaveChanges();
                return Ok(existing);
            }
            return BadRequest();
        }

        [HttpDelete("DeleteCustomerV1/{id}")]
        public IActionResult DeleteCustomerV1(int id)
        {
            var delete = context.CustomerV1new.FirstOrDefault(c => c.Id == id);
            if (delete != null)
            {
                context.CustomerV1new.Remove(delete);
                context.SaveChanges();
                return Ok("Successfully Deleted");
            }
            return BadRequest();
        }

        //From Query
        [HttpPost("AddCustomerV1fromQuery")]
        public IActionResult AddCustomerV1fromQuery([FromQuery] CustomerV1new c)
        {
            foreach (var v in context.CustomerV1new)
            {
                if (v.Id == c.Id) return BadRequest();
            }
            if (c == null)
            {
                return BadRequest();
            }
            context.CustomerV1new.Add(c);
            context.SaveChanges();
            return Ok(context.CustomerV1new.ToList());
        }

        //From Route
        [HttpPost("AddCustomerV1fromRoute/{id}")]
        public IActionResult AddCustomerV1fromRoute([FromRoute] int id)
        {
            var res = context.CustomerV1new.FirstOrDefault(c => c.Id == id);
            if (res == null) return BadRequest();

            return Ok(res);
        }
    }
}