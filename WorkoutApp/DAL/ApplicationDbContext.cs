using Microsoft.EntityFrameworkCore;
using WorkoutApp.Models;

namespace WorkoutApp.DAL
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<WorkoutPlan> WorkoutPlans { get; set; }
        public DbSet<WorkoutDay> WorkoutDays { get; set; }
        public DbSet<Exercise> Exercises { get; set; }
        public DbSet<UserExercise> UserExercises { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<Calendar> Calendars { get; set; }
        public DbSet<CalendarDay> CalendarDays { get; set; }
        public DbSet<Meal> Meals { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductCategory> ProductCategories { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<WorkoutDay>()
            .HasOne(wd => wd.WorkoutPlan)
            .WithMany(wp => wp.WorkoutDays)
            .HasForeignKey(wd => wd.WorkoutPlanId)
            .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Exercise>().HasData(

               new Exercise()
               {
                   ExerciseId = 1,
                   ExerciseName = "Podciąganie na drążku",
                   Description = "Podciąganie na drążku to ćwiczenie wykonywane przy użyciu wagi ciała, polegające na chwyceniu drążka nad głową, " +
                   "dłonie skierowane na zewnątrz. Poprzez unoszenie ciała ku górze skupiasz się na mięśniach górnej części pleców, szczególnie na mięśniu czworobocznym. " +
                   "Ćwiczenie to również angażuje bicepsy i mięśnie brzucha, zapewniając wszechstronny trening górnej części ciała.",
                   BodyPart = "Plecy"
               },
                new Exercise()
                {
                    ExerciseId = 2,
                    ExerciseName = "Wyciskanie sztangi",
                    Description = "Wyciskanie sztangi to ruch wykonywany przy użyciu ciężarów. Sztanga startuje na kołkach bezpieczeństwa, " +
                    "co dodaje wyzwań związanych z bezruchem. Unoszenie i wyciskanie wzmacnia mięśnie klatki piersiowej, tricepsów i ramion. " +
                    "Ćwiczenie to koncentruje się na osłabionych punktach w wyciskaniu sztangi.",
                    BodyPart = "Klatka piersiowa"
                },
                new Exercise()
                {
                    ExerciseId = 3,
                    ExerciseName = "Podciąganie liny do twarzy",
                    Description = "Podciąganie liny do twarzy to ćwiczenie wykonywane przy użyciu maszyny z linką i końcówką w kształcie liny. " +
                    "Pociągnij linkę w kierunku twarzy, trzymając łokcie wysoko, aby zaangażować mięśnie naramienne tyłowe oraz górną część pleców. " +
                    "Ćwiczenie to poprawia stabilność ramion i postawę ciała.",
                    BodyPart = "Barki"
                },
                new Exercise()
                {
                    ExerciseId = 4,
                    ExerciseName = "Skull crusher i pullover",
                    Description = "Połączenie ćwiczeń Skull Crusher i Pullover. Zacznij od wyciągnięcia sztangi na wyciskanie nad czoło (skull crusher), " +
                    "a następnie przenieś ją nad głowę (pullover). Zaangażowana zostaje trójgłowa część ramienia, klatka piersiowa i mięśnie pleców, " +
                    "zapewniając kompleksowy trening górnej części ciała.",
                    BodyPart = "Mięsień trójgłowy ramienia"
                },
                new Exercise()
                {
                    ExerciseId = 5,
                    ExerciseName = "Uginanie ramion ze sztangą",
                    Description = "Uginanie ramion ze sztangą to klasyczne ćwiczenie wzmacniające mięśnie ramion. Trzymaj sztangę podchwytem, " +
                    "dłonie skierowane do góry, i unosząc ją ku górze, wykorzystaj mięśnie bicepasa. Opuszczaj sztangę z kontrolą. " +
                    "Ćwiczenie to koncentruje się na mięśniach bicepsa, pomagając rozwijać ich definicję i siłę.",
                    BodyPart = "Biceps"
                },
                new Exercise()
                {
                    ExerciseId = 6,
                    ExerciseName = "Plank na piłce szwajcarskiej",
                    Description = "Plank na piłce szwajcarskiej to efektywne ćwiczenie dla mięśni brzucha. Rozpocznij w pozycji plank na przedramionach, " +
                    "piłka szwajcarska pod brzuchem, a palce stóp na podłodze. Utrzymuj prostą linię od głowy do pięt, angażując mięśnie brzucha i stabilizujące. " +
                    "Ćwiczenie to pomaga poprawić siłę, stabilność i równowagę mięśni brzucha.",
                    BodyPart = "Brzuch"
                },
                new Exercise()
                {
                    ExerciseId = 7,
                    ExerciseName = "Podnoszenie pięt ze sztangą w staniu",
                    Description = "Podnoszenie pięt ze sztangą w staniu to ćwiczenie wzmacniające łydki. Stań prosto, ze sztangą opierającą się na górnej części pleców." +
                    " Wstawaj na palce poprzez unoszenie pięt i przesuwanie wagi ciała na przednią część stóp. Opuszczaj pięty z kontrolą, wykonując pełen zakres ruchu." +
                    " Ćwiczenie to koncentruje się na mięśniach łydek, poprawiając ich siłę i wyrazistość.",
                    BodyPart = "Łydki"
                },
                new Exercise()
                {
                    ExerciseId = 8,
                    ExerciseName = "Wypady z sztangą nad plecami",
                    Description = "Wypady z sztangą nad plecami to ćwiczenie dolnej partii ciała. Trzymając sztangę na górnej części pleców," +
                    " cofnij się jedną nogą do wypadu, obniżając kolano tylnej nogi w kierunku podłoża. Napnij przednią nogę, aby wrócić do pozycji wyjściowej. " +
                    "Ćwiczenie to angażuje mięśnie czworogłowe uda, mięśnie dwugłowe uda i pośladki, poprawiając siłę i stabilność nóg.",
                    BodyPart = "Czworogłowe uda"
                },
                new Exercise()
                {
                    ExerciseId = 9,
                    ExerciseName = "Martwy ciąg na podwyższeniu",
                    Description = "Martwy ciąg na podwyższeniu to ćwiczenie z ciężarami, polegające na podnoszeniu sztangi z podwyższenia. " +
                    "Stań na podkładkach lub blokach, chwytając sztangę podchwytem, i unosząc ją poprzez wyprostowanie bioder i kolan. " +
                    "Opuszczaj sztangę z kontrolą. Ćwiczenie to angażuje mięśnie dwugłowe uda, pośladki, dolny odcinek pleców i mięśnie brzucha, " +
                    "wspierając ogólną siłę i rozwój mięśniowy.",
                    BodyPart = "Dolny odcinek pleców"
                },
                new Exercise()
                {
                    ExerciseId = 10,
                    ExerciseName = "Uginanie ramion z hantlami w supinacji",
                    Description = "Uginanie ramion z hantlami w supinacji to ćwiczenie wzmacniające mięśnie bicepsa. " +
                    "Trzymaj hantle w dłoniach ze zwiniętymi do góry dłońmi (supinacja) i unieś je ku górze, skręcając bicepsy." +
                    " Opuszczaj hantle z kontrolą. Ćwiczenie to efektywnie izoluje i rozwija mięśnie bicepsa, poprawiając siłę i definicję ramion.",
                    BodyPart = "Biceps"
                });



            modelBuilder.Entity<ProductCategory>().HasData(
                new ProductCategory()
                {
                    ProductCategoryId = 1,
                    ProductCategoryName = "Owoce"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 2,
                    ProductCategoryName = "Warzywa"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 3,
                    ProductCategoryName = "Mięso"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 4,
                    ProductCategoryName = "Nabiał"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 5,
                    ProductCategoryName = "Pieczywo"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 6,
                    ProductCategoryName = "Produkty zbożowe"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 7,
                    ProductCategoryName = "Ryby i owoce morza"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 8,
                    ProductCategoryName = "Słodycze"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 9,
                    ProductCategoryName = "Napoje"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 10,
                    ProductCategoryName = "Sosy i przyprawy"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 11,
                    ProductCategoryName = "Przekąski"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 12,
                    ProductCategoryName = "Zupy"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 13,
                    ProductCategoryName = "Napoje alkoholowe"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 14,
                    ProductCategoryName = "Produkty bezglutenowe"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 15,
                    ProductCategoryName = "Kawy i herbaty"
                },
                new ProductCategory()
                {
                    ProductCategoryId = 16,
                    ProductCategoryName = "Inne"
                });

        }



        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionString);
        }


    }

}
