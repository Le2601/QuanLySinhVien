using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLySinhVien.Migrations
{
    public partial class SemesterCourse123 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SemesterCourseId",
                table: "Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "SemesterCourse",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SemesterCourse", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Course_SemesterCourseId",
                table: "Course",
                column: "SemesterCourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Course_SemesterCourse_SemesterCourseId",
                table: "Course",
                column: "SemesterCourseId",
                principalTable: "SemesterCourse",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Course_SemesterCourse_SemesterCourseId",
                table: "Course");

            migrationBuilder.DropTable(
                name: "SemesterCourse");

            migrationBuilder.DropIndex(
                name: "IX_Course_SemesterCourseId",
                table: "Course");

            migrationBuilder.DropColumn(
                name: "SemesterCourseId",
                table: "Course");
        }
    }
}
