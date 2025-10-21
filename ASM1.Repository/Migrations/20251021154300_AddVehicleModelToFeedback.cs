using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ASM1.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddVehicleModelToFeedback : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "VehicleModelId",
                table: "Feedback",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_VehicleModelId",
                table: "Feedback",
                column: "VehicleModelId");

            migrationBuilder.AddForeignKey(
                name: "FK_Feedback_VehicleModel_VehicleModelId",
                table: "Feedback",
                column: "VehicleModelId",
                principalTable: "VehicleModel",
                principalColumn: "vehicleModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Feedback_VehicleModel_VehicleModelId",
                table: "Feedback");

            migrationBuilder.DropIndex(
                name: "IX_Feedback_VehicleModelId",
                table: "Feedback");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "User");

            migrationBuilder.DropColumn(
                name: "VehicleModelId",
                table: "Feedback");
        }
    }
}
