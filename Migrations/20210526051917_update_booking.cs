using Microsoft.EntityFrameworkCore.Migrations;

namespace ResortProjectAPI.Migrations
{
    public partial class update_booking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Staffs_StaffID",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_StaffID",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "StaffID",
                table: "Bookings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StaffID",
                table: "Bookings",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_StaffID",
                table: "Bookings",
                column: "StaffID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Staffs_StaffID",
                table: "Bookings",
                column: "StaffID",
                principalTable: "Staffs",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
