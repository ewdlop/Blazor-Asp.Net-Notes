using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorServerApp.Migrations
{
    public partial class ChangeMemberShipKeyType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomerMembership");

            migrationBuilder.DropTable(
                name: "Membership");

            migrationBuilder.DropTable(
                name: "Customer");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Customer",
                columns: table => new
                {
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Customer", x => new { x.FirstName, x.LastName, x.EmailAddress });
                });

            migrationBuilder.CreateTable(
                name: "Membership",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    Fee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(25)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Membership", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CustomerMembership",
                columns: table => new
                {
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    EmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    MembershipId = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: false),
                    MemberSince = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerMembership", x => new { x.FirstName, x.LastName, x.EmailAddress, x.MembershipId });
                    table.ForeignKey(
                        name: "FK_CustomerMembership_Membership_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Membership",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CustomerMembership_Customer_FirstName_LastName_EmailAddress",
                        columns: x => new { x.FirstName, x.LastName, x.EmailAddress },
                        principalTable: "Customer",
                        principalColumns: new[] { "FirstName", "LastName", "EmailAddress" });
                });

            migrationBuilder.CreateIndex(
                name: "IX_CustomerMembership_MembershipId",
                table: "CustomerMembership",
                column: "MembershipId");
        }
    }
}
