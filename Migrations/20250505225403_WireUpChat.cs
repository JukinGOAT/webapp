using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapp.Migrations
{
    /// <inheritdoc />
    public partial class WireUpChat : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserProfiles_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserProfiles_SenderId",
                table: "Messages");

            migrationBuilder.InsertData(
                table: "UserProfiles",
                columns: new[] { "Id", "About", "CoverImageUrl", "Faculty", "FullName", "IsOnline", "ProfileImageUrl", "Tag", "Tags" },
                values: new object[,]
                {
                    { 2, "", "", "", "Marko Marić", false, "/images/avatars/marko.png", "", "" },
                    { 3, "", "", "", "Ivana Valić", false, "/images/avatars/ivana.png", "", "" }
                });

            migrationBuilder.InsertData(
                table: "Messages",
                columns: new[] { "Id", "Body", "ReceiverId", "SenderId", "SentAt" },
                values: new object[,]
                {
                    { 2, "Super, hvala – a ti?", 1, 2, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) },
                    { 3, "Bok, kako si?", 1, 3, new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc) }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserProfiles_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserProfiles_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserProfiles_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_UserProfiles_SenderId",
                table: "Messages");

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Messages",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "UserProfiles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserProfiles_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_UserProfiles_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
