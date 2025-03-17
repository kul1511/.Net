using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Data;
using FitnessTracker.Models;
using FitnessTracker.Repositories;
using FitnessTracker.DTO;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MemberController : ControllerBase
    {
        private readonly IMember m;
        public MemberController(IMember _m)
        {
            m = _m;
        }

        [HttpGet("GetAllMembers")]
        public async Task<IActionResult> GetAllMembers()
        {
            var res = await m.GetAllMembers();
            return Ok(res);
        }

        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember([FromBody] MemberDTO member)
        {
            if (member == null)
            {
                return BadRequest("Empty Request");
            }
            bool res = await m.AddMember(member);
            if (!res)
            {
                return BadRequest("Member already exists");
            }
            return Ok("Added");
        }

        [HttpPut("UpdateMemberEmail/{id}")]
        public async Task<IActionResult> UpdateMemberEmail(int id, [FromBody] string email)
        {
            bool res = await m.UpdateMemberEmail(id, email);
            if (res)
            {
                return Ok("Email Updated Successfully");
            }
            return BadRequest();
        }

        [HttpDelete("DeleteMember/{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            bool res = await m.DeleteMember(id);

            if (res)
            {
                return Ok("Member Deleted Successfully");
            }
            return BadRequest();
        }

        [HttpGet("GetHighestCaloriesBurned/{num}")]
        public async Task<IActionResult> GetHighestCaloriesBurned(int num)
        {
            IEnumerable<Member> res = await m.GetTopMembersWithHighestCaloriesBurned(num);
            if (res.Count() == 0)
            {
                return Ok($"There are No Members with {num} count.");
            }
            return Ok(res);

        }
    }
}