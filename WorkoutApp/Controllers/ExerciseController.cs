using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Route("api/workoutplans/{workoutPlanId}/workoutdays/{workoutDayId}/exercises")]
    [ApiController]
    [Authorize]
    public class ExerciseController : ControllerBase
    {
        private readonly IExerciseService _eService;

        public ExerciseController(IExerciseService eService)
        {
            _eService = eService;
        }

        [HttpPost("create")]
        public ActionResult CreateExercise([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromBody] CreateExerciseDto dto)
        {

            var exerciseId = _eService.Create(workoutPlanId, workoutDayId, dto);

            return Created($"api/workoutplans/{workoutPlanId}/workoutdays/{workoutDayId}/exercises/{exerciseId}", null);

        }


        [HttpPost("{userExerciseId}/delete")]
        public ActionResult DeleteExercise([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int userExerciseId)
        {

            _eService.Delete(workoutPlanId, workoutDayId, userExerciseId);
            return Ok("Usunales Exercise");

        }

        [HttpGet]
        public ActionResult<List<UserExerciseDto>> GetAllExercises([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId)
        {
            var exercises = _eService.GetAllExercises(workoutPlanId, workoutDayId);

            return Ok(exercises);

        }

        [HttpPut("{userExerciseId}/editNumbers")]
        public ActionResult EditNumbers([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int userExerciseId, [FromBody] UpdateExerciseNumbersDto dto)
        {
            _eService.Update(workoutPlanId, workoutDayId, userExerciseId, dto);
            return Ok();
        }

        [HttpGet("getsamples")]
        public ActionResult GetReadyExercises()
        {
            var exercises = _eService.GetSamples();

            return Ok(exercises);
        }


        [HttpPost("getsamples/{exerciseId}")]
        public ActionResult AddToUserExercises([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int exerciseId)
        {
            _eService.AddToUserExercise(workoutPlanId, workoutDayId, exerciseId);

            return Ok("Dodales cwiczenie");

        }



        [HttpGet("{userExerciseId}")]
        public ActionResult<UserExerciseDto> GetExerciseById([FromRoute] int workoutPlanId, [FromRoute] int workoutDayId, [FromRoute] int userExerciseId)
        {
            var exercise = _eService.GetById(workoutPlanId, workoutDayId, userExerciseId);

            return Ok(exercise);
        }



        [HttpGet("getAll")]
        [AllowAnonymous]
        public ActionResult GetAllUserExercise()
        {
            var exercises = _eService.GetAll();

            return Ok(exercises);
        }
    }
}
