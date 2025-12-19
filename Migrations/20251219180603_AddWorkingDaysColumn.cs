using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SporSalonu2.Migrations
{
    /// <inheritdoc />
    public partial class AddWorkingDaysColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Availabilities");

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Services",
                keyColumn: "Id",
                keyValue: 3);

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

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trainers",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AlterColumn<string>(
                name: "Specialty",
                table: "Trainers",
                type: "nvarchar(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "WorkingDays",
                table: "Trainers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkingDays",
                table: "Trainers");

            migrationBuilder.AlterColumn<string>(
                name: "Specialty",
                table: "Trainers",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldMaxLength: 300);

            migrationBuilder.CreateTable(
                name: "Availabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsBookable = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Availabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Availabilities_Trainers_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Services",
                columns: new[] { "Id", "Description", "DurationMinutes", "Name", "Price" },
                values: new object[,]
                {
                    { 1, "Kardiyo ve genel form.", 60, "Fitness", 350m },
                    { 2, "Esneklik ve denge.", 45, "Yoga", 400m },
                    { 3, "Core güçlendirme.", 50, "Pilates", 450m },
                    { 4, "Serbest ağırlık ve vücut geliştirme.", 90, "Gym & Ağırlık", 500m },
                    { 5, "Yüzme antrenmanları.", 60, "Havuz", 600m },
                    { 6, "Boks ve kickboks dersleri.", 60, "Boks", 550m }
                });

            migrationBuilder.InsertData(
                table: "Trainers",
                columns: new[] { "Id", "Bio", "Email", "FirstName", "LastName", "Phone", "Specialty" },
                values: new object[,]
                {
                    { 1, "Kişisel antrenör ve beslenme danışmanı.", "ahmet@fitzone.com", "Ahmet", "Yılmaz", "5551112233", "Fitness & Kilo Verme" },
                    { 2, "Uzman yoga eğitmeni.", "zeynep@fitzone.com", "Zeynep", "Kara", "5554445566", "Yoga & Meditasyon" },
                    { 3, "Reformer ve mat pilates uzmanı.", "mehmet@fitzone.com", "Mehmet", "Demir", "5557778899", "Pilates & Rehabilitasyon" }
                });

            migrationBuilder.InsertData(
                table: "Availabilities",
                columns: new[] { "Id", "DayOfWeek", "EndTime", "IsBookable", "StartTime", "TrainerId" },
                values: new object[,]
                {
                    { 1, 0, new TimeSpan(0, 12, 0, 0, 0), true, new TimeSpan(0, 9, 0, 0, 0), 1 },
                    { 2, 1, new TimeSpan(0, 17, 0, 0, 0), true, new TimeSpan(0, 14, 0, 0, 0), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Availabilities_TrainerId",
                table: "Availabilities",
                column: "TrainerId");
        }
    }
}
