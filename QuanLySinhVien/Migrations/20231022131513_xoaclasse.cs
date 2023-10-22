using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLySinhVien.Migrations
{
    public partial class xoaclasse : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UploadAssignment_Account_AccountId",
                table: "UploadAssignment");

            migrationBuilder.DropIndex(
                name: "IX_UploadAssignment_AccountId",
                table: "UploadAssignment");

            migrationBuilder.DropColumn(
                name: "AccountId",
                table: "UploadAssignment");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "UploadAssignment",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Mssv",
                table: "UploadAssignment",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FullName",
                table: "UploadAssignment");

            migrationBuilder.DropColumn(
                name: "Mssv",
                table: "UploadAssignment");

            migrationBuilder.AddColumn<int>(
                name: "AccountId",
                table: "UploadAssignment",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UploadAssignment_AccountId",
                table: "UploadAssignment",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_UploadAssignment_Account_AccountId",
                table: "UploadAssignment",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
