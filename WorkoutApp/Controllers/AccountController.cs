using Microsoft.AspNetCore.Mvc;
using WorkoutApp.Dtos;
using WorkoutApp.Services;

namespace WorkoutApp.Controllers
{
    [Route("api")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        [HttpPost("register")]
        public ActionResult Register([FromBody] RegisterDto dto)
        {
            _accountService.Register(dto);
            return Created("/api/login", null);
        }

        [HttpPost("login")]
        public ActionResult Login([FromBody] LoginDto dto)
        {
            string token = _accountService.Login(dto);

            return Ok(token);
        }



    }
}
