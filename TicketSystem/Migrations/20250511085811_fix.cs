using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class fix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentID1",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_DepartmentID1",
                table: "Categories",
                column: "DepartmentID1");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Departments_DepartmentID1",
                table: "Categories",
                column: "DepartmentID1",
                principalTable: "Departments",
                principalColumn: "DepartmentID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Departments_DepartmentID1",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_DepartmentID1",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "DepartmentID1",
                table: "Categories");
        }
    }
}
