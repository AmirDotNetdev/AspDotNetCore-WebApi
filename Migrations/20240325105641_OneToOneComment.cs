using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class OneToOneComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "762faa76-4c24-46cf-a76b-966f66e3fb07");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8a8c7deb-84ea-4ff9-8cd0-671e226fa07e");

            migrationBuilder.AddColumn<string>(
                name: "AppUserId",
                table: "comment",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "5f09a354-c02f-4e33-b1c6-7ee56a215134", null, "Admin", "ADMIN" },
                    { "c320d7d7-564a-474b-9639-7638f1b8d99b", null, "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_comment_AppUserId",
                table: "comment",
                column: "AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_comment_AspNetUsers_AppUserId",
                table: "comment",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_comment_AspNetUsers_AppUserId",
                table: "comment");

            migrationBuilder.DropIndex(
                name: "IX_comment_AppUserId",
                table: "comment");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f09a354-c02f-4e33-b1c6-7ee56a215134");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "c320d7d7-564a-474b-9639-7638f1b8d99b");

            migrationBuilder.DropColumn(
                name: "AppUserId",
                table: "comment");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "762faa76-4c24-46cf-a76b-966f66e3fb07", null, "Admin", "ADMIN" },
                    { "8a8c7deb-84ea-4ff9-8cd0-671e226fa07e", null, "User", "USER" }
                });
        }
    }
}
