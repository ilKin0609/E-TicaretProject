using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Ticaret_Project.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SpecialRequestChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SpecialRequestImages");

            migrationBuilder.AddColumn<Guid>(
                name: "SpecialRequestId",
                table: "Images",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 6, 40, 13, 892, DateTimeKind.Utc).AddTicks(8613));

            migrationBuilder.UpdateData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 6, 40, 13, 893, DateTimeKind.Utc).AddTicks(6372));

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 6, 6, 40, 13, 894, DateTimeKind.Utc).AddTicks(3227));

            migrationBuilder.CreateIndex(
                name: "IX_Images_SpecialRequestId",
                table: "Images",
                column: "SpecialRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Images_SpecialRequests_SpecialRequestId",
                table: "Images",
                column: "SpecialRequestId",
                principalTable: "SpecialRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Images_SpecialRequests_SpecialRequestId",
                table: "Images");

            migrationBuilder.DropIndex(
                name: "IX_Images_SpecialRequestId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "SpecialRequestId",
                table: "Images");

            migrationBuilder.CreateTable(
                name: "SpecialRequestImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SpecialRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedUser = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SpecialRequestImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SpecialRequestImages_SpecialRequests_SpecialRequestId",
                        column: x => x.SpecialRequestId,
                        principalTable: "SpecialRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AboutUs",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 12, 48, 50, 229, DateTimeKind.Utc).AddTicks(1571));

            migrationBuilder.UpdateData(
                table: "ContactInfo",
                keyColumn: "Id",
                keyValue: new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 12, 48, 50, 229, DateTimeKind.Utc).AddTicks(8906));

            migrationBuilder.UpdateData(
                table: "SiteSetting",
                keyColumn: "Id",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"),
                column: "CreatedAt",
                value: new DateTime(2025, 8, 4, 12, 48, 50, 230, DateTimeKind.Utc).AddTicks(5334));

            migrationBuilder.CreateIndex(
                name: "IX_SpecialRequestImages_SpecialRequestId",
                table: "SpecialRequestImages",
                column: "SpecialRequestId");
        }
    }
}
