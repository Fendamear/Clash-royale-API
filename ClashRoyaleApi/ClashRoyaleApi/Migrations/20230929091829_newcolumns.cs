using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashRoyaleApi.Migrations
{
    /// <inheritdoc />
    public partial class newcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Fame",
                table: "RiverRaceClan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "RiverRaceClan",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "NewTrophies",
                table: "RiverRaceClan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "RiverRaceClan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "RiverRaceClan",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "TrophyChange",
                table: "RiverRaceClan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fame",
                table: "RiverRaceClan");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "RiverRaceClan");

            migrationBuilder.DropColumn(
                name: "NewTrophies",
                table: "RiverRaceClan");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "RiverRaceClan");

            migrationBuilder.DropColumn(
                name: "Tag",
                table: "RiverRaceClan");

            migrationBuilder.DropColumn(
                name: "TrophyChange",
                table: "RiverRaceClan");
        }
    }
}
