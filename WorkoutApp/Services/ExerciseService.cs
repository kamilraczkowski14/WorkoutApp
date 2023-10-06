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
    public interface IExerciseService
    {
        int Create(int workoutPlanId, int workoutDayId, CreateExerciseDto dto);
        void Delete(int workoutPlanId, int workoutDayId, int userExerciseId);
        List<UserExerciseDto> GetAllExercises(int workoutPlanId, int workoutDayId);
        void Update(int workoutPlanId, int workoutDayId, int userExerciseId, UpdateExerciseNumbersDto dto);
        List<Exercise> GetSamples();
        void AddToUserExercise(int workoutPlanId, int workoutDayId, int exerciseId);
        UserExerciseDto GetById(int workoutPlanId, int workoutDayId, int userExerciseId);
        List<UserExercise> GetAll();
    }
    public class ExerciseService : IExerciseService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public ExerciseService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public void AddToUserExercise(int workoutPlanId, int workoutDayId, int exerciseId)
        {

            var workoutDay = GetWorkoutDay(workoutPlanId, workoutDayId);

            var exerciseToAdd = _context.Exercises
                                    .FirstOrDefault(e => e.ExerciseId == exerciseId);

            if (exerciseToAdd == null)
            {
                throw new NotFoundException("Exercise not found");
            }

            var userExercise = new UserExercise
            {
                ExerciseName = exerciseToAdd.ExerciseName,
                Description = exerciseToAdd.Description,
                BodyPart = exerciseToAdd.BodyPart,
                NumberOfSeries = exerciseToAdd.NumberOfSeries,
                NumberOfRepeats = exerciseToAdd.NumberOfRepeats,
                WorkoutDayId = workoutDayId
            };

            _context.UserExercises.Add(userExercise);
            _context.SaveChanges();

        }

        public int Create(int workoutPlanId, int workoutDayId, CreateExerciseDto dto)
        {

            var workoutDay = GetWorkoutDay(workoutPlanId, workoutDayId);

            var newExercise = _mapper.Map<UserExercise>(dto);
            newExercise.WorkoutDayId = workoutDayId;

            _context.UserExercises.Add(newExercise);
            _context.SaveChanges();

            var exerciseId = newExercise.UserExerciseId;

            return exerciseId;
        }

        public void Delete(int workoutPlanId, int workoutDayId, int userExerciseId)
        {

            var exercise = GetUserExercise(workoutPlanId, workoutDayId, userExerciseId);

            _context.UserExercises.Remove(exercise);
            _context.SaveChanges();
        }


        public List<UserExercise> GetAll()
        {
            var exercises = _context.UserExercises
            .Include(e => e.WorkoutDay)
            .ToList();

            return exercises;
        }

        public List<UserExerciseDto> GetAllExercises(int workoutPlanId, int workoutDayId)
        {

            var workoutDay = GetWorkoutDay(workoutPlanId, workoutDayId);

            var exercisesDtos = _mapper.Map<List<UserExerciseDto>>(workoutDay.UserExercises);

            return exercisesDtos;
        }

        public UserExerciseDto GetById(int workoutPlanId, int workoutDayId, int userExerciseId)
        {

            var exercise = GetUserExercise(workoutPlanId, workoutDayId, userExerciseId);

            var exerciseDto = _mapper.Map<UserExerciseDto>(exercise);

            return exerciseDto;
        }

        public List<Exercise> GetSamples()
        {
            var exercises = _context
            .Exercises
            .ToList();

            return exercises;
        }

        public void Update(int workoutPlanId, int workoutDayId, int userExerciseId, UpdateExerciseNumbersDto dto)
        {
            var exercise = GetUserExercise(workoutPlanId, workoutDayId, userExerciseId);

            exercise.NumberOfSeries = dto.NumberOfSeries;
            if (exercise.NumberOfSeries < 1)
            {
                throw new BadRequestException("Licza serii musi wynosic przynajmniej 1");
            }

            exercise.NumberOfRepeats = dto.NumberOfRepeats;
            if (exercise.NumberOfRepeats < 1)
            {
                throw new BadRequestException("Licza powtorzen musi wynosic przynajmniej 1");
            }

            _context.SaveChanges();
        }



        private WorkoutDay GetWorkoutDay(int workoutPlanId, int workoutDayId)
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

        private UserExercise GetUserExercise(int workoutPlanId, int workoutDayId, int userExerciseId)
        {
            var workoutDay = GetWorkoutDay(workoutPlanId, workoutDayId);

            var exercise = _context.UserExercises
                .Include(e => e.WorkoutDay)
                .FirstOrDefault(e => e.UserExerciseId == userExerciseId);

            if (exercise == null)
            {
                throw new NotFoundException("Exercise not found");
            }

            if (exercise.WorkoutDayId != workoutDayId)
            {
                throw new ForbidException("Forbid");
            }

            return exercise;
        }

    }



}


