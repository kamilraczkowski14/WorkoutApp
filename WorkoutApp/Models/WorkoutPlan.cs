using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class WorkoutPlan
    {
        public int WorkoutPlanId { get; set; }

        [Required]
        public string? Name { get; set; }
        public int UserId { get; set; }

        public bool? isPreferred { get; set; } = false;

        public virtual User User { get; set; }

        public virtual List<WorkoutDay> WorkoutDays { get; set; }

        public virtual List<Note> Notes { get; set; }


    }
}
