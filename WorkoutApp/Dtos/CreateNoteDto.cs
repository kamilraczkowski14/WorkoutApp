using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class CreateNoteDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string NoteName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Text { get; set; }
        public int WorkoutPlanId { get; set; }
    }
}
