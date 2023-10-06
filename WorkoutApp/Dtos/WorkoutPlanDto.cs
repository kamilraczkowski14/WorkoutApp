using WorkoutApp.Dtos;

namespace WorkoutApp.Models
{
    public class WorkoutPlanDto
    {
        public int WorkoutPlanId { get; set; }
        public string Name { get; set; }

        public bool? isPreferred { get; set; } = false;
        public virtual List<WorkoutDayDto> WorkoutDays { get; set; }

        public virtual List<NoteDto> Notes { get; set; }

    }
}
