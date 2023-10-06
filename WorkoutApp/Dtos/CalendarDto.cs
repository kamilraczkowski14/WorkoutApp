namespace WorkoutApp.Dtos
{
    public class CalendarDto
    {
        public int CalendarId { get; set; }

        public virtual List<CalendarDayDto> CalendarDays { get; set; }
    }
}
