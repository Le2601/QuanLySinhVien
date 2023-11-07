using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLySinhVien.Migrations
{
    public partial class hahaaa : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseContent_Course_CourseId",
                table: "CourseContent");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseContent",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_CourseContent_Course_CourseId",
                table: "CourseContent",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CourseContent_Course_CourseId",
                table: "CourseContent");

            migrationBuilder.AlterColumn<int>(
                name: "CourseId",
                table: "CourseContent",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CourseContent_Course_CourseId",
                table: "CourseContent",
                column: "CourseId",
                principalTable: "Course",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
