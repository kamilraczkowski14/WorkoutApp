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
    public interface IWorkoutPlanService
    {
        int Create(CreateWorkoutPlanDto dto);
        void Delete(int workoutPlanId);
        List<WorkoutPlanDto> GetAllWorkoutPlans();
        WorkoutPlanDto GetById(int workoutPlanId);
        void Update(int workoutPlanId, UpdateWorkoutPlanDto dto);

        List<WorkoutPlan> GetAll();
    }
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public WorkoutPlanService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public int Create(CreateWorkoutPlanDto dto)
        {

            var newWorkoutPlan = _mapper.Map<WorkoutPlan>(dto);

            newWorkoutPlan.UserId = (int)_userContextService.LoggedUserId;

            _context.WorkoutPlans.Add(newWorkoutPlan);
            _context.SaveChanges();

            return newWorkoutPlan.WorkoutPlanId;
        }

        public void Delete(int workoutPlanId)
        {

            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, workoutPlan,
                new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }


            _context.WorkoutPlans.Remove(workoutPlan);
            _context.SaveChanges();
        }

        public List<WorkoutPlanDto> GetAllWorkoutPlans()
        {
            var loggedUserId = _userContextService.LoggedUserId;

            if (loggedUserId == null)
            {
                throw new ForbidException("User is not logged in.");
            }

            var user = _context
                .Users
                .Include(u => u.WorkoutPlans)
                .FirstOrDefault(u => u.UserId == loggedUserId);

            if (user == null)
            {
                throw new NotFoundException("User not found");
            }


            var workoutplansDtos = _mapper.Map<List<WorkoutPlanDto>>(user.WorkoutPlans);

            return workoutplansDtos;
        }

        public List<WorkoutPlan> GetAll()
        {
            var workoutPlans = _context.WorkoutPlans
                .Include(u => u.User)
                .Include(wd => wd.WorkoutDays)
                .ToList();

            return workoutPlans;
        }

        public WorkoutPlanDto GetById(int workoutPlanId)
        {
            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, workoutPlan,
                new ResourceOperationRequirement(ResourceOperation.Read)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            var workoutPlanDto = _mapper.Map<WorkoutPlanDto>(workoutPlan);

            return workoutPlanDto;
        }

        public void Update(int workoutPlanId, UpdateWorkoutPlanDto dto)
        {
            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var authorizationResult = _authorization.AuthorizeAsync(_userContextService.User, workoutPlan,
             new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException("Forbid");
            }

            workoutPlan.isPreferred = dto.isPreferred;

            _context.SaveChanges();
        }





        private WorkoutPlan GetWorkoutPlan(int workoutPlanId)
        {

            var workoutPlan = _context
                    .WorkoutPlans
                    .Include(wp => wp.WorkoutDays)
                    .Include(wp => wp.User)
                    .Include(wp => wp.Notes)
                    .FirstOrDefault(wp => wp.WorkoutPlanId == workoutPlanId);

            if (workoutPlan == null)
            {
                throw new NotFoundException("Workout Plan not found");
            }

            return workoutPlan;
        }
    }

}
