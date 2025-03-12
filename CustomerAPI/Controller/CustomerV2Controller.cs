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
    public class CustomerV2Controller : ControllerBase
    {
        private readonly ICustomerV2Repository cr;
        public CustomerV2Controller(ICustomerV2Repository _cr)
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

        [HttpPost("AddCustomerV2")]
        public IActionResult Add([FromBody] CustomerV2 c)
        {
            if (cr.AddCustomerV2(c)) return Ok("Successfully Added Customer");
            else return BadRequest("Check you request either your request is not correct or it already contains the Customer");
        }

        [HttpPut("UpdateCustomerV2/{id}")]
        public IActionResult UpdateCustomerV2(int id, [FromBody] CustomerV2 c)
        {
            var res = cr.UpdateCustomerV2(id, c);
            if (res == null)
            {
                return BadRequest("The Customer you want to update is not Present.");
            }
            else
            {
                return Ok(cr.GetAll());
            }
        }

        [HttpDelete("DeleteCustomerV2/{id}")]
        public IActionResult DeleteCustomerV2(int id)
        {
            if (cr.DeleteCustomerV2(id))
            {
                return Ok("Successfully Deleted");
            }
            return BadRequest("The Customer you want to Delete is not Present.");
        }
    }
}