using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticaret_Project.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactInfoNew : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_ContactInfo_ContactInfoId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_ContactInfoId",
                table: "Images");

            migrationBuilder.DeleteData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"));

            migrationBuilder.DropColumn(
                name: "ContactInfoId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "FacebookUrl",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "InstagramUrl",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "YoutubeUrl",
                table: "ContactInfo");

            migrationBuilder.AddColumn<string>(
                name: "Keywords",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription_Az",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription_En",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaDescription_Ru",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle_Az",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle_En",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MetaTitle_Ru",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title_Az",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title_En",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title_Ru",
                table: "ContactInfo",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 12, 48, 50, 229, DateTimeKind.Utc).AddTicks(1571));

            migrationBuilder.InsertData(
                table: "ContactInfo",
                columns: new[] { "Id", "AddressAZ", "AddressEN", "AddressRU", "CreatedAt", "CreatedUser", "Email", "IsDeleted", "Keywords", "MapIframeSrc", "MetaDescription_Az", "MetaDescription_En", "MetaDescription_Ru", "MetaTitle_Az", "MetaTitle_En", "MetaTitle_Ru", "Phone", "Title_Az", "Title_En", "Title_Ru", "UpdatedAt", "UpdatedUser" },
                values: new object[] { new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "Bakı, Azərbaycan", "Baku, Azerbaijan", "Баку, Азербайджан", new DateTime(2025, 8, 4, 12, 48, 50, 229, DateTimeKind.Utc).AddTicks(8906), null, "info@example.com", false, "Əlaqə, Bizimlə əlaqə, Contact, Communication", "https://maps.google.com/?q=labstend", "Bizimlə əlaqə saxlayın", "Contact us", "Свяжитесь с нами", "Əlaqə", "Contact", "Контакт", "+994502223344", "Əlaqə məlumatları", "Contact Information", "Контактная информация", null, null });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 12, 48, 50, 230, DateTimeKind.Utc).AddTicks(5334));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"));

            migrationBuilder.DropColumn(
                name: "Keywords",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "MetaDescription_Az",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "MetaDescription_En",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "MetaDescription_Ru",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "MetaTitle_Az",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "MetaTitle_En",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "MetaTitle_Ru",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "Title_Az",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "Title_En",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "Title_Ru",
                table: "ContactInfo");

            migrationBuilder.AddColumn<Guid>(
                name: "ContactInfoId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FacebookUrl",
                table: "ContactInfo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InstagramUrl",
                table: "ContactInfo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "YoutubeUrl",
                table: "ContactInfo",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 6, 47, 26, 499, DateTimeKind.Utc).AddTicks(452));

            migrationBuilder.InsertData(
                table: "ContactInfo",
                columns: new[] { "Id", "AddressAZ", "AddressEN", "AddressRU", "CreatedAt", "CreatedUser", "Email", "FacebookUrl", "InstagramUrl", "IsDeleted", "MapIframeSrc", "Phone", "UpdatedAt", "UpdatedUser", "YoutubeUrl" },
                values: new object[] { new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "Bakı, Azərbaycan", "Baku, Azerbaijan", "Баку, Азербайджан", new DateTime(2025, 8, 4, 6, 47, 26, 499, DateTimeKind.Utc).AddTicks(8761), null, "info@labstend.com", "https://facebook.com/labstend", "https://instagram.com/labstend", false, "https://maps.google.com/?q=labstend", "+994 50 123 45 67", null, null, "https://youtube.com/labstend" });

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 6, 47, 26, 500, DateTimeKind.Utc).AddTicks(6097));

            migrationBuilder.CreateIndex(
                name: "IX_Images_ContactInfoId",
                table: "Images",
                column: "ContactInfoId",
                unique: true,
                filter: "[IsDeleted] = 0");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_ContactInfo_ContactInfoId",
                table: "Images",
                column: "ContactInfoId",
                principalTable: "ContactInfo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
