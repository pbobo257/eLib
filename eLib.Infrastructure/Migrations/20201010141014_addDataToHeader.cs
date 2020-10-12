﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace eLib.Infrastructure.Migrations
{
    public partial class addDataToHeader : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BooksHeaders",
                columns: new[] { "Id", "Cover", "Name" },
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BooksHeaders",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}