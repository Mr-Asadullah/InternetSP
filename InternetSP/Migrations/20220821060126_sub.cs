using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternetSP.Migrations
{
    public partial class sub : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SubscribePkgs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dateime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PackgeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubscribePkgs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubscribePkgs_Packges_PackgeId",
                        column: x => x.PackgeId,
                        principalTable: "Packges",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SubscribePkgs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SubscribePkgs_PackgeId",
                table: "SubscribePkgs",
                column: "PackgeId");

            migrationBuilder.CreateIndex(
                name: "IX_SubscribePkgs_UserId",
                table: "SubscribePkgs",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubscribePkgs");
        }
    }
}
