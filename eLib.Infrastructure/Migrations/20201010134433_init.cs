using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eLib.Infrastructure.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Accounts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Login = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    UserType = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Accounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BooksHeaders",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Cover = table.Column<byte[]>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksHeaders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BooksDetails",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Author = table.Column<string>(nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    ReleaseDate = table.Column<DateTime>(type: "date", nullable: false),
                    Genre = table.Column<string>(maxLength: 50, nullable: false),
                    Book = table.Column<byte[]>(nullable: false),
                    HeaderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BooksDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BooksDetails_BooksHeaders_HeaderId",
                        column: x => x.HeaderId,
                        principalTable: "BooksHeaders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Email", "Login", "Password", "UserType" },
                values: new object[] { 1, "DefaultAdmin@gmail.com", "DefaultAdmin", "DefaultAdminPassword123", "Admin" });

            migrationBuilder.InsertData(
                table: "Accounts",
                columns: new[] { "Id", "Email", "Login", "Password", "UserType" },
                values: new object[] { 2, "DefaultUser@gmail.com", "DefaultUser", "DefaultUserPassword123", "User" });

            migrationBuilder.CreateIndex(
                name: "IX_BooksDetails_HeaderId",
                table: "BooksDetails",
                column: "HeaderId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Accounts");

            migrationBuilder.DropTable(
                name: "BooksDetails");

            migrationBuilder.DropTable(
                name: "BooksHeaders");
        }
    }
}
