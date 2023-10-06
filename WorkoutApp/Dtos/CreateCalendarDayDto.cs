using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class CreateCalendarDayDto
    {
        public int CalendarId { get; set; }

        [Required]
        public string CalendarDate { get; set; }

    }
}
