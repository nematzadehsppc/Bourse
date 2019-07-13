using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Back.DAL.Migrations
{
    public partial class initd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__Item__",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___Item__", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "__Symbol__",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___Symbol__", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "__User__",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    FamilyName = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    UserName = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    CheckSum = table.Column<string>(type: "VARCHAR(64)", maxLength: 64, nullable: true),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(255)", maxLength: 12, nullable: true),
                    Email = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: true),
                    BirthDate = table.Column<DateTime>(type: "DateTime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___User__", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "__ParamValue__",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TradingDate = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Value = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    SymbolId = table.Column<int>(nullable: false),
                    ItemId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___ParamValue__", x => x.Id);
                    table.ForeignKey(
                        name: "FK___ParamValue_____Item___ItemId",
                        column: x => x.ItemId,
                        principalTable: "__Item__",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK___ParamValue_____Symbol___SymbolId",
                        column: x => x.SymbolId,
                        principalTable: "__Symbol__",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX___ParamValue___ItemId",
                table: "__ParamValue__",
                column: "ItemId");

            migrationBuilder.CreateIndex(
                name: "IX___ParamValue___SymbolId",
                table: "__ParamValue__",
                column: "SymbolId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "__ParamValue__");

            migrationBuilder.DropTable(
                name: "__User__");

            migrationBuilder.DropTable(
                name: "__Item__");

            migrationBuilder.DropTable(
                name: "__Symbol__");
        }
    }
}
