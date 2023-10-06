using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class UserExerciseDto
    {
        public int UserExerciseId { get; set; }

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
