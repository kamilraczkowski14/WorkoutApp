using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class UpdateUserDto
    {
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}",
           ErrorMessage = "The email format is invalid")]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
