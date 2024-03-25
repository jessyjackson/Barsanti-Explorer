﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BarsantiExplorer.Migrations
{
    /// <inheritdoc />
    public partial class removeTelegramTmpId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TelegramUserRegistrationId",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TelegramUserRegistrationId",
                table: "Users",
                type: "int",
                nullable: true);
        }
    }
}
