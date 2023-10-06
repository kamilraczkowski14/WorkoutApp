using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Route("api/workoutplans/{workoutPlanId}/workoutdays")]
    [ApiController]
    [Authorize]
    public class WorkoutDayController : ControllerBase
    {
        private readonly IWorkoutDayService _wdService;

        public WorkoutDayController(IWorkoutDayService wdService)
        {
            _wdService = wdService;
        }

        // Metoda tworzenia WorkoutDaya dla danego WorkoutPlanu
        [HttpPost("create")]
        public ActionResult CreateWorkoutDay([FromRoute] int workoutPlanId, [FromBody] CreateWorkoutDayDto dto)
        {

            var workoutdayId = _wdService.Create(workoutPlanId, dto);


            return Created($"api/workoutplans/{workoutPlanId}/workoutdays/{workoutdayId}", null);

        }


        // Metoda usuwania WorkoutDaya dla danego WorkoutPlanu
        [HttpDelete("{workoutDayId}/delete")]
        public ActionResult DeleteWorkoutDay([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
        {
            _wdService.Delete(workoutPlanId, workoutDayId);

            return Ok("Usunales WorkoutDay");

        }

        //metoda zwracajaca wszystkie WorkoutDay danego WorkoutPlan
        [HttpGet]
        public ActionResult<List<WorkoutDayDto>> GetAllWorkoutDaysWP([FromRoute] int workoutPlanId)
        {
            var workoutDays = _wdService.GetAllWorkoutDays(workoutPlanId);

            return Ok(workoutDays);

        }


        // Metoda zwracająca WorkoutDay na podstawie id dla WorkoutPlanu
        [HttpGet("{workoutDayId}")]
        public ActionResult<WorkoutDayDto> GetWorkoutDayById([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
        {
            var workoutDay = _wdService.GetById(workoutPlanId, workoutDayId);

            return Ok(workoutDay);
        }


        // Metoda zwracająca wszystkie WorkoutDay wszystkich WorkoutPlan
        [HttpGet("GetAll")]
        [AllowAnonymous]
        public ActionResult GetAllWorkoutDays()
        {
            var workoutDays = GetAllWorkoutDays();

            return Ok(workoutDays);

        }
    }
}
