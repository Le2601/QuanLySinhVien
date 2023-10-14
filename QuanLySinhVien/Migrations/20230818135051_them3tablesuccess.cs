using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLySinhVien.Migrations
{
    public partial class them3tablesuccess : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UploadAssignment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExerciseContentId = table.Column<int>(type: "int", nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Data = table.Column<byte[]>(type: "varbinary(max)", nullable: true),
                    UpdateDay = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UploadAssignment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UploadAssignment_Account_AccountId",
                        column: x => x.AccountId,
                        principalTable: "Account",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UploadAssignment_ExerciseContent_ExerciseContentId",
                        column: x => x.ExerciseContentId,
                        principalTable: "ExerciseContent",
                        principalColumn: "Id",
                        //onDelete: ReferentialAction.Cascade);
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UploadAssignment_AccountId",
                table: "UploadAssignment",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_UploadAssignment_ExerciseContentId",
                table: "UploadAssignment",
                column: "ExerciseContentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UploadAssignment");
        }
    }
}
