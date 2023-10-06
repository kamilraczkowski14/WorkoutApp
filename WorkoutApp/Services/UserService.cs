using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Authorization;
using WorkoutApp.DAL;
using WorkoutApp.Dtos;
using WorkoutApp.Exceptions;
using WorkoutApp.Models;
using WorkoutApp.Tools;

namespace WorkoutApp.Services
{
    public interface IUserService
    {
        UserDto GetUser(int id);
        public void Delete(int userId);
        public void Update(int userId, UpdateUserDto dto);
        public List<User> GetAll();
        public List<WorkoutPlanDto> GetPreferredWorkoutPlans();

    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public UserService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public void Delete(int userId)
        {

            var user = _context
                .Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var authorizationResult = _authorization.AuthorizeAsync(
                _userContextService.User, user, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            _context.Users.Remove(user);
            _context.SaveChanges();


        }


        public void Update(int userId, UpdateUserDto dto)
        {

            var user = _context
                .Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var authorizationResult = _authorization.AuthorizeAsync(
                _userContextService.User, user, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }


            user.Email = dto.Email;
            var existingEmail = _context.Users.FirstOrDefault(u => u.Email == user.Email);
            //sprawdzenie czy uzytkownik o podanym emailu juz istnieje
            if (existingEmail != null)
            {
                throw new BadRequestException("Uzytkownik o podanym adresie email juz istnieje.");
            }

            user.Password = dto.Password;
            if (user.Password.Length < 3)
            {
                throw new BadRequestException("Haslo musi zawierac co najmniej 3 znaki.");
            }

            //walidacja hasla
            user.Password = Password.hashPassword(user.Password);


            _context.SaveChanges();

        }

        public List<User> GetAll()
        {
            var users = _context.Users
            .Include(wp => wp.WorkoutPlans)
            .Include(u => u.Calendars)
            .ToList();

            return users;
        }

        public UserDto GetUser(int userId)
        {
            var user = _context
                .Users
                .FirstOrDefault(u => u.UserId == userId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var authorizationResult = _authorization.AuthorizeAsync(
                _userContextService.User, user, new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            var userDto = _mapper.Map<UserDto>(user);

            return userDto;
        }


        public List<WorkoutPlanDto> GetPreferredWorkoutPlans()
        {

            var loggedUserId = _userContextService.LoggedUserId;

            if (loggedUserId == null)
            {
                throw new ForbidException("User is not logged in.");
            }

            var user = _context
                .Users
                .FirstOrDefault(u => u.UserId == loggedUserId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }


            var workoutPlan = _context
               .WorkoutPlans
               .Where(wp => wp.isPreferred == true && wp.UserId == user.UserId)
               .FirstOrDefault();

            if (workoutPlan == null)
            {
                throw new NotFoundException("Workout plan not found");
            }

            var workoutplansDtos = _mapper.Map<List<WorkoutPlanDto>>(workoutPlan);

            return workoutplansDtos;

        }

    }
}
