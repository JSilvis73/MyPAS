using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyPAS.Migrations
{
    /// <inheritdoc />
    public partial class RenameServiceToProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Services_ServiceId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Services");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Payments",
                newName: "ProcedureId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ServiceId",
                table: "Payments",
                newName: "IX_Payments_ProcedureId");

            migrationBuilder.CreateTable(
                name: "Procedures",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProcedureName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProcedureDate = table.Column<DateTime>(type: "date", nullable: false),
                    CptCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CptAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PatientChargedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PatientId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Procedures", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Procedures_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Procedures_PatientId",
                table: "Procedures",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Procedures_ProcedureId",
                table: "Payments",
                column: "ProcedureId",
                principalTable: "Procedures",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Procedures_ProcedureId",
                table: "Payments");

            migrationBuilder.DropTable(
                name: "Procedures");

            migrationBuilder.RenameColumn(
                name: "ProcedureId",
                table: "Payments",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Payments_ProcedureId",
                table: "Payments",
                newName: "IX_Payments_ServiceId");

            migrationBuilder.CreateTable(
                name: "Services",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<int>(type: "int", nullable: false),
                    CptAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CptCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PatientChargedAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ServiceDate = table.Column<DateTime>(type: "date", nullable: false),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Services", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Services_Patients_PatientId",
                        column: x => x.PatientId,
                        principalTable: "Patients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Services_PatientId",
                table: "Services",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Services_ServiceId",
                table: "Payments",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
