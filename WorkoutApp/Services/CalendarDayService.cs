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
    public interface ICalendarDayService
    {
        int Create(int calendarId, CreateCalendarDayDto dto);
        void Delete(int calendarId, int calendarDayId);
        List<CalendarDayDto> GetAllCalendarDays(int calendarId);
        CalendarDayDto GetById(int calendarId, int calendarDayId);
        List<CalendarDay> GetAll();
    }
    public class CalendarDayService : ICalendarDayService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public CalendarDayService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public int Create(int calendarId, CreateCalendarDayDto dto)
        {

            var calendar = GetCalendar(calendarId);

            var newCalendarDay = _mapper.Map<CalendarDay>(dto);

            newCalendarDay.CalendarId = calendarId;

            var date = newCalendarDay.CalendarDate;

            var existingCalendarDay = _context.CalendarDays
              .Where(cd => cd.Calendar.UserId == (int)_userContextService.LoggedUserId && cd.CalendarDate == date)
              .FirstOrDefault();

            if (existingCalendarDay != null)
            {
                throw new BadRequestException("Ta data juz istnieje w twoich kalendarzach");
            }

            _context.CalendarDays.Add(newCalendarDay);
            _context.SaveChanges();

            var calendarDayId = newCalendarDay.CalendarDayId;

            return calendarDayId;
        }

        public void Delete(int calendarId, int calendarDayId)
        {

            var calendarDay = GetCalendarDay(calendarId, calendarDayId);

            _context.CalendarDays.Remove(calendarDay);
            _context.SaveChanges();
        }

        public List<CalendarDay> GetAll()
        {
            var calendarDays = _context.CalendarDays
                .Include(c => c.Calendar)
                .ToList();

            return calendarDays;
        }

        public List<CalendarDayDto> GetAllCalendarDays(int calendarId)
        {

            /*
            if (loggeduserID != user.UserId)
            {
                return Forbid();
            }
            */

            var calendar = GetCalendar(calendarId);

            var calendardaysDtos = _mapper.Map<List<CalendarDayDto>>(calendar.CalendarDays);

            return calendardaysDtos;
        }

        public CalendarDayDto GetById(int calendarId, int calendarDayId)
        {
            var calendarDay = GetCalendarDay(calendarId, calendarDayId);

            var calendarDayDto = _mapper.Map<CalendarDayDto>(calendarDay);

            return calendarDayDto;
        }




        private Calendar GetCalendar(int calendarId)
        {
            var calendar = _context
                .Calendars
                .Include(c => c.CalendarDays)
                .FirstOrDefault(c => c.CalendarId == calendarId);

            if (calendar == null)
            {
                throw new NotFoundException("Calendar not found");
            }

            /*
            if (calendar.UserId != userId)
            {
                throw new ForbidException("Forbid");
            }
            */

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, calendar,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            return calendar;
        }

        private CalendarDay GetCalendarDay(int calendarId, int calendarDayId)
        {

            var calendar = GetCalendar(calendarId);

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
    }
}
