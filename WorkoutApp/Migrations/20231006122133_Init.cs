using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WorkoutApp.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Exercises",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyPart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSeries = table.Column<int>(type: "int", nullable: true),
                    NumberOfRepeats = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Exercises", x => x.ExerciseId);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.ProductCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Calendars",
                columns: table => new
                {
                    CalendarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Calendars", x => x.CalendarId);
                    table.ForeignKey(
                        name: "FK_Calendars_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutPlans",
                columns: table => new
                {
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    isPreferred = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutPlans", x => x.WorkoutPlanId);
                    table.ForeignKey(
                        name: "FK_WorkoutPlans_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CalendarDays",
                columns: table => new
                {
                    CalendarDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CalendarDate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CalendarId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CalendarDays", x => x.CalendarDayId);
                    table.ForeignKey(
                        name: "FK_CalendarDays_Calendars_CalendarId",
                        column: x => x.CalendarId,
                        principalTable: "Calendars",
                        principalColumn: "CalendarId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    NoteId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.NoteId);
                    table.ForeignKey(
                        name: "FK_Notes_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "WorkoutPlanId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Meals",
                columns: table => new
                {
                    MealId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MealName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TotalKcal = table.Column<int>(type: "int", nullable: false),
                    CalendarDayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meals", x => x.MealId);
                    table.ForeignKey(
                        name: "FK_Meals_CalendarDays_CalendarDayId",
                        column: x => x.CalendarDayId,
                        principalTable: "CalendarDays",
                        principalColumn: "CalendarDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkoutDays",
                columns: table => new
                {
                    WorkoutDayId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WorkoutPlanId = table.Column<int>(type: "int", nullable: false),
                    CalendarDayId = table.Column<int>(type: "int", nullable: false),
                    CalendarDate = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkoutDays", x => x.WorkoutDayId);
                    table.ForeignKey(
                        name: "FK_WorkoutDays_CalendarDays_CalendarDayId",
                        column: x => x.CalendarDayId,
                        principalTable: "CalendarDays",
                        principalColumn: "CalendarDayId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkoutDays_WorkoutPlans_WorkoutPlanId",
                        column: x => x.WorkoutPlanId,
                        principalTable: "WorkoutPlans",
                        principalColumn: "WorkoutPlanId");
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductKcal = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryId = table.Column<int>(type: "int", nullable: false),
                    ProductCategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MealId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductId);
                    table.ForeignKey(
                        name: "FK_Products_Meals_MealId",
                        column: x => x.MealId,
                        principalTable: "Meals",
                        principalColumn: "MealId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategoryId",
                        column: x => x.ProductCategoryId,
                        principalTable: "ProductCategories",
                        principalColumn: "ProductCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserExercises",
                columns: table => new
                {
                    UserExerciseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BodyPart = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumberOfSeries = table.Column<int>(type: "int", nullable: false),
                    NumberOfRepeats = table.Column<int>(type: "int", nullable: false),
                    WorkoutDayId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserExercises", x => x.UserExerciseId);
                    table.ForeignKey(
                        name: "FK_UserExercises_WorkoutDays_WorkoutDayId",
                        column: x => x.WorkoutDayId,
                        principalTable: "WorkoutDays",
                        principalColumn: "WorkoutDayId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Exercises",
                columns: new[] { "ExerciseId", "BodyPart", "Description", "ExerciseName", "NumberOfRepeats", "NumberOfSeries" },
                values: new object[,]
                {
                    { 1, "Plecy", "Podciąganie na drążku to ćwiczenie wykonywane przy użyciu wagi ciała, polegające na chwyceniu drążka nad głową, dłonie skierowane na zewnątrz. Poprzez unoszenie ciała ku górze skupiasz się na mięśniach górnej części pleców, szczególnie na mięśniu czworobocznym. Ćwiczenie to również angażuje bicepsy i mięśnie brzucha, zapewniając wszechstronny trening górnej części ciała.", "Podciąganie na drążku", null, null },
                    { 2, "Klatka piersiowa", "Wyciskanie sztangi to ruch wykonywany przy użyciu ciężarów. Sztanga startuje na kołkach bezpieczeństwa, co dodaje wyzwań związanych z bezruchem. Unoszenie i wyciskanie wzmacnia mięśnie klatki piersiowej, tricepsów i ramion. Ćwiczenie to koncentruje się na osłabionych punktach w wyciskaniu sztangi.", "Wyciskanie sztangi", null, null },
                    { 3, "Barki", "Podciąganie liny do twarzy to ćwiczenie wykonywane przy użyciu maszyny z linką i końcówką w kształcie liny. Pociągnij linkę w kierunku twarzy, trzymając łokcie wysoko, aby zaangażować mięśnie naramienne tyłowe oraz górną część pleców. Ćwiczenie to poprawia stabilność ramion i postawę ciała.", "Podciąganie liny do twarzy", null, null },
                    { 4, "Mięsień trójgłowy ramienia", "Połączenie ćwiczeń Skull Crusher i Pullover. Zacznij od wyciągnięcia sztangi na wyciskanie nad czoło (skull crusher), a następnie przenieś ją nad głowę (pullover). Zaangażowana zostaje trójgłowa część ramienia, klatka piersiowa i mięśnie pleców, zapewniając kompleksowy trening górnej części ciała.", "Skull crusher i pullover", null, null },
                    { 5, "Biceps", "Uginanie ramion ze sztangą to klasyczne ćwiczenie wzmacniające mięśnie ramion. Trzymaj sztangę podchwytem, dłonie skierowane do góry, i unosząc ją ku górze, wykorzystaj mięśnie bicepasa. Opuszczaj sztangę z kontrolą. Ćwiczenie to koncentruje się na mięśniach bicepsa, pomagając rozwijać ich definicję i siłę.", "Uginanie ramion ze sztangą", null, null },
                    { 6, "Brzuch", "Plank na piłce szwajcarskiej to efektywne ćwiczenie dla mięśni brzucha. Rozpocznij w pozycji plank na przedramionach, piłka szwajcarska pod brzuchem, a palce stóp na podłodze. Utrzymuj prostą linię od głowy do pięt, angażując mięśnie brzucha i stabilizujące. Ćwiczenie to pomaga poprawić siłę, stabilność i równowagę mięśni brzucha.", "Plank na piłce szwajcarskiej", null, null },
                    { 7, "Łydki", "Podnoszenie pięt ze sztangą w staniu to ćwiczenie wzmacniające łydki. Stań prosto, ze sztangą opierającą się na górnej części pleców. Wstawaj na palce poprzez unoszenie pięt i przesuwanie wagi ciała na przednią część stóp. Opuszczaj pięty z kontrolą, wykonując pełen zakres ruchu. Ćwiczenie to koncentruje się na mięśniach łydek, poprawiając ich siłę i wyrazistość.", "Podnoszenie pięt ze sztangą w staniu", null, null },
                    { 8, "Czworogłowe uda", "Wypady z sztangą nad plecami to ćwiczenie dolnej partii ciała. Trzymając sztangę na górnej części pleców, cofnij się jedną nogą do wypadu, obniżając kolano tylnej nogi w kierunku podłoża. Napnij przednią nogę, aby wrócić do pozycji wyjściowej. Ćwiczenie to angażuje mięśnie czworogłowe uda, mięśnie dwugłowe uda i pośladki, poprawiając siłę i stabilność nóg.", "Wypady z sztangą nad plecami", null, null },
                    { 9, "Dolny odcinek pleców", "Martwy ciąg na podwyższeniu to ćwiczenie z ciężarami, polegające na podnoszeniu sztangi z podwyższenia. Stań na podkładkach lub blokach, chwytając sztangę podchwytem, i unosząc ją poprzez wyprostowanie bioder i kolan. Opuszczaj sztangę z kontrolą. Ćwiczenie to angażuje mięśnie dwugłowe uda, pośladki, dolny odcinek pleców i mięśnie brzucha, wspierając ogólną siłę i rozwój mięśniowy.", "Martwy ciąg na podwyższeniu", null, null },
                    { 10, "Biceps", "Uginanie ramion z hantlami w supinacji to ćwiczenie wzmacniające mięśnie bicepsa. Trzymaj hantle w dłoniach ze zwiniętymi do góry dłońmi (supinacja) i unieś je ku górze, skręcając bicepsy. Opuszczaj hantle z kontrolą. Ćwiczenie to efektywnie izoluje i rozwija mięśnie bicepsa, poprawiając siłę i definicję ramion.", "Uginanie ramion z hantlami w supinacji", null, null }
                });

            migrationBuilder.InsertData(
                table: "ProductCategories",
                columns: new[] { "ProductCategoryId", "ProductCategoryName" },
                values: new object[,]
                {
                    { 1, "Owoce" },
                    { 2, "Warzywa" },
                    { 3, "Mięso" },
                    { 4, "Nabiał" },
                    { 5, "Pieczywo" },
                    { 6, "Produkty zbożowe" },
                    { 7, "Ryby i owoce morza" },
                    { 8, "Słodycze" },
                    { 9, "Napoje" },
                    { 10, "Sosy i przyprawy" },
                    { 11, "Przekąski" },
                    { 12, "Zupy" },
                    { 13, "Napoje alkoholowe" },
                    { 14, "Produkty bezglutenowe" },
                    { 15, "Kawy i herbaty" },
                    { 16, "Inne" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CalendarDays_CalendarId",
                table: "CalendarDays",
                column: "CalendarId");

            migrationBuilder.CreateIndex(
                name: "IX_Calendars_UserId",
                table: "Calendars",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Meals_CalendarDayId",
                table: "Meals",
                column: "CalendarDayId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_WorkoutPlanId",
                table: "Notes",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_MealId",
                table: "Products",
                column: "MealId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategoryId",
                table: "Products",
                column: "ProductCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_UserExercises_WorkoutDayId",
                table: "UserExercises",
                column: "WorkoutDayId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_CalendarDayId",
                table: "WorkoutDays",
                column: "CalendarDayId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutDays_WorkoutPlanId",
                table: "WorkoutDays",
                column: "WorkoutPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkoutPlans_UserId",
                table: "WorkoutPlans",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Exercises");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "UserExercises");

            migrationBuilder.DropTable(
                name: "Meals");

            migrationBuilder.DropTable(
                name: "ProductCategories");

            migrationBuilder.DropTable(
                name: "WorkoutDays");

            migrationBuilder.DropTable(
                name: "CalendarDays");

            migrationBuilder.DropTable(
                name: "WorkoutPlans");

            migrationBuilder.DropTable(
                name: "Calendars");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
