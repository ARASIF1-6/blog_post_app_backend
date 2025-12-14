using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPostApp.Migrations
{
    /// <inheritdoc />
    public partial class UserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "APP_USERS",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    EMAIL = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    PASSWORD = table.Column<string>(type: "text", nullable: false),
                    FULLNAME = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ROLE = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false, defaultValue: "User"),
                    CREATED_AT = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_APP_USERS", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_APP_USERS_EMAIL",
                table: "APP_USERS",
                column: "EMAIL",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "APP_USERS");
        }
    }
}
