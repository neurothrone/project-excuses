using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Excuses.Persistence.EFCore.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Excuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Excuses", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Excuses",
                columns: new[] { "Id", "Category", "Text" },
                values: new object[,]
                {
                    { 1, "work", "My computer exploded." },
                    { 2, "work", "I got stuck in an endless email loop." },
                    { 3, "work", "My pet python is shedding and needs supervision." },
                    { 4, "school", "Aliens abducted my homework." },
                    { 5, "school", "The dog ate my laptop." },
                    { 6, "school", "I got lost in the metaverse and couldn't escape." },
                    { 7, "social", "I accidentally set my alarm for PM instead of AM." },
                    { 8, "social", "I was practicing social distancing... from everyone." },
                    { 9, "social", "My grandma challenged me to a gaming tournament." },
                    { 10, "technology", "My Wi-Fi ran out of data." },
                    { 11, "technology", "The internet was too slow to function." },
                    { 12, "technology", "I mistakenly set my phone to 'Airplane Mode' and it flew away." },
                    { 13, "pets", "My cat hid my car keys." },
                    { 14, "pets", "My dog locked me out." },
                    { 15, "pets", "My parrot changed my password and won’t tell me what it is." },
                    { 16, "general", "Gravity stopped working for me temporarily." },
                    { 17, "general", "I got stuck in an existential crisis." },
                    { 18, "general", "I was time-traveling and lost track of reality." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Excuses");
        }
    }
}
