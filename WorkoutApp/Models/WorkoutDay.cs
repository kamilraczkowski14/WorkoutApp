using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class WorkoutDay
    {
        public int WorkoutDayId { get; set; }

        public int WorkoutPlanId { get; set; }

        public int CalendarDayId { get; set; }

        [Required]
        public string? CalendarDate { get; set; }
        public virtual WorkoutPlan WorkoutPlan { get; set; }

        public virtual List<UserExercise> UserExercises { get; set; }

        public virtual CalendarDay CalendarDay { get; set; }
    }
}
