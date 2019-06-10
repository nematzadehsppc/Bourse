using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Back.DAL.Migrations
{
    public partial class initDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "__ParamType__",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___ParamType__", x => x.Id);
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
                    Name = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    Password = table.Column<string>(type: "NVARCHAR(255)", maxLength: 255, nullable: false),
                    AccessLevel = table.Column<int>(nullable: false),
                    PhoneNumber = table.Column<string>(type: "NVARCHAR(255)", maxLength: 12, nullable: false),
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
                    DateDay = table.Column<DateTime>(type: "DateTime", nullable: false),
                    Value = table.Column<decimal>(type: "DECIMAL(18, 2)", nullable: false),
                    SymbolId = table.Column<int>(nullable: false),
                    ParamTypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK___ParamValue__", x => x.Id);
                    table.ForeignKey(
                        name: "FK___ParamValue_____ParamType___ParamTypeId",
                        column: x => x.ParamTypeId,
                        principalTable: "__ParamType__",
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
                name: "IX___ParamValue___ParamTypeId",
                table: "__ParamValue__",
                column: "ParamTypeId");

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
                name: "__ParamType__");

            migrationBuilder.DropTable(
                name: "__Symbol__");
        }
    }
}
