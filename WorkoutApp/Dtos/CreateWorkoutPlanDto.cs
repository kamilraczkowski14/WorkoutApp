using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class CreateWorkoutPlanDto
    {
        [Required]
        public string Name { get; set; }
        public bool? isPreferred { get; set; } = false;
        public int UserId { get; set; }
    }
}
