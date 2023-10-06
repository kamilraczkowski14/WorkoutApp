using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Dtos
{
    public class LoginDto
    {
        [StringLength(30)]
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
