using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashRoyaleApi.Migrations
{
    /// <inheritdoc />
    public partial class currentriverrace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CurrentRiverRace",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    DayId = table.Column<int>(type: "int", nullable: false),
                    SeasonSectionDay = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Tag = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Name = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fame = table.Column<int>(type: "int", nullable: false),
                    DecksUsedToday = table.Column<int>(type: "int", nullable: false),
                    DecksNotUsed = table.Column<int>(type: "int", nullable: false),
                    Schedule = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CurrentRiverRace", x => x.Guid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CurrentRiverRace");
        }
    }
}
