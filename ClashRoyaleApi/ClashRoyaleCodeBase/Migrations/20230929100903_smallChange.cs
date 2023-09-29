using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashRoyaleCodeBase.Migrations
{
    /// <inheritdoc />
    public partial class smallChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DecksNotUsed",
                table: "RiverRaceParticipant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DecksNotUsed",
                table: "RiverRaceParticipant");
        }
    }
}
