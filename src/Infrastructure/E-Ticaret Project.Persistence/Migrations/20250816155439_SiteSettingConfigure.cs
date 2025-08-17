using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticaret_Project.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SiteSettingConfigure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ScroolTextSpeed",
                table: "SiteSetting",
                newName: "ScrollTextSpeed");

            migrationBuilder.AddColumn<string>(
                name: "BingSiteVerification",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GoogleSiteVerification",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeKeywords",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeMetaDescriptionAz",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeMetaDescriptionEn",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeMetaDescriptionRu",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeMetaTitleAz",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeMetaTitleEn",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HomeMetaTitleRu",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PublicEmail",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YandexSiteVerification",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "YoutubeUrl",
                table: "SiteSetting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 15, 54, 39, 467, DateTimeKind.Utc).AddTicks(5714));

            migrationBuilder.UpdateData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 15, 54, 39, 468, DateTimeKind.Utc).AddTicks(5083));

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "BingSiteVerification", "CreatedAt", "FacebookUrl", "GoogleSiteVerification", "HomeKeywords", "HomeMetaDescriptionAz", "HomeMetaDescriptionEn", "HomeMetaDescriptionRu", "HomeMetaTitleAz", "HomeMetaTitleEn", "HomeMetaTitleRu", "InstagramUrl", "PublicEmail", "YandexSiteVerification", "YoutubeUrl" },
                values: new object[] { null, new DateTime(2025, 8, 16, 15, 54, 39, 470, DateTimeKind.Utc).AddTicks(1857), null, null, null, null, null, null, null, null, null, null, null, null, null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BingSiteVerification",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "GoogleSiteVerification",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "HomeKeywords",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "HomeMetaDescriptionAz",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "HomeMetaDescriptionEn",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "HomeMetaDescriptionRu",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "HomeMetaTitleAz",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "HomeMetaTitleEn",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "HomeMetaTitleRu",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "PublicEmail",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "YandexSiteVerification",
                table: "SiteSetting");

            migrationBuilder.DropColumn(
                name: "YoutubeUrl",
                table: "SiteSetting");

            migrationBuilder.RenameColumn(
                name: "ScrollTextSpeed",
                table: "SiteSetting",
                newName: "ScroolTextSpeed");

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 14, 43, 7, 874, DateTimeKind.Utc).AddTicks(2155));

            migrationBuilder.UpdateData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 14, 43, 7, 875, DateTimeKind.Utc).AddTicks(1940));

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 16, 14, 43, 7, 876, DateTimeKind.Utc).AddTicks(9229));
        }
    }
}
