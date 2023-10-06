namespace WorkoutApp.Dtos
{
    public class WorkoutDayDto
    {
        public int WorkoutDayId { get; set; }
        public string CalendarDate { get; set; }
        public virtual List<UserExerciseDto> UserExercises { get; set; }
    }
}
