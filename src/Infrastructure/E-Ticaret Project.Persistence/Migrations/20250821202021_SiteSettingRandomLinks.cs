using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticaret_Project.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SiteSettingRandomLinks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 21, 20, 20, 20, 579, DateTimeKind.Utc).AddTicks(6476));

            migrationBuilder.UpdateData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 21, 20, 20, 20, 580, DateTimeKind.Utc).AddTicks(5888));

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                columns: new[] { "CreatedAt", "FacebookUrl", "InstagramUrl", "WhatsappInquiryLink", "YoutubeUrl" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 20, 20, 582, DateTimeKind.Utc).AddTicks(2922), "https://www.instagram.com/instagram", "https://www.facebook.com/NASA", "https://wa.me/12025550123", "https://www.youtube.com/watch?v=jfKfPfyJRdk" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                columns: new[] { "CreatedAt", "FacebookUrl", "InstagramUrl", "WhatsappInquiryLink", "YoutubeUrl" },
                values: new object[] { new DateTime(2025, 8, 16, 15, 54, 39, 470, DateTimeKind.Utc).AddTicks(1857), null, null, "https://wa.me/994000000000", null });
        }
    }
}
