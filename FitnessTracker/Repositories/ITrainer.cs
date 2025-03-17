using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessTracker.Models;

namespace FitnessTracker.Repositories
{
    public interface ITrainer
    {
        Task<IEnumerable<Trainer>> GetTrainersBySpeciality(string speciality);
        Task<Trainer> GetTrainerWithMostSessions();
        Task<IEnumerable<Trainer>> GetTrainersWithHighestAverageSessionDuration();

    }
}