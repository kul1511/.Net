﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CROPDEAL.Migrations
{
    /// <inheritdoc />
    public partial class PaymentTableAddUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Payments",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Payments");
        }
    }
}
