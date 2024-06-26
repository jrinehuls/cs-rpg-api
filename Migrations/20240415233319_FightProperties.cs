﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RPG_API.Migrations
{
    /// <inheritdoc />
    public partial class FightProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Defeats",
                table: "Character",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Fights",
                table: "Character",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Victories",
                table: "Character",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Defeats",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Fights",
                table: "Character");

            migrationBuilder.DropColumn(
                name: "Victories",
                table: "Character");
        }
    }
}
