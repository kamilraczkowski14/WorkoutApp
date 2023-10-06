
using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class Note
    {
        public int NoteId { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(20)]
        public string NoteName { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(500)]
        public string Text { get; set; }
        public int WorkoutPlanId { get; set; }
        public virtual WorkoutPlan WorkoutPlan { get; set; }

    }
}
