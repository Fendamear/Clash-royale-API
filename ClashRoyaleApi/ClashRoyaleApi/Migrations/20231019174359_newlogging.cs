using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashRoyaleApi.Migrations
{
    /// <inheritdoc />
    public partial class newlogging : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentRiverRaceLogs",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TimeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SchedulerTime = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Exception = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentRiverRaceLogs", x => x.Guid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentRiverRaceLogs");
        }
    }
}
