using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LightOffSchedule.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LightOffSchedules",
                columns: table => new
                {
                    GroupNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightOffSchedules", x => x.GroupNumber);
                });

            migrationBuilder.CreateTable(
                name: "LightOffScheduleIntervals",
                columns: table => new
                {
                    Start = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    End = table.Column<TimeSpan>(type: "TEXT", nullable: false),
                    LightOffScheduleEntityGroupNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LightOffScheduleIntervals", x => new { x.Start, x.End, x.LightOffScheduleEntityGroupNumber });
                    table.ForeignKey(
                        name: "FK_LightOffScheduleIntervals_LightOffSchedules_LightOffScheduleEntityGroupNumber",
                        column: x => x.LightOffScheduleEntityGroupNumber,
                        principalTable: "LightOffSchedules",
                        principalColumn: "GroupNumber",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LightOffScheduleIntervals_LightOffScheduleEntityGroupNumber",
                table: "LightOffScheduleIntervals",
                column: "LightOffScheduleEntityGroupNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LightOffScheduleIntervals");

            migrationBuilder.DropTable(
                name: "LightOffSchedules");
        }
    }
}
