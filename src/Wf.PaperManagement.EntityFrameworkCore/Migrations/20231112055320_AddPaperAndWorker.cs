using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Wf.PaperManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddPaperAndWorker : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PmWorkers",
                columns: table => new
                {
                    WorkerId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmWorkers", x => x.WorkerId);
                });

            migrationBuilder.CreateTable(
                name: "PmPapers",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Address = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    ProblemType = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: false),
                    ProblemDescription = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Solution = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    ReceiverId = table.Column<int>(type: "integer", nullable: false),
                    WorkerId = table.Column<int>(type: "integer", nullable: true),
                    Worker2Id = table.Column<int>(type: "integer", nullable: true),
                    ReceiveTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CompleteTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Note = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    ExtraProperties = table.Column<string>(type: "text", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    DeleterId = table.Column<Guid>(type: "uuid", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PmPapers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PmPapers_PmWorkers_ReceiverId",
                        column: x => x.ReceiverId,
                        principalTable: "PmWorkers",
                        principalColumn: "WorkerId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PmPapers_PmWorkers_Worker2Id",
                        column: x => x.Worker2Id,
                        principalTable: "PmWorkers",
                        principalColumn: "WorkerId");
                    table.ForeignKey(
                        name: "FK_PmPapers_PmWorkers_WorkerId",
                        column: x => x.WorkerId,
                        principalTable: "PmWorkers",
                        principalColumn: "WorkerId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_PmPapers_ReceiverId",
                table: "PmPapers",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_PmPapers_Worker2Id",
                table: "PmPapers",
                column: "Worker2Id");

            migrationBuilder.CreateIndex(
                name: "IX_PmPapers_WorkerId",
                table: "PmPapers",
                column: "WorkerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PmPapers");

            migrationBuilder.DropTable(
                name: "PmWorkers");
        }
    }
}
