using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ward27EmploymentRegistry.Migrations
{
    /// <inheritdoc />
    public partial class InitialUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetUserRoleLinks");

            migrationBuilder.AddColumn<int>(
                name: "Gender",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gender",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "AspNetUserRoleLinks",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoleLinks", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoleLinks_AspNetRoles_RoleId1",
                        column: x => x.RoleId1,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_AspNetUserRoleLinks_AspNetUserRoles_UserId_RoleId",
                        columns: x => new { x.UserId, x.RoleId },
                        principalTable: "AspNetUserRoles",
                        principalColumns: new[] { "UserId", "RoleId" },
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoleLinks_RoleId",
                table: "AspNetUserRoleLinks",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoleLinks_RoleId1",
                table: "AspNetUserRoleLinks",
                column: "RoleId1");
        }
    }
}
