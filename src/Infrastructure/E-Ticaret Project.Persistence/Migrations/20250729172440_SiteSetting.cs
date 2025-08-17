using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticaret_Project.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SiteSetting : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SiteSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HideProductPrices = table.Column<bool>(type: "bit", nullable: false),
                    DisablePartnerLogin = table.Column<bool>(type: "bit", nullable: false),
                    HidePopCategory = table.Column<bool>(type: "bit", nullable: false),
                    HideSearchBar = table.Column<bool>(type: "bit", nullable: false),
                    ScrollText = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WhatsappInquiryLink = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SiteSetting", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "SiteSetting",
                columns: new[] { "Id", "CreatedAt", "CreatedUser", "DisablePartnerLogin", "HidePopCategory", "HideProductPrices", "HideSearchBar", "IsDeleted", "ScrollText", "UpdatedAt", "UpdatedUser", "WhatsappInquiryLink" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), new DateTime(2025, 7, 29, 17, 24, 40, 263, DateTimeKind.Utc).AddTicks(1167), null, false, false, false, false, false, "Xoş gəlmisiniz!", null, null, "https://wa.me/994000000000" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SiteSetting");
        }
    }
}
