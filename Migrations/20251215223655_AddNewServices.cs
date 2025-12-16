using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SporSalonu2.Migrations
{
    /// <inheritdoc />
    public partial class AddNewServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Kardiyo ve genel form.");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Esneklik ve denge.");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Core güçlendirme.");

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "DurationMinutes", "Name", "Price" },
                values: new object[,]
                {
                    { 4, "Serbest ağırlık ve vücut geliştirme.", 90, "Gym & Ağırlık", 500m },
                    { 5, "Yüzme antrenmanları.", 60, "Havuz", 600m },
                    { 6, "Boks ve kickboks dersleri.", 60, "Boks", 550m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1,
                column: "Description",
                value: "Ağırlık ve kardiyo ekipmanları.");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2,
                column: "Description",
                value: "Esneklik ve zihin dengesi.");

            migrationBuilder.UpdateData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3,
                column: "Description",
                value: "Core güçlendirme ve duruş düzeltme.");
        }
    }
}

