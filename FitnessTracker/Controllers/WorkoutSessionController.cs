using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Models;
using FitnessTracker.Repositories;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkoutSessionController : ControllerBase
    {
        private readonly IWorkoutSession w;
        public WorkoutSessionController(IWorkoutSession _w)
        {
            w = _w;
        }

        [HttpGet("GetWorkoutSessionsByMember/{memberId}")]
        public async Task<IActionResult> GetWorkoutSessionsByMember(int memberId)
        {
            IEnumerable<WorkoutSession> res = await w.GetWorkoutSessionsByMember(memberId);
            if (res.Count() == 0)
            {
                return NotFound("No records found");
            }
            return Ok(res);
        }

        [HttpGet("GetWorkoutSessionsByDateRange")]
        public async Task<IActionResult> GetWorkoutSessionsByDateRange([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            IEnumerable<WorkoutSession> res = await w.GetWorkoutSessionsByDateRange(startDate, endDate);
            if (res.Count() == 0)
            {
                return NotFound("No records found");
            }
            return Ok(res);
        }

        [HttpGet("GetWorkoutSessionsWithHighestCaloriesByExerciseType")]
        public async Task<IActionResult> GetWorkoutSessionsWithHighestCaloriesByExerciseType()
        {
            // IEnumerable<WorkoutSession> res = await t.GetWorkoutSessionsWithHighestCaloriesByExerciseType();
            // if (res.Count() == 0)
            // {
            //     return NotFound("No records found");
            // }
            return Ok();

        }
    }
}