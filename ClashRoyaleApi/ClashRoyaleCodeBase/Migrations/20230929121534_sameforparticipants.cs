using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashRoyaleCodeBase.Migrations
{
    /// <inheritdoc />
    public partial class sameforparticipants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SeasonSectionIndex",
                table: "RiverRaceParticipant",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeasonSectionIndex",
                table: "RiverRaceParticipant");
        }
    }
}
