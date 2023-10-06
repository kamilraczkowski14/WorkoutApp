using AutoMapper;
using WorkoutApp.Dtos;
using WorkoutApp.Models;

namespace WorkoutApp.Tools
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDto>();

            CreateMap<WorkoutPlan, WorkoutPlanDto>();

            CreateMap<WorkoutPlanDto, WorkoutPlan>();

            CreateMap<CreateWorkoutPlanDto, WorkoutPlan>();

            CreateMap<WorkoutDay, WorkoutDayDto>();

            CreateMap<CreateWorkoutDayDto, WorkoutDay>();

            CreateMap<UserExercise, UserExerciseDto>();

            CreateMap<CreateExerciseDto, UserExercise>();

            CreateMap<Note, NoteDto>();

            CreateMap<CreateNoteDto, Note>();

            CreateMap<Calendar, CalendarDto>();

            CreateMap<CreateCalendarDto, Calendar>();

            CreateMap<CreateCalendarDayDto, CalendarDay>();

            CreateMap<CalendarDay, CalendarDayDto>();

            CreateMap<Meal, MealDto>();

            CreateMap<CreateMealDto, Meal>();

            CreateMap<Product, ProductDto>();

            CreateMap<CreateProductDto, Product>();
        }
    }
}
