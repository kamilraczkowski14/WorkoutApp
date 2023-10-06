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
    public interface ICalendarService
    {
        int Create(CreateCalendarDto dto);
        void Delete(int calendarId);
        List<CalendarDto> GetAllCalendars();
        CalendarDto GetById(int calendarId);
        List<Calendar> GetAll();
    }
    public class CalendarService : ICalendarService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public CalendarService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public int Create(CreateCalendarDto dto)
        {

            var newCalendar = _mapper.Map<Calendar>(dto);

            newCalendar.UserId = (int)_userContextService.LoggedUserId;

            _context.Calendars.Add(newCalendar);
            _context.SaveChanges();

            var calendarId = newCalendar.CalendarId;

            return calendarId;
        }

        public void Delete(int calendarId)
        {

            var calendar = GetCalendar(calendarId);

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, calendar,
               new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            _context.Calendars.Remove(calendar);
            _context.SaveChanges();

        }

        public List<Calendar> GetAll()
        {
            var calendars = _context.Calendars
                .Include(u => u.User)
                .ToList();

            return calendars;
        }

        public List<CalendarDto> GetAllCalendars()
        {
            var loggedUserId = _userContextService.LoggedUserId;

            if (loggedUserId == null)
            {
                throw new ForbidException("User is not logged in.");
            }

            var user = _context
                .Users
                .Include(u => u.Calendars)
                .FirstOrDefault(u => u.UserId == loggedUserId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }

            var calendarsDtos = _mapper.Map<List<CalendarDto>>(user.Calendars);

            return calendarsDtos;
        }

        public CalendarDto GetById(int calendarId)
        {

            var calendar = GetCalendar(calendarId);

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, calendar,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            var calendarDto = _mapper.Map<CalendarDto>(calendar);

            return calendarDto;
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

            return calendar;
        }
    }
}
