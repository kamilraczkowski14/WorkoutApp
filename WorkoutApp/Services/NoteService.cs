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
    public interface INoteService
    {
        int Create(int workoutPlanId, CreateNoteDto dto);
        void Delete(int workoutPlanId, int noteId);
        List<NoteDto> GetAllNotes(int workoutPlanId);
        NoteDto GetById(int workoutPlanId, int noteId);
        void Update(int workoutPlanId, int noteId, UpdateNoteDto dto);
        List<Note> GetAll();
    }
    public class NoteService : INoteService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IAuthorizationService _authorization;
        private readonly IUserContextService _userContextService;

        public NoteService(ApplicationDbContext context, IMapper mapper, IAuthorizationService authorization,
            IUserContextService userContextService)
        {
            _context = context;
            _mapper = mapper;
            _authorization = authorization;
            _userContextService = userContextService;
        }

        public int Create(int workoutPlanId, CreateNoteDto dto)
        {

            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var newNote = _mapper.Map<Note>(dto);
            newNote.WorkoutPlanId = workoutPlanId;

            _context.Notes.Add(newNote);
            _context.SaveChanges();

            var noteId = newNote.NoteId;

            return noteId;
        }

        public void Delete(int workoutPlanId, int noteId)
        {

            var note = GetNote(workoutPlanId, noteId);

            _context.Notes.Remove(note);
            _context.SaveChanges();
        }

        public List<Note> GetAll()
        {
            var notes = _context.Notes
                .Include(wp => wp.WorkoutPlan)
                .ToList();

            return notes;
        }

        public List<NoteDto> GetAllNotes(int workoutPlanId)
        {

            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var notesDtos = _mapper.Map<List<NoteDto>>(workoutPlan.Notes);

            return notesDtos;
        }

        public NoteDto GetById(int workoutPlanId, int noteId)
        {

            var note = GetNote(workoutPlanId, noteId);

            var noteDto = _mapper.Map<NoteDto>(note);

            return noteDto;
        }

        public void Update(int workoutPlanId, int noteId, UpdateNoteDto dto)
        {
            var note = GetNote(workoutPlanId, noteId);

            note.NoteName = dto.NoteName;

            note.Text = dto.Text;

            _context.SaveChanges();
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

        private Note GetNote(int workoutPlanId, int noteId)
        {

            var workoutPlan = GetWorkoutPlan(workoutPlanId);

            var note = _context.Notes
                .Include(n => n.WorkoutPlan)
                .FirstOrDefault(n => n.NoteId == noteId);

            if (note == null)
            {
                throw new NotFoundException("Note not found");
            }

            if (note.WorkoutPlanId != workoutPlanId)
            {
                throw new ForbidException("Forbid");
            }

            return note;
        }
    }
}
