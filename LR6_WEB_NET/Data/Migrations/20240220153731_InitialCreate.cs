using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LR6_WEB_NET.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Animals",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ScientificName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Animals", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Keepers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Age = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Keepers", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "user_roles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_roles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Shifts",
                columns: table => new
                {
                    KeeperId = table.Column<int>(type: "int", nullable: false),
                    AnimalId = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    Salary = table.Column<double>(type: "double", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => new { x.KeeperId, x.AnimalId });
                    table.ForeignKey(
                        name: "FK_Shifts_Animals_AnimalId",
                        column: x => x.AnimalId,
                        principalTable: "Animals",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Shifts_Keepers_KeeperId",
                        column: x => x.KeeperId,
                        principalTable: "Keepers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    FirstName = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "longblob", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "longblob", nullable: false),
                    LastLogin = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    InvalidLoginAttempts = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_user_roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "user_roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Animals",
                columns: new[] { "Id", "Age", "Name", "ScientificName" },
                values: new object[,]
                {
                    { 1, 5, "Lion", "Panthera leo" },
                    { 2, 4, "Tiger", "Panthera tigris" },
                    { 3, 10, "Elephant", "Loxodonta" },
                    { 4, 7, "Giraffe", "Giraffa camelopardalis" },
                    { 5, 6, "Zebra", "Equus zebra" },
                    { 6, 8, "Hippopotamus", "Hippopotamus amphibius" },
                    { 7, 9, "Crocodile", "Crocodylus" },
                    { 8, 3, "Penguin", "Spheniscidae" },
                    { 9, 2, "Kangaroo", "Macropodidae" },
                    { 10, 1, "Koala", "Phascolarctos cinereus" }
                });

            migrationBuilder.InsertData(
                table: "Keepers",
                columns: new[] { "Id", "Age", "Name" },
                values: new object[,]
                {
                    { 1, 25, "John Doe" },
                    { 2, 30, "Jane Doe" },
                    { 3, 35, "John Smith" },
                    { 4, 40, "Jane Smith" },
                    { 5, 31, "Steve Lane" },
                    { 6, 32, "Conor Wood" },
                    { 7, 33, "Alfred Wolf" },
                    { 8, 34, "Jim How" },
                    { 9, 35, "Jane Rich" },
                    { 10, 36, "Jack Clinton" }
                });

            migrationBuilder.InsertData(
                table: "user_roles",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Admin" },
                    { 2, "User" }
                });

            migrationBuilder.InsertData(
                table: "Shifts",
                columns: new[] { "AnimalId", "KeeperId", "EndDate", "Salary", "StartDate" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 2, 21, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8061), 100.0, new DateTime(2024, 2, 20, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8008) },
                    { 2, 2, new DateTime(2024, 2, 22, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8069), 200.0, new DateTime(2024, 2, 21, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8067) },
                    { 3, 3, new DateTime(2024, 2, 23, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8073), 300.0, new DateTime(2024, 2, 22, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8071) },
                    { 4, 4, new DateTime(2024, 2, 24, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8077), 400.0, new DateTime(2024, 2, 23, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8075) },
                    { 5, 5, new DateTime(2024, 2, 25, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8081), 500.0, new DateTime(2024, 2, 24, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8079) },
                    { 6, 6, new DateTime(2024, 2, 26, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8085), 600.0, new DateTime(2024, 2, 25, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8083) },
                    { 7, 7, new DateTime(2024, 2, 27, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8089), 700.0, new DateTime(2024, 2, 26, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8087) },
                    { 8, 8, new DateTime(2024, 2, 28, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8093), 800.0, new DateTime(2024, 2, 27, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8091) },
                    { 9, 9, new DateTime(2024, 2, 29, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8097), 900.0, new DateTime(2024, 2, 28, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8095) },
                    { 10, 10, new DateTime(2024, 3, 1, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8100), 1000.0, new DateTime(2024, 2, 29, 17, 37, 30, 973, DateTimeKind.Local).AddTicks(8099) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "BirthDate", "Email", "FirstName", "InvalidLoginAttempts", "IsLocked", "LastLogin", "LastName", "PasswordHash", "PasswordSalt", "RoleId" },
                values: new object[,]
                {
                    { 1, new DateTime(2004, 2, 21, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(552), "email1@mail.com", "FirstName1", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(587), "LastName1", new byte[] { 28, 48, 216, 147, 16, 3, 75, 214, 247, 39, 200, 79, 125, 138, 182, 10, 207, 124, 163, 136, 172, 0, 234, 53, 55, 240, 83, 70, 92, 44, 52, 88, 240, 162, 212, 140, 194, 187, 214, 175, 204, 9, 7, 28, 156, 234, 145, 54, 20, 33, 147, 37, 182, 27, 236, 83, 73, 193, 81, 9, 171, 71, 115, 0 }, new byte[] { 44, 51, 185, 120, 71, 154, 238, 72, 137, 38, 189, 240, 76, 6, 43, 209, 192, 122, 235, 115, 104, 5, 85, 140, 245, 227, 140, 102, 216, 238, 253, 141, 157, 19, 220, 87, 96, 124, 157, 141, 252, 140, 164, 226, 88, 32, 18, 214, 222, 117, 75, 200, 157, 6, 92, 207, 33, 74, 150, 186, 146, 205, 18, 20, 34, 95, 27, 165, 150, 36, 73, 54, 13, 83, 173, 182, 152, 82, 188, 40, 254, 124, 242, 233, 79, 150, 8, 1, 230, 78, 252, 206, 161, 183, 67, 240, 7, 132, 43, 153, 209, 10, 92, 41, 190, 234, 90, 166, 171, 228, 36, 97, 25, 225, 3, 103, 252, 247, 202, 2, 70, 253, 46, 78, 181, 218, 104, 61 }, 2 },
                    { 2, new DateTime(2004, 2, 22, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(913), "email2@mail.com", "FirstName2", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(917), "LastName2", new byte[] { 77, 20, 147, 105, 199, 159, 190, 169, 124, 73, 196, 57, 104, 123, 221, 229, 234, 44, 117, 175, 172, 54, 72, 206, 164, 234, 143, 19, 225, 212, 29, 156, 213, 209, 42, 254, 246, 201, 86, 150, 170, 177, 40, 105, 173, 253, 67, 22, 134, 5, 41, 60, 174, 54, 146, 68, 118, 36, 126, 152, 9, 248, 143, 24 }, new byte[] { 217, 182, 45, 125, 197, 163, 210, 179, 132, 31, 16, 34, 3, 141, 121, 183, 83, 45, 224, 213, 24, 192, 5, 225, 235, 111, 31, 4, 246, 161, 51, 21, 75, 84, 136, 84, 59, 2, 97, 79, 159, 115, 67, 25, 180, 20, 219, 38, 16, 96, 110, 42, 112, 194, 68, 70, 63, 19, 247, 54, 141, 233, 65, 18, 83, 74, 253, 252, 235, 193, 175, 6, 184, 118, 204, 182, 35, 66, 144, 14, 158, 174, 240, 51, 15, 124, 51, 165, 46, 131, 126, 94, 64, 20, 219, 85, 131, 157, 103, 41, 160, 29, 141, 5, 102, 164, 164, 112, 197, 181, 55, 142, 4, 136, 11, 191, 213, 97, 71, 194, 253, 94, 235, 191, 182, 223, 222, 168 }, 1 },
                    { 3, new DateTime(2004, 2, 23, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(968), "email3@mail.com", "FirstName3", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(970), "LastName3", new byte[] { 183, 255, 98, 236, 150, 115, 125, 65, 83, 85, 46, 62, 231, 107, 140, 48, 107, 65, 33, 38, 216, 141, 53, 65, 246, 73, 223, 86, 94, 210, 137, 206, 136, 164, 192, 37, 67, 231, 143, 97, 168, 237, 154, 124, 106, 30, 35, 46, 2, 129, 202, 160, 92, 149, 231, 161, 80, 122, 197, 232, 173, 141, 189, 117 }, new byte[] { 148, 197, 101, 147, 215, 42, 248, 132, 59, 62, 61, 114, 105, 9, 4, 223, 252, 4, 224, 222, 97, 204, 227, 8, 218, 97, 99, 145, 190, 101, 20, 178, 175, 45, 10, 236, 30, 132, 77, 134, 161, 153, 185, 161, 54, 12, 82, 249, 91, 182, 230, 249, 248, 155, 221, 154, 55, 194, 17, 107, 66, 110, 109, 84, 130, 109, 73, 18, 214, 250, 211, 147, 71, 24, 255, 119, 157, 95, 214, 194, 235, 68, 60, 73, 57, 14, 156, 233, 85, 0, 146, 246, 27, 98, 1, 110, 135, 187, 20, 76, 29, 108, 253, 231, 245, 44, 229, 226, 136, 116, 0, 62, 20, 71, 98, 242, 175, 143, 69, 90, 218, 80, 47, 44, 212, 169, 1, 188 }, 2 },
                    { 4, new DateTime(2004, 2, 24, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1014), "email4@mail.com", "FirstName4", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1017), "LastName4", new byte[] { 84, 171, 242, 130, 240, 160, 206, 104, 7, 252, 19, 156, 231, 192, 223, 226, 121, 139, 110, 170, 72, 76, 81, 32, 56, 44, 189, 45, 7, 239, 172, 100, 151, 156, 204, 23, 47, 65, 59, 135, 146, 118, 31, 78, 59, 126, 49, 69, 222, 137, 65, 202, 206, 87, 188, 9, 0, 244, 20, 29, 122, 236, 231, 120 }, new byte[] { 181, 191, 42, 220, 245, 154, 133, 166, 215, 54, 116, 26, 55, 34, 15, 74, 164, 60, 41, 80, 38, 240, 224, 186, 120, 103, 188, 72, 46, 45, 73, 132, 202, 152, 255, 7, 170, 5, 9, 79, 11, 47, 240, 82, 217, 180, 194, 154, 44, 155, 81, 48, 207, 68, 250, 206, 221, 176, 61, 158, 211, 152, 55, 43, 118, 173, 90, 137, 154, 16, 196, 236, 11, 83, 94, 55, 105, 100, 96, 170, 220, 173, 32, 200, 10, 231, 114, 120, 178, 55, 179, 95, 223, 124, 99, 49, 101, 125, 165, 250, 90, 82, 14, 127, 97, 197, 114, 27, 129, 150, 159, 133, 82, 100, 51, 235, 24, 182, 213, 242, 131, 156, 244, 175, 158, 151, 171, 201 }, 1 },
                    { 5, new DateTime(2004, 2, 25, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1097), "email5@mail.com", "FirstName5", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1100), "LastName5", new byte[] { 34, 117, 45, 130, 57, 93, 227, 218, 152, 111, 166, 147, 90, 49, 89, 118, 70, 119, 196, 57, 64, 136, 13, 225, 251, 10, 212, 150, 54, 136, 249, 209, 42, 203, 187, 31, 57, 223, 152, 39, 110, 192, 133, 208, 180, 70, 220, 175, 82, 227, 70, 153, 20, 107, 89, 226, 215, 157, 85, 194, 26, 149, 190, 28 }, new byte[] { 56, 244, 179, 166, 254, 98, 58, 74, 2, 111, 189, 2, 222, 107, 219, 15, 69, 119, 190, 33, 161, 46, 17, 10, 171, 150, 254, 154, 146, 177, 78, 121, 238, 42, 232, 118, 111, 11, 117, 74, 157, 157, 27, 9, 230, 198, 225, 185, 72, 233, 26, 118, 129, 116, 13, 216, 207, 48, 232, 174, 74, 239, 192, 179, 92, 7, 134, 3, 142, 56, 177, 120, 33, 97, 248, 176, 88, 68, 199, 0, 203, 188, 201, 144, 178, 223, 33, 190, 159, 241, 148, 39, 89, 150, 88, 114, 152, 201, 39, 119, 193, 37, 236, 188, 237, 182, 21, 223, 7, 17, 119, 175, 213, 173, 205, 70, 80, 143, 96, 205, 8, 127, 79, 200, 224, 89, 9, 76 }, 2 },
                    { 6, new DateTime(2004, 2, 26, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1148), "email6@mail.com", "FirstName6", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1151), "LastName6", new byte[] { 231, 146, 11, 194, 85, 204, 229, 250, 100, 74, 159, 232, 206, 23, 130, 191, 30, 158, 153, 116, 30, 147, 123, 113, 110, 244, 97, 200, 49, 209, 225, 133, 159, 158, 87, 163, 8, 110, 125, 150, 210, 241, 67, 81, 247, 133, 75, 227, 19, 105, 114, 131, 164, 160, 74, 86, 157, 156, 97, 43, 137, 35, 42, 34 }, new byte[] { 14, 67, 76, 244, 76, 77, 98, 138, 232, 58, 254, 86, 129, 148, 147, 188, 49, 38, 47, 129, 131, 240, 89, 26, 247, 68, 34, 244, 73, 154, 195, 163, 168, 47, 106, 75, 239, 222, 58, 9, 0, 250, 153, 163, 238, 42, 183, 12, 7, 224, 164, 82, 232, 92, 74, 40, 62, 10, 31, 34, 161, 206, 169, 146, 248, 65, 231, 240, 211, 250, 154, 133, 24, 146, 175, 141, 49, 14, 151, 6, 233, 120, 160, 162, 130, 71, 211, 178, 166, 101, 46, 20, 19, 14, 178, 94, 191, 215, 30, 12, 81, 185, 174, 230, 227, 223, 44, 169, 207, 216, 79, 60, 251, 241, 60, 194, 101, 154, 206, 202, 143, 182, 25, 247, 175, 39, 72, 93 }, 1 },
                    { 7, new DateTime(2004, 2, 27, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1193), "email7@mail.com", "FirstName7", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1195), "LastName7", new byte[] { 207, 119, 70, 224, 67, 59, 21, 79, 117, 53, 44, 153, 186, 23, 11, 10, 177, 88, 58, 104, 246, 243, 202, 29, 79, 182, 245, 31, 177, 34, 165, 195, 5, 122, 137, 55, 46, 29, 182, 219, 0, 97, 39, 119, 208, 73, 212, 184, 10, 184, 196, 40, 252, 170, 150, 139, 105, 179, 111, 34, 81, 119, 183, 129 }, new byte[] { 232, 95, 198, 214, 214, 162, 149, 13, 112, 179, 240, 229, 62, 41, 85, 70, 235, 157, 187, 222, 167, 220, 21, 94, 67, 84, 45, 93, 214, 185, 111, 143, 211, 86, 7, 173, 241, 193, 220, 158, 20, 15, 250, 62, 190, 51, 215, 20, 68, 156, 184, 175, 253, 218, 87, 9, 80, 56, 129, 123, 183, 254, 181, 42, 111, 115, 180, 235, 40, 207, 103, 229, 4, 132, 251, 2, 226, 186, 245, 220, 228, 66, 137, 138, 227, 248, 86, 90, 184, 27, 144, 149, 208, 54, 74, 33, 90, 5, 204, 39, 55, 194, 95, 228, 119, 39, 17, 16, 44, 66, 135, 5, 198, 246, 224, 41, 188, 162, 214, 172, 117, 67, 145, 157, 10, 98, 14, 114 }, 2 },
                    { 8, new DateTime(2004, 2, 28, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1237), "email8@mail.com", "FirstName8", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1240), "LastName8", new byte[] { 109, 182, 215, 65, 153, 63, 12, 176, 154, 66, 118, 1, 232, 16, 64, 14, 81, 132, 86, 151, 144, 24, 216, 244, 184, 34, 221, 99, 105, 146, 25, 26, 15, 172, 214, 19, 174, 216, 172, 123, 176, 164, 37, 102, 11, 183, 18, 104, 120, 167, 205, 61, 137, 183, 206, 48, 165, 175, 205, 49, 123, 43, 104, 145 }, new byte[] { 152, 150, 29, 123, 142, 126, 213, 61, 1, 124, 84, 243, 3, 25, 73, 22, 125, 58, 196, 178, 193, 148, 235, 178, 237, 188, 248, 95, 18, 145, 106, 181, 130, 225, 100, 195, 237, 87, 199, 4, 158, 55, 19, 15, 132, 158, 105, 0, 93, 202, 67, 224, 252, 179, 134, 111, 114, 8, 74, 197, 84, 95, 75, 171, 174, 249, 144, 10, 117, 187, 191, 69, 2, 17, 173, 152, 17, 119, 221, 30, 7, 2, 191, 223, 78, 157, 96, 174, 71, 194, 253, 72, 195, 241, 96, 114, 12, 163, 182, 110, 203, 254, 56, 153, 203, 217, 111, 57, 200, 216, 232, 23, 159, 42, 187, 162, 138, 93, 53, 79, 254, 124, 142, 184, 108, 249, 78, 12 }, 1 },
                    { 9, new DateTime(2004, 2, 29, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1281), "email9@mail.com", "FirstName9", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1283), "LastName9", new byte[] { 237, 137, 79, 61, 179, 178, 225, 196, 233, 201, 163, 9, 254, 186, 118, 16, 2, 31, 251, 191, 119, 2, 190, 161, 8, 86, 4, 43, 150, 107, 202, 81, 52, 97, 63, 64, 137, 97, 146, 231, 89, 194, 46, 240, 55, 6, 114, 85, 54, 20, 62, 23, 168, 167, 49, 148, 3, 125, 217, 190, 211, 87, 123, 33 }, new byte[] { 79, 122, 55, 246, 54, 193, 119, 129, 44, 126, 81, 30, 86, 116, 89, 223, 238, 187, 74, 99, 11, 218, 121, 208, 128, 153, 174, 0, 126, 163, 120, 16, 22, 45, 92, 33, 114, 217, 123, 86, 231, 135, 59, 46, 24, 225, 217, 103, 197, 199, 61, 105, 179, 101, 84, 175, 177, 163, 72, 99, 35, 175, 14, 218, 98, 43, 171, 77, 241, 39, 220, 244, 33, 100, 251, 107, 199, 36, 114, 147, 192, 5, 210, 17, 241, 60, 9, 57, 119, 42, 6, 190, 155, 210, 74, 128, 146, 122, 219, 165, 204, 20, 100, 249, 203, 55, 251, 100, 95, 192, 73, 240, 244, 75, 201, 36, 105, 188, 246, 99, 184, 178, 29, 188, 199, 169, 200, 192 }, 2 },
                    { 10, new DateTime(2004, 3, 1, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1325), "email10@mail.com", "FirstName10", 0, false, new DateTime(2024, 2, 20, 17, 37, 30, 975, DateTimeKind.Local).AddTicks(1328), "LastName10", new byte[] { 84, 8, 234, 228, 176, 125, 110, 198, 39, 0, 240, 18, 85, 242, 180, 124, 120, 223, 217, 216, 161, 125, 229, 2, 229, 223, 186, 69, 156, 38, 202, 130, 188, 150, 70, 84, 243, 35, 93, 39, 82, 86, 89, 96, 150, 77, 79, 78, 240, 142, 128, 186, 28, 96, 124, 156, 40, 98, 141, 64, 181, 142, 54, 104 }, new byte[] { 55, 187, 100, 113, 182, 109, 29, 233, 59, 201, 158, 131, 102, 224, 199, 30, 76, 3, 142, 207, 249, 204, 169, 77, 127, 167, 225, 24, 152, 93, 190, 39, 187, 225, 103, 172, 94, 169, 173, 122, 51, 107, 117, 3, 217, 133, 179, 100, 181, 207, 8, 46, 73, 105, 226, 123, 96, 125, 11, 221, 247, 148, 66, 179, 9, 80, 185, 133, 100, 5, 150, 169, 152, 12, 190, 232, 244, 97, 76, 193, 248, 74, 25, 169, 185, 207, 144, 132, 217, 134, 90, 80, 254, 60, 254, 39, 100, 186, 239, 200, 139, 35, 184, 141, 216, 137, 98, 149, 218, 119, 151, 222, 77, 77, 114, 214, 60, 48, 94, 179, 78, 193, 112, 32, 164, 54, 164, 118 }, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_AnimalId",
                table: "Shifts",
                column: "AnimalId");

            migrationBuilder.CreateIndex(
                name: "IX_Users_RoleId",
                table: "Users",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Shifts");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Animals");

            migrationBuilder.DropTable(
                name: "Keepers");

            migrationBuilder.DropTable(
                name: "user_roles");
        }
    }
}
