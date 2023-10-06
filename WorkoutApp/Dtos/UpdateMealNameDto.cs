using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class UpdateMealNameDto
    {
        [Required]
        [MaxLength(50)]
        [MinLength(1)]
        public string MealName { get; set; }
    }
}
