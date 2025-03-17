using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public interface IWorkoutSession
    {
        Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsByMember(int memberId);
        Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsByDateRange(DateTime startDate, DateTime endDate);
        // Task<IEnumerable<WorkoutSession>> GetWorkoutSessionsWithHighestCaloriesByExerciseType();
    }
}