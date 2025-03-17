using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Models;
using FitnessTracker.Data;
using Microsoft.EntityFrameworkCore;


namespace FitnessTracker.Repositories
{
    public class WorkoutSessionRepository : IWorkoutSession
    {
        private readonly FitnessTrackerContext db;
        public WorkoutSessionRepository(FitnessTrackerContext _db)
        {
            db = _db;
        }
        public async Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsByMember(int memberId)
        {
            return await db.WorkoutSessions.Where(ws => ws.Id == memberId).ToListAsync();
        }
        public async Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsByDateRange(DateTime startDate, DateTime endDate)
        {
            return await db.WorkoutSessions.Where(ws => ws.SessionDate >= startDate && ws.SessionDate <= endDate).ToListAsync();
        }
        public async Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsWithHighestCaloriesByExerciseType()
        {
            var exerciseType = await db.WorkoutSessions.GroupBy(w => w.ExerciseType).Select(s => new { ExercisteType = s.Key, TotalCalories = s.Sum(sum => sum.CaloriesBurned) }).OrderByDescending(w => w.TotalCalories).FirstOrDefault();

            if (exerciseType == null) return new List<WorkoutSession>();

            return await db.WorkoutSessions.Where(w => w.ExerciseType == exerciseType.ExerciseType).ToListAsync();

            // return res;
        }

    }
}