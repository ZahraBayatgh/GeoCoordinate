using Microsoft.EntityFrameworkCore.Migrations;

namespace Location.API.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "location");

            migrationBuilder.CreateTable(
                name: "UserLocations",
                schema: "location",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    BaseLatitude = table.Column<double>(nullable: false),
                    BaseLongitude = table.Column<double>(nullable: false),
                    TargetLatitude = table.Column<double>(nullable: false),
                    TargetLongitude = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLocations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserLocations",
                schema: "location");
        }
    }
}
