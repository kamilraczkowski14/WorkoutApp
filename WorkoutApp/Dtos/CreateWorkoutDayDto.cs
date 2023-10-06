using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class CreateWorkoutDayDto
    {
        public int WorkoutPlanId { get; set; }

        [Required]
        public string CalendarDate { get; set; }

    }
}
