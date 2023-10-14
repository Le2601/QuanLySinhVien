using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLySinhVien.Migrations
{
    public partial class quanent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Creator",
                table: "Course",
                newName: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Course_AccountId",
                table: "Course",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_Account_AccountId",
                table: "Course",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_Account_AccountId",
                table: "Course");

            migrationBuilder.DropIndex(
                name: "IX_Course_AccountId",
                table: "Course");

            migrationBuilder.RenameColumn(
                name: "AccountId",
                table: "Course",
                newName: "Creator");
        }
    }
}
