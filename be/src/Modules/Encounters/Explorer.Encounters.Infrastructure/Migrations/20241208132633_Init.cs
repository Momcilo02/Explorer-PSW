using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Explorer.Encounters.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "encounters");

            migrationBuilder.CreateTable(
                name: "EncounterExecution",
                schema: "encounters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    EncounterId = table.Column<int>(type: "integer", nullable: false),
                    TouristId = table.Column<int>(type: "integer", nullable: false),
                    TouristLongitude = table.Column<double>(type: "double precision", nullable: false),
                    TouristLatitude = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    NumberOfActiveTourists = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EncounterExecution", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Encounters",
                schema: "encounters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    TotalXp = table.Column<int>(type: "integer", nullable: false),
                    CreatorId = table.Column<int>(type: "integer", nullable: false),
                    Longitude = table.Column<double>(type: "double precision", nullable: false),
                    Latitude = table.Column<double>(type: "double precision", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EncounterType = table.Column<int>(type: "integer", nullable: false),
                    TouristRequestStatus = table.Column<int>(type: "integer", nullable: true),
                    isTourRequired = table.Column<bool>(type: "boolean", nullable: true),
                    TourId = table.Column<int>(type: "integer", nullable: true),
                    ActivateRange = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Encounters", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HiddenLocationEncounters",
                schema: "encounters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Image = table.Column<string>(type: "text", nullable: false),
                    ImageLongitude = table.Column<double>(type: "double precision", nullable: false),
                    ImageLatitude = table.Column<double>(type: "double precision", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HiddenLocationEncounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HiddenLocationEncounters_Encounters_Id",
                        column: x => x.Id,
                        principalSchema: "encounters",
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MiscEncounters",
                schema: "encounters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Instructions = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MiscEncounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MiscEncounters_Encounters_Id",
                        column: x => x.Id,
                        principalSchema: "encounters",
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialEncounters",
                schema: "encounters",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PeopleNumb = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialEncounters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialEncounters_Encounters_Id",
                        column: x => x.Id,
                        principalSchema: "encounters",
                        principalTable: "Encounters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EncounterExecution",
                schema: "encounters");

            migrationBuilder.DropTable(
                name: "HiddenLocationEncounters",
                schema: "encounters");

            migrationBuilder.DropTable(
                name: "MiscEncounters",
                schema: "encounters");

            migrationBuilder.DropTable(
                name: "SocialEncounters",
                schema: "encounters");

            migrationBuilder.DropTable(
                name: "Encounters",
                schema: "encounters");
        }
    }
}
