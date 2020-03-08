﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace LinkShorter.Migrations
{
    public partial class AddedEventDateInUrlStatistics : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "UrlStatistics",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "UrlStatistics");
        }
    }
}
