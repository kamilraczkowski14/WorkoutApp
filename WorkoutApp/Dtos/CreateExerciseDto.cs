using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class CreateExerciseDto
    {
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
    }
}
