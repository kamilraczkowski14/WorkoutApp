using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Route("api/calendars/{calendarId}/calendardays")]
    [ApiController]
    [Authorize]
    public class CalendarDayController : ControllerBase
    {
        private readonly ICalendarDayService _cdService;

        public CalendarDayController(ICalendarDayService cdService)
        {
            _cdService = cdService;
        }

        [HttpPost("create")]
        public ActionResult CreateCalendarDay([FromRoute] int calendarId, [FromBody] CreateCalendarDayDto dto)
        {
            var calendarDayId = _cdService.Create(calendarId, dto);

            return Created($"/api/calendars/{calendarId}/calendardays/{calendarDayId}", null);

        }


        [HttpDelete("{calendarDayId}/delete")]
        public ActionResult DeleteCalendarDay([FromRoute] int calendarId, [FromRoute] int calendarDayId)
        {
            _cdService.Delete(calendarId, calendarDayId);

            return Ok("Usunales Calendar Day");

        }

        [HttpGet]
        public ActionResult<List<CalendarDayDto>> GetAllCalendarDaysC([FromRoute] int calendarId)
        {
            var calendarDays = _cdService.GetAllCalendarDays(calendarId);

            return Ok(calendarDays);

        }

        [HttpGet("{calendarDayId}")]
        public ActionResult<CalendarDayDto> GetCalendarDayById([FromRoute] int calendarId, [FromRoute] int calendarDayId)
        {
            var calendarDay = _cdService.GetById(calendarId, calendarDayId);

            return Ok(calendarDay);

        }



        [HttpGet("GetAll")]
        [AllowAnonymous]
        public ActionResult GetAllCalendarDays()
        {
            var calendarDays = _cdService.GetAll();

            return Ok(calendarDays);

        }
    }
}
