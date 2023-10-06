using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using WorkoutApp.Authorization;
using WorkoutApp.DAL;
using WorkoutApp.Dtos;
using WorkoutApp.Exceptions;
using WorkoutApp.Models;

namespace WorkoutApp.Services
{
    public interface IWorkoutDayService
    {
        int Create(int workoutPlanId, CreateWorkoutDayDto dto);
        void Delete(int workoutPlanId, int workoutDayId);
        List<WorkoutDayDto> GetAllWorkoutDays(int workoutPlanId);
        WorkoutDayDto GetById(int workoutPlanId, int workoutDayId);
        List<WorkoutDay> GetAll();
    }
    public class WorkoutDayService : IWorkoutDayService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public WorkoutDayService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public int Create(int workoutPlanId, CreateWorkoutDayDto dto)
        {

            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var newWorkoutDay = _mapper.Map<WorkoutDay>(dto);
            newWorkoutDay.WorkoutPlanId = workoutPlanId;

            var date = newWorkoutDay.CalendarDate;

            var existingWorkoutDay = _context.WorkoutDays
                .Where(wd => wd.WorkoutPlan.UserId == (int)_userContextService.LoggedUserId && wd.CalendarDate == date)
                .FirstOrDefault();


            if (existingWorkoutDay != null)
            {
                throw new BadRequestException("Dzien o tej dacie juz istnieje w twoich WorkoutPlanach");
            }

            var existingCalendarDay = _context.CalendarDays
              .Where(cd => cd.Calendar.UserId == (int)_userContextService.LoggedUserId && cd.CalendarDate == date)
              .FirstOrDefault();

            if (existingCalendarDay == null)
            {
                throw new BadRequestException("Nie ma takiego utworzonego dnia");
            }

            newWorkoutDay.CalendarDayId = existingCalendarDay.CalendarDayId;

            _context.WorkoutDays.Add(newWorkoutDay);
            _context.SaveChanges();

            return newWorkoutDay.WorkoutDayId;
        }

        public void Delete(int workoutPlanId, int workoutDayId)
        {
            var workoutDay = GetWorkoutDay(workoutPlanId, workoutDayId);

            _context.WorkoutDays.Remove(workoutDay);
            _context.SaveChanges();
        }

        public List<WorkoutDay> GetAll()
        {
            var workoutDays = _context.WorkoutDays
                .Include(wp => wp.WorkoutPlan)
                .ToList();

            return workoutDays;
        }

        public List<WorkoutDayDto> GetAllWorkoutDays(int workoutPlanId)
        {

            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var workoutdaysDtos = _mapper.Map<List<WorkoutDayDto>>(workoutPlan.WorkoutDays);

            return workoutdaysDtos;
        }

        public WorkoutDayDto GetById(int workoutPlanId, int workoutDayId)
        {

            var workoutDay = GetWorkoutDay(workoutPlanId, workoutDayId);

            var workoutDayDto = _mapper.Map<WorkoutDayDto>(workoutDay);

            return workoutDayDto;
        }


        private WorkoutPlan GetWorkoutPlan(int workoutPlanId)
        {
            var workoutPlan = _context.WorkoutPlans
          .Include(wp => wp.User)
          .Include(wp => wp.WorkoutDays)
          .FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);


            if (workoutPlan == null)
            {
                throw new NotFoundException("Workout plan not found");
            }

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, workoutPlan,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            return workoutPlan;
        }

        private WorkoutDay GetWorkoutDay(int workoutPlanId, int workoutDayId)
        {
            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var workoutDay = _context.WorkoutDays
                .Include(wd => wd.WorkoutPlan)
                .Include(wd => wd.UserExercises)
                .FirstOrDefault(wd => wd.WorkoutDayId == workoutDayId);

            if (workoutDay == null)
            {
                throw new NotFoundException("Workout day not found");
            }

            if (workoutDay.WorkoutPlanId != workoutPlanId)
            {
                throw new ForbidException("Forbid");
            }

            return workoutDay;
        }
    }
}
