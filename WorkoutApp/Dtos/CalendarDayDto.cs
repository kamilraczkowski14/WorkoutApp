using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class CalendarDayDto
    {
        public int CalendarDayId { get; set; }

        [Required]
        public string CalendarDate { get; set; }

        public virtual WorkoutDayDto WorkoutDay { get; set; }

        public virtual List<MealDto> Meals { get; set; }
    }
}
