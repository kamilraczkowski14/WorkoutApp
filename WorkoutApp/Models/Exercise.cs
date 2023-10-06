using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class Exercise
    {
        public int ExerciseId { get; set; }

        [Required]
        public string ExerciseName { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string BodyPart { get; set; }

        public int? NumberOfSeries { get; set; }

        public int? NumberOfRepeats { get; set; }
    }
}
