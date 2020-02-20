using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace Opeqe.Common.IntegrationEventLogEF.Migrations
{
    public partial class IntegrationEvent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationEventLog",
                columns: table => new
                {
                    EventId = table.Column<Guid>(nullable: false),
                    EventTypeName = table.Column<string>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    TimesSent = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    TransactionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationEventLog", x => x.EventId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationEventLog");
        }
    }
}
