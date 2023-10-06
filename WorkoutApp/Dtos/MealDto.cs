namespace WorkoutApp.Dtos
{
    public class MealDto
    {
        public int MealId { get; set; }
        public string MealName { get; set; }
        public int TotalKcal { get; set; } = 0;

        public virtual List<ProductDto> Products { get; set; }

    }
}
