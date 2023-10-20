using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLySinhVien.Migrations
{
    public partial class attContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "CourseContent",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "CourseContent");
        }
    }
}
