using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using FitnessTracker.Data;

namespace FitnessTracker.Repositories
{
    public class TrainerRepository : ITrainer
    {
        private readonly FitnessTrackerContext db;
        public TrainerRepository(FitnessTrackerContext _db)
        {
            db = _db;
        }
        public async Task<IEnumerable<Trainer>> GetTrainersBySpeciality(string speciality)
        {
            return await db.Trainers.Where(t => t.Speciality == speciality).ToListAsync();
            // if (res


            // return true;
        }
        public async Task<Trainer> GetTrainerWithMostSessions()
        {
            var res = await db.Trainers.OrderByDescending(t => t.WorkoutSessions.Count()).FirstAsync();
            return res;
        }
        public async Task<IEnumerable<Trainer>> GetTrainersWithHighestAverageSessionDuration()
        {
            var trainersWithAverages = db.Trainers.Select(trainer => new { Trainer = trainer, AverageDuration = trainer.WorkoutSessions.Any() ? trainer.WorkoutSessions.Average(ws => ws.DurationMinutes) : 0 });

            var maxAverageDuration = await trainersWithAverages.MaxAsync(t => t.AverageDuration);

            return await trainersWithAverages.Where(t => t.AverageDuration == maxAverageDuration).Select(t => t.Trainer).ToListAsync();
        }
    }
}