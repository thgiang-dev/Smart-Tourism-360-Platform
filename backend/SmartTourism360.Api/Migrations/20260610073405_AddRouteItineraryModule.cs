using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartTourism360.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddRouteItineraryModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "routes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RegionId = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "text", nullable: false),
                    Slug = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true),
                    EstimatedDuration = table.Column<string>(type: "text", nullable: true),
                    DistanceKm = table.Column<decimal>(type: "numeric", nullable: true),
                    Theme = table.Column<string>(type: "text", nullable: true),
                    ThumbnailId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_routes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_routes_media_files_ThumbnailId",
                        column: x => x.ThumbnailId,
                        principalTable: "media_files",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_routes_regions_RegionId",
                        column: x => x.RegionId,
                        principalTable: "regions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "route_destinations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RouteId = table.Column<Guid>(type: "uuid", nullable: false),
                    DestinationId = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayOrder = table.Column<int>(type: "integer", nullable: false),
                    EstimatedStayTime = table.Column<string>(type: "text", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_route_destinations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_route_destinations_destinations_DestinationId",
                        column: x => x.DestinationId,
                        principalTable: "destinations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_route_destinations_routes_RouteId",
                        column: x => x.RouteId,
                        principalTable: "routes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_route_destinations_DestinationId",
                table: "route_destinations",
                column: "DestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_route_destinations_RouteId_DestinationId",
                table: "route_destinations",
                columns: new[] { "RouteId", "DestinationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_route_destinations_RouteId_DisplayOrder",
                table: "route_destinations",
                columns: new[] { "RouteId", "DisplayOrder" });

            migrationBuilder.CreateIndex(
                name: "IX_routes_RegionId_Slug",
                table: "routes",
                columns: new[] { "RegionId", "Slug" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_routes_Status",
                table: "routes",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_routes_Theme",
                table: "routes",
                column: "Theme");

            migrationBuilder.CreateIndex(
                name: "IX_routes_ThumbnailId",
                table: "routes",
                column: "ThumbnailId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "route_destinations");

            migrationBuilder.DropTable(
                name: "routes");
        }
    }
}
