using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Models;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{

    [Route("api/workoutplans")]
    [ApiController]
    [Authorize]
    public class WorkoutPlanController : ControllerBase
    {
        private readonly IWorkoutPlanService _wpService;

        public WorkoutPlanController(IWorkoutPlanService wpService)
        {
            _wpService = wpService;
        }

        // Metoda tworzenia WorkoutPlanu dla danego użytkownika
        [HttpPost("create")]
        public ActionResult CreateWorkoutPlan([FromBody] CreateWorkoutPlanDto dto)
        {
            var workoutplanId = _wpService.Create(dto);
            return Created($"/api/workoutplans/{workoutplanId}", null);

        }

        // Metoda usuwania WorkoutPlanu danego użytkownika
        [HttpDelete("{workoutPlanId}/delete")]
        public ActionResult DeleteWorkoutPlan([FromRoute] int workoutPlanId)
        {

            _wpService.Delete(workoutPlanId);
            return Ok("Usunales WorkoutPlan");

        }


        //Metoda zwracajaca wszystkie WorkoutPlany danego uzytkownika
        [HttpGet]
        public ActionResult<IEnumerable<WorkoutPlanDto>> GetAllUserWorkoutPlans()
        {
            var workoutplans = _wpService.GetAllWorkoutPlans();

            return Ok(workoutplans);

        }


        // Metoda zwracająca WorkoutPlan użytkownika na podstawie id
        [HttpGet("{workoutPlanId}")]
        public ActionResult<WorkoutPlanDto> GetWorkoutPlanById([FromRoute] int workoutPlanId)
        {
            var workoutPlan = _wpService.GetById(workoutPlanId);

            return Ok(workoutPlan);

        }

        [HttpPut("{workoutPlanId}/edit")]
        public ActionResult EditWorkoutPlan([FromRoute] int workoutPlanId, [FromBody] UpdateWorkoutPlanDto dto)
        {
            _wpService.Update(workoutPlanId, dto);

            return Ok();


        }




        // Metoda zwracająca wszystkie WorkoutPlany wszystkich uzytkownikow 
        [HttpGet("getAll")]
        [AllowAnonymous]
        public ActionResult GetAllWorkoutPlans()
        {
            var workoutPlans = _wpService.GetAll();

            return Ok(workoutPlans);
        }

    }

}
