using System.ComponentModel.DataAnnotations;
using WorkoutApp.Models;

namespace WorkoutApp.Dtos
{
    public class UserDto
    {
        public int UserId { get; set; }

        [StringLength(30)]

        public string Username { get; set; }

        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
            ErrorMessage = "The email format is invalid")]

        public string Email { get; set; }


        //public string Password { get; set; }

        //public WorkoutPlan WorkoutPlanId { get; set; }
        //public Calendar CalendarId { get; set; }

        public List<WorkoutPlanDto>? WorkoutPlans { get; set; }
        public List<CalendarDto>? Calendars { get; set; }
    }
}
