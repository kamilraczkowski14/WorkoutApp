using System.ComponentModel.DataAnnotations;

namespace WorkoutApp.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; }

        [Required]
        public int ProductKcal { get; set; }
        public int ProductCategoryId { get; set; }

        [Required]
        public string ProductCategoryName { get; set; }
        public int MealId { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }

        public virtual Meal Meal { get; set; }

    }
}
