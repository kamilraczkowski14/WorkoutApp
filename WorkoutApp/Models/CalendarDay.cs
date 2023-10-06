using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class CalendarDay
    {
        public int CalendarDayId { get; set; }

        [Required]
        public string CalendarDate { get; set; }
        public int CalendarId { get; set; }

        public virtual WorkoutDay WorkoutDay { get; set; }
        public virtual Calendar Calendar { get; set; }
        public virtual List<Meal> Meals { get; set; }
    }
}
