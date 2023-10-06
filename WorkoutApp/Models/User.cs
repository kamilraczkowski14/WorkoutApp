using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class User
    {
        public int UserId { get; set; }

        [StringLength(30)]
        [Required]
        public string Username { get; set; }

        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "The email format is invalid")]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        //public WorkoutPlan WorkoutPlanId { get; set; }
        //public Calendar CalendarId { get; set; }

        public virtual List<WorkoutPlan>? WorkoutPlans { get; set; }

        public virtual List<Calendar>? Calendars { get; set; }

    }
}
