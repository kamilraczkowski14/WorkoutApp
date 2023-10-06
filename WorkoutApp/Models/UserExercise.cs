using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class UserExercise
    {
        public int UserExerciseId { get; set; }

        [Required]
        public string ExerciseName { get; set; }
        public string Description { get; set; }

        [Required]
        public string BodyPart { get; set; }

        [Required]
        public int? NumberOfSeries { get; set; }

        [Required]
        public int? NumberOfRepeats { get; set; }

        public int WorkoutDayId { get; set; }

        public virtual WorkoutDay WorkoutDay { get; set; }
    }
}
