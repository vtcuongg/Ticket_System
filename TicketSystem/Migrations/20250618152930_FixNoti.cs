using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TicketSystem.Migrations
{
    /// <inheritdoc />
    public partial class FixNoti : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Tickets_TicketID",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_TicketID",
                table: "Notifications");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notifications_TicketID",
                table: "Notifications",
                column: "TicketID");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Tickets_TicketID",
                table: "Notifications",
                column: "TicketID",
                principalTable: "Tickets",
                principalColumn: "TicketID");
        }
    }
}
