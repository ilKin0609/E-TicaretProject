using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticaret_Project.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AboutNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "AboutUs",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription_Az",
                table: "AboutUs",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription_En",
                table: "AboutUs",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription_Ru",
                table: "AboutUs",
                type: "nvarchar(160)",
                maxLength: 160,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle_Az",
                table: "AboutUs",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle_En",
                table: "AboutUs",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle_Ru",
                table: "AboutUs",
                type: "nvarchar(70)",
                maxLength: 70,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "Keywords", "MetaDescription_Az", "MetaDescription_En", "MetaDescription_Ru", "MetaTitle_Az", "MetaTitle_En", "MetaTitle_Ru" },
                values: new object[] { new DateTime(2025, 8, 4, 6, 47, 26, 499, DateTimeKind.Utc).AddTicks(452), "Vinil,Banner,Reklam,Forex", "Vinil, banner, forex və digər reklam xidmətləri təklif edən peşəkar şirkət.", "Professional company offering vinyl, banner, forex and other advertising services.", "Профессиональная компания, предлагающая винил, баннер, форекс и другие рекламные услуги.", "LABstend - Reklam Xidməti", "LABstend - Advertising Service", "LABstend - Рекламные Услуги" });

            migrationBuilder.UpdateData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 6, 47, 26, 499, DateTimeKind.Utc).AddTicks(8761));

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 6, 47, 26, 500, DateTimeKind.Utc).AddTicks(6097));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "MetaDescription_Az",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "MetaDescription_En",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "MetaDescription_Ru",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "MetaTitle_Az",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "MetaTitle_En",
                table: "AboutUs");

            migrationBuilder.DropColumn(
                name: "MetaTitle_Ru",
                table: "AboutUs");

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 13, 58, 8, 789, DateTimeKind.Utc).AddTicks(9091));

            migrationBuilder.UpdateData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 13, 58, 8, 790, DateTimeKind.Utc).AddTicks(6145));

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 1, 13, 58, 8, 791, DateTimeKind.Utc).AddTicks(2875));
        }
    }
}
