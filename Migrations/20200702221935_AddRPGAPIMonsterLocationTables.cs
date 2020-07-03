using Microsoft.EntityFrameworkCore.Migrations;

namespace BlazorServerApp.Migrations
{
    public partial class AddRPGAPIMonsterLocationTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Membership",
                type: "nvarchar(25)",
                nullable: false,
                defaultValue: "Free",
                oldClrType: typeof(string),
                oldType: "nvarchar(25)");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Monsters",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(maxLength: 30, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(25)", nullable: false, defaultValue: "Creature"),
                    Rarity = table.Column<string>(type: "nvarchar(15)", nullable: false, defaultValue: "Normal"),
                    AttackType = table.Column<string>(type: "nvarchar(15)", nullable: false, defaultValue: "Melee")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Monsters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonsterResidencies",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    MonsterId = table.Column<int>(nullable: false),
                    LocationId = table.Column<int>(nullable: false),
                    RespawnTime = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonsterResidencies", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MonsterResidencies_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MonsterResidencies_Monsters_MonsterId",
                        column: x => x.MonsterId,
                        principalTable: "Monsters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MonsterResidencies_LocationId",
                table: "MonsterResidencies",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterResidencies_MonsterId",
                table: "MonsterResidencies",
                column: "MonsterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MonsterResidencies");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Monsters");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Membership",
                type: "nvarchar(25)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(25)",
                oldDefaultValue: "Free");
        }
    }
}
