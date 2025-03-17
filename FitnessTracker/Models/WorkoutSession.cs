using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace FitnessTracker.Models
{
    public class WorkoutSession
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public DateTime SessionDate { get; set; }
        public string ExerciseType { get; set; }
        public int DurationMinutes { get; set; }
        public int CaloriesBurned { get; set; }
        public Member member { get; set; }
        public int TrainerId { get; set; }
        public Trainer trainer { get; set; }
    }
}