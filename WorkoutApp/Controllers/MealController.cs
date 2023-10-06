using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Route("api/calendars/{calendarId}/calendardays/{calendarDayId}/meals")]
    [ApiController]
    [Authorize]
    public class MealController : ControllerBase
    {

        private readonly IMealService _mealService;

        public MealController(IMealService mealService)
        {
            _mealService = mealService;
        }

        [HttpPost("create")]
        public ActionResult CreateMeal([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromBody] CreateMealDto dto)
        {
            var mealId = _mealService.Create(calendarId, calendarDayId, dto);

            return Created($"/api/calendars/{calendarId}/calendardays/{calendarDayId}/meals/{mealId}", null);

        }

        [HttpDelete("{mealId}/delete")]
        public ActionResult DeleteMeal([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId)
        {
            _mealService.Delete(calendarId, calendarDayId, mealId);

            return Ok("Usunales Meal");
        }


        [HttpGet]
        public ActionResult<List<MealDto>> GetAllMealsCD([FromRoute] int calendarId, [FromRoute] int calendarDayId)
        {

            var meals = _mealService.GetAllMeals(calendarId, calendarDayId);

            return Ok(meals);

        }

        [HttpGet("{mealId}")]
        public ActionResult<MealDto> GetMealById([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId)
        {
            var meal = _mealService.GetById(calendarId, calendarDayId, mealId);

            return Ok(meal);

        }



        [HttpPut("{mealId}/edit")]
        public ActionResult UpdateMeal([FromRoute] int calendarId, [FromRoute] int calendarDayId, [FromRoute] int mealId,
            [FromBody] UpdateMealNameDto dto)
        {
            _mealService.Update(calendarId, calendarDayId, mealId, dto);

            return Ok();
        }




        [HttpGet("GetAll")]
        [AllowAnonymous]
        public ActionResult GetAllMeals()
        {
            var meals = _mealService.GetAll();

            return Ok(meals);
        }

    }
}
