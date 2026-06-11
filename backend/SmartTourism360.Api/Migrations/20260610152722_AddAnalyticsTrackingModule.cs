using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartTourism360.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAnalyticsTrackingModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "analytics_events",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventName = table.Column<string>(type: "text", nullable: false),
                    TargetType = table.Column<string>(type: "text", nullable: true),
                    TargetId = table.Column<Guid>(type: "uuid", nullable: true),
                    SessionId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    Metadata = table.Column<string>(type: "jsonb", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_analytics_events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_analytics_events_users_UserId",
                        column: x => x.UserId,
                        principalTable: "users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_analytics_events_CreatedAt",
                table: "analytics_events",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_analytics_events_EventName",
                table: "analytics_events",
                column: "EventName");

            migrationBuilder.CreateIndex(
                name: "IX_analytics_events_SessionId",
                table: "analytics_events",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_analytics_events_TargetType_TargetId",
                table: "analytics_events",
                columns: new[] { "TargetType", "TargetId" });

            migrationBuilder.CreateIndex(
                name: "IX_analytics_events_UserId",
                table: "analytics_events",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "analytics_events");
        }
    }
}
