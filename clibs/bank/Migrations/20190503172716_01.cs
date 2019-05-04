using Microsoft.EntityFrameworkCore.Migrations;

namespace bank.Migrations
{
    public partial class _01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BookId",
                table: "Factss",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Factss_BookId",
                table: "Factss",
                column: "BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Factss_Books_BookId",
                table: "Factss",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "BookId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Factss_Books_BookId",
                table: "Factss");

            migrationBuilder.DropIndex(
                name: "IX_Factss_BookId",
                table: "Factss");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "Factss");
        }
    }
}
