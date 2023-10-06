using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class NoteDto
    {
        public int NoteId { get; set; }
        public string NoteName { get; set; }

        [Required]
        public string Text { get; set; }
    }
}
