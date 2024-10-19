using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddGymSession : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RemainingCapacity",
                table: "AppointmentsTemp",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GymSessionId",
                table: "Appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "GymSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    GymId = table.Column<int>(type: "int", nullable: false),
                    StartSession = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndSession = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GymSessions_Gyms_GymId",
                        column: x => x.GymId,
                        principalTable: "Gyms",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_GymSessions_Sessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Sessions",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_GymSessionId",
                table: "Appointments",
                column: "GymSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_GymSessions_GymId",
                table: "GymSessions",
                column: "GymId");

            migrationBuilder.CreateIndex(
                name: "IX_GymSessions_SessionId",
                table: "GymSessions",
                column: "SessionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_GymSessions_GymSessionId",
                table: "Appointments",
                column: "GymSessionId",
                principalTable: "GymSessions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_GymSessions_GymSessionId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "GymSessions");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_GymSessionId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "GymSessionId",
                table: "Appointments");

            migrationBuilder.AlterColumn<int>(
                name: "RemainingCapacity",
                table: "AppointmentsTemp",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SessionId",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "Appointments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
