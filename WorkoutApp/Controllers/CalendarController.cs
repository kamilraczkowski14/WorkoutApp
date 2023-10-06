using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Models;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Route("api/calendars")]
    [ApiController]
    [Authorize]
    public class CalendarController : ControllerBase
    {
        private readonly ICalendarService _calendarService;

        public CalendarController(ICalendarService calendarService)
        {
            _calendarService = calendarService;
        }

        [HttpPost("create")]
        public ActionResult CreateCalendar([FromBody] CreateCalendarDto dto)
        {
            var calendarId = _calendarService.Create(dto);
            return Created($"/api/calendars/{calendarId}", null);

        }


        [HttpDelete("{calendarId}/delete")]
        public ActionResult DeleteCalendar([FromRoute] int calendarId)
        {
            _calendarService.Delete(calendarId);

            return Ok("Usunales kalendarz");

        }

        [HttpGet]
        public ActionResult<IEnumerable<CalendarDto>> GetAllUserCalendars()
        {
            var calendars = _calendarService.GetAllCalendars();

            return Ok(calendars);

        }

        [HttpGet("{calendarId}")]
        public ActionResult<CalendarDto> GetCalendarById([FromRoute] int calendarId)
        {
            var calendar = _calendarService.GetById(calendarId);

            return Ok(calendar);

        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        public ActionResult<List<Calendar>> GetAllCalendars()
        {
            var calendars = _calendarService.GetAll();

            return Ok(calendars);
        }

    }
}
