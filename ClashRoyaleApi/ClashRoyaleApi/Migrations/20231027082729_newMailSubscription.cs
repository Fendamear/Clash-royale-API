using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ClashRoyaleApi.Migrations
{
    /// <inheritdoc />
    public partial class newMailSubscription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "type",
                table: "RiverRaceLogs",
                newName: "Type");

            migrationBuilder.RenameColumn(
                name: "timeStamp",
                table: "RiverRaceLogs",
                newName: "TimeStamp");

            migrationBuilder.RenameColumn(
                name: "Tag",
                table: "DbClanMembers",
                newName: "ClanTag");

            migrationBuilder.UpdateData(
                table: "RiverClanMemberLog",
                keyColumn: "OldValue",
                keyValue: null,
                column: "OldValue",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "OldValue",
                table: "RiverClanMemberLog",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "RiverClanMemberLog",
                keyColumn: "NewValue",
                keyValue: null,
                column: "NewValue",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "NewValue",
                table: "RiverClanMemberLog",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MailSubscriptions",
                columns: table => new
                {
                    Guid = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    MailType = table.Column<int>(type: "int", nullable: false),
                    SchedulerTime = table.Column<int>(type: "int", nullable: true),
                    ClanTag = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MailSubscriptions", x => x.Guid);
                })
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MailSubscriptions");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "RiverRaceLogs",
                newName: "type");

            migrationBuilder.RenameColumn(
                name: "TimeStamp",
                table: "RiverRaceLogs",
                newName: "timeStamp");

            migrationBuilder.RenameColumn(
                name: "ClanTag",
                table: "DbClanMembers",
                newName: "Tag");

            migrationBuilder.AlterColumn<string>(
                name: "OldValue",
                table: "RiverClanMemberLog",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "NewValue",
                table: "RiverClanMemberLog",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");
        }
    }
}
