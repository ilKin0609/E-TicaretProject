using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticaret_Project.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AboutContactInfoImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "AboutUs");

            migrationBuilder.AddColumn<Guid>(
                name: "AboutUsId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ContactInfoId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AboutUs",
                columns: new[] { "Id", "CreatedAt", "CreatedUser", "DescriptionAZ", "DescriptionEN", "DescriptionRU", "IsActive", "IsDeleted", "TitleAZ", "TitleEN", "TitleRU", "UpdatedAt", "UpdatedUser" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 7, 30, 9, 52, 26, 689, DateTimeKind.Utc).AddTicks(3098), null, "LABstend Şirkəti", "LABstend Company", "Компания Лабстенд ", true, false, "Haqqımızda", "AboutUs", "О нас", null, null });

            migrationBuilder.InsertData(
                table: "ContactInfo",
                columns: new[] { "Id", "AddressAZ", "AddressEN", "AddressRU", "CreatedAt", "CreatedUser", "Email", "FacebookUrl", "InstagramUrl", "IsActive", "IsDeleted", "MapIframeSrc", "Phone", "UpdatedAt", "UpdatedUser", "YoutubeUrl" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Bakı, Azərbaycan", "Baku, Azerbaijan", "Баку, Азербайджан", new DateTime(2025, 7, 30, 9, 52, 26, 690, DateTimeKind.Utc).AddTicks(876), null, "info@labstend.com", "https://facebook.com/labstend", "https://instagram.com/labstend", true, false, "https://maps.google.com/?q=labstend", "+994 50 123 45 67", null, null, "https://youtube.com/labstend" });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 30, 9, 52, 26, 690, DateTimeKind.Utc).AddTicks(8088));

            migrationBuilder.CreateIndex(
                name: "IX_Images_AboutUsId",
                table: "Images",
                column: "AboutUsId",
                unique: true,
                filter: "[AboutUsId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Images_ContactInfoId",
                table: "Images",
                column: "ContactInfoId",
                unique: true,
                filter: "[ContactInfoId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_AboutUs_AboutUsId",
                table: "Images",
                column: "AboutUsId",
                principalTable: "AboutUs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ContactInfo_ContactInfoId",
                table: "Images",
                column: "ContactInfoId",
                principalTable: "ContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_AboutUs_AboutUsId",
                table: "Images");

            migrationBuilder.DropForeignKey(
                name: "FK_Images_ContactInfo_ContactInfoId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_AboutUsId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ContactInfoId",
                table: "Images");

            migrationBuilder.DeleteData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));

            migrationBuilder.DeleteData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DropColumn(
                name: "AboutUsId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ContactInfoId",
                table: "Images");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "AboutUs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 7, 29, 18, 25, 25, 710, DateTimeKind.Utc).AddTicks(435));
        }
    }
}
