using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Models;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Route("api/user")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService service)
        {
            _userService = service;
        }

        [HttpGet("{userId}")]
        public ActionResult<UserDto> Get(int userId)
        {
            var user = _userService.GetUser(userId);

            return Ok(user);

        }

        [HttpPut("{userId}/change")]
        public ActionResult Update([FromRoute] int userId, [FromBody] UpdateUserDto dto)
        {
            _userService.Update(userId, dto);

            return Ok();

        }

        [HttpDelete("{userId}/delete")]
        public ActionResult Delete([FromRoute] int userId)
        {

            _userService.Delete(userId);

            return Ok("Usunales uzytkowika");

        }


        [HttpGet("preferred")]
        public ActionResult<IEnumerable<WorkoutPlanDto>> GetPreferredWorkoutPlans()
        {
            var workoutPlans = _userService.GetPreferredWorkoutPlans();

            return Ok(workoutPlans);
        }

        [HttpGet("getAll")]
        [AllowAnonymous]
        public ActionResult GetAllUsers()
        {
            var users = _userService.GetAll();

            return Ok(users);
        }

    }
}
