using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class CreateMealDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string MealName { get; set; }

        public int CalendarDayId { get; set; }
    }
}
