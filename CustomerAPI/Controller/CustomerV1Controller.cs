using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CustomerAPI.Model;
using CustomerAPI.Controller;
using CustomerAPI.Interface;

namespace CustomerAPI.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerV1Controller : ControllerBase
    {
        private readonly ICustomerV1Repository cr;
        public CustomerV1Controller(ICustomerV1Repository _cr)
        {
            cr = _cr;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var customer = cr.GetAll();
            // if (customer != null)
            // {
            //     return Ok(customer);
            // }
            // return BadRequest();
            return Ok(customer);
        }

        [HttpPost("AddCustomerV1")]
        public IActionResult AddCustomerV1([FromBody] CustomerV1 c)
        {
            if (cr.AddCustomerV1(c)) return Ok("Successfully Added Customer");
            else return BadRequest("Check you request either your request is not correct or it already contains the Customer");
        }

        [HttpPut("UpdateCustomerV1/{id}")]
        public IActionResult UpdateCustomerV1(int id, [FromBody] CustomerV1 c)
        {
            var res = cr.UpdateCustomerV1(id, c);
            if (res == null)
            {
                return BadRequest("The Customer you want to update is not Present.");
            }
            else
            {
                return Ok(cr.GetAll());
            }
        }

        [HttpDelete("DeleteCustomerV1/{id}")]
        public IActionResult DeleteCustomerV1(int id)
        {
            if (cr.DeleteCustomerV1(id))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("The Customer you want to Delete is not Present.");
        }

    }
}