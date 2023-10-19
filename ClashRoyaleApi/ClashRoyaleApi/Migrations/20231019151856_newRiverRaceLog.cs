using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashRoyaleApi.Migrations
{
    /// <inheritdoc />
    public partial class newRiverRaceLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserName",
                table: "DBUser",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "RiverRaceLogs",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    timeStamp = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    SeasonId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiverRaceLogs", x => x.Guid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiverRaceLogs");

            migrationBuilder.DropColumn(
                name: "UserName",
                table: "DBUser");
        }
    }
}
