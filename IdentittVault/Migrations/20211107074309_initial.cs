using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace IdentittVault.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Email = table.Column<string>(type: "varchar(767)", nullable: false),
                    HashPassword = table.Column<string>(type: "text", nullable: false),
                    PrivateKeyCrypt = table.Column<byte[]>(type: "varbinary(4000)", nullable: false),
                    PublicKeyPlain = table.Column<byte[]>(type: "varbinary(4000)", nullable: false),
                    PrivateKeyPlainHash = table.Column<string>(type: "varchar(767)", nullable: false),
                    PublicKeyPlainHash = table.Column<string>(type: "varchar(767)", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    UserId = table.Column<byte[]>(type: "varbinary(16)", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: false),
                    CypherPassword = table.Column<string>(type: "text", nullable: false),
                    Name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Account_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PrivateKeyPlainHash",
                table: "User",
                column: "PrivateKeyPlainHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_PublicKeyPlainHash",
                table: "User",
                column: "PublicKeyPlainHash",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
