using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FitnessTracker.Repositories;
using FitnessTracker.Models;

namespace FitnessTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainerController : ControllerBase
    {
        private readonly ITrainer t;
        public TrainerController(ITrainer _t)
        {
            t = _t;
        }

        [HttpGet("GetTrainersBySpeciality/{speciality}")]
        public async Task<IActionResult> GetTrainersBySpeciality(string speciality)
        {
            IEnumerable<Trainer> res = await t.GetTrainersBySpeciality(speciality);
            if (res.Count() == 0)
            {
                return NotFound();
            }
            return Ok(res);
        }

        [HttpGet("GetTrainerWithMostSessions")]
        public async Task<IActionResult> GetTrainerWithMostSessions()
        {
            var res = await t.GetTrainerWithMostSessions();
            if (res == null)
            {
                return NotFound("No records found");
            }
            return Ok(res);
        }

        [HttpGet("GetTrainersWithHighestAverageSessionDuration")]
        public async Task<IActionResult> GetTrainersWithHighestAverageSessionDuration()
        {
            IEnumerable<Trainer> res = await t.GetTrainersWithHighestAverageSessionDuration();
            if (res == null)
            {
                return NotFound("No records found");
            }
            return Ok(res);
        }
    }
}