using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PradCat.Api.Migrations
{
    /// <inheritdoc />
    public partial class DeleteCascadeTutor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tutor_TutorId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Tutor",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Tutor_AppUserId",
                table: "Tutor",
                column: "AppUserId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tutor_TutorId",
                table: "AspNetUsers",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Tutor_AspNetUsers_AppUserId",
                table: "Tutor",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Tutor_TutorId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Tutor_AspNetUsers_AppUserId",
                table: "Tutor");

            migrationBuilder.DropIndex(
                name: "IX_Tutor_AppUserId",
                table: "Tutor");

            migrationBuilder.AlterColumn<string>(
                name: "AppUserId",
                table: "Tutor",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Tutor_TutorId",
                table: "AspNetUsers",
                column: "TutorId",
                principalTable: "Tutor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
