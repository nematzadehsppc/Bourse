using Microsoft.EntityFrameworkCore.Migrations;

namespace Back.DAL.Migrations
{
    public partial class updateSessionTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FileSize",
                table: "__Session__",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "MCheckSum",
                table: "__Session__",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileSize",
                table: "__Session__");

            migrationBuilder.DropColumn(
                name: "MCheckSum",
                table: "__Session__");
        }
    }
}
