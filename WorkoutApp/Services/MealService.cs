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
    public interface IMealService
    {
        int Create(int calendarId, int calendarDayId, CreateMealDto dto);
        void Delete(int calendarId, int calendarDayId, int mealId);
        List<MealDto> GetAllMeals(int calendarId, int calendarDayId);
        MealDto GetById(int calendarId, int calendarDayId, int mealId);
        void Update(int calendarId, int calendarDayId, int mealId, UpdateMealNameDto dto);
        List<Meal> GetAll();
    }
    public class MealService : IMealService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public MealService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;

        }

        public int Create(int calendarId, int calendarDayId, CreateMealDto dto)
        {

            var calendarDay = GetCalendarDay(calendarId, calendarDayId);


            var newMeal = _mapper.Map<Meal>(dto);


            newMeal.CalendarDayId = calendarDayId;

            _context.Meals.Add(newMeal);
            _context.SaveChanges();

            var mealId = newMeal.MealId;

            return mealId;
        }

        public void Delete(int calendarId, int calendarDayId, int mealId)
        {

            var meal = GetMeal(calendarDayId, mealId);

            _context.Meals.Remove(meal);
            _context.SaveChanges();

        }

        public List<Meal> GetAll()
        {
            var meals = _context.Meals
               .Include(m => m.CalendarDay)
               .ToList();

            return meals;
        }

        public List<MealDto> GetAllMeals(int calendarId, int calendarDayId)
        {

            var calendarDay = GetCalendarDay(calendarId, calendarDayId);

            var mealsDtos = _mapper.Map<List<MealDto>>(calendarDay.Meals);

            return mealsDtos;
        }

        public MealDto GetById(int calendarId, int calendarDayId, int mealId)
        {

            var meal = GetMeal(calendarDayId, mealId);

            var mealDto = _mapper.Map<MealDto>(meal);

            return mealDto;
        }

        public void Update(int calendarId, int calendarDayId, int mealId, UpdateMealNameDto dto)
        {

            var meal = GetMeal(calendarDayId, mealId);

            meal.MealName = dto.MealName;

            _context.SaveChanges();

        }


        private CalendarDay GetCalendarDay(int calendarId, int calendarDayId)
        {

            var calendar = _context
               .Calendars
               .Include(c => c.CalendarDays)
               .FirstOrDefault(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                throw new NotFoundException("Calendar not found");
            }

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, calendar,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            var calendarDay = _context.CalendarDays
                .FirstOrDefault(c => c.CalendarDayId == calendarDayId);

            if (calendarDay == null)
            {
                throw new NotFoundException("Calendar day not found");
            }

            if (calendarDay.CalendarId != calendarId)
            {
                throw new ForbidException("Forbid");
            }

            return calendarDay;

        }

        private Meal GetMeal(int calendarDayId, int mealId)
        {
            var calendarDay = GetCalendarDay(calendarDayId, mealId);

            var meal = _context.Meals
                .Include(m => m.CalendarDay)
                .Include(m => m.Products)
                .FirstOrDefault(m => m.MealId == mealId);

            if (meal == null)
            {
                throw new NotFoundException("Meal not found");
            }

            if (meal.CalendarDayId != calendarDayId)
            {
                throw new ForbidException("Forbid");
            }

            return meal;
        }
    }
}
