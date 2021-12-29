using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace NSG.NetIncident4.Core.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Companies",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyShortName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    State = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Companies", x => x.CompanyId);
                });

            migrationBuilder.CreateTable(
                name: "IncidentType",
                columns: table => new
                {
                    IncidentTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IncidentTypeShortDesc = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    IncidentTypeDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    IncidentTypeFromServer = table.Column<bool>(type: "bit", nullable: false),
                    IncidentTypeSubjectLine = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    IncidentTypeEmailTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    IncidentTypeTimeTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    IncidentTypeThanksTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    IncidentTypeLogTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    IncidentTypeTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentType", x => x.IncidentTypeId);
                });

            migrationBuilder.CreateTable(
                name: "Log",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Application = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Method = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    LogLevel = table.Column<byte>(type: "tinyint", nullable: false),
                    Level = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    UserAccount = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Message = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: false),
                    Exception = table.Column<string>(type: "nvarchar(4000)", maxLength: 4000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Log", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NIC",
                columns: table => new
                {
                    NIC_Id = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    NICDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    NICAbuseEmailAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NICRestService = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    NICWebSite = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NIC", x => x.NIC_Id);
                });

            migrationBuilder.CreateTable(
                name: "NoteType",
                columns: table => new
                {
                    NoteTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteTypeShortDesc = table.Column<string>(type: "nvarchar(8)", maxLength: 8, nullable: false),
                    NoteTypeDesc = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NoteTypeClientScript = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NoteType", x => x.NoteTypeId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserNicName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Servers",
                columns: table => new
                {
                    ServerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    ServerShortName = table.Column<string>(type: "nvarchar(12)", maxLength: 12, nullable: false),
                    ServerName = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    ServerDescription = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    WebSite = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    ServerLocation = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FromName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FromNicName = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    FromEmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    DST = table.Column<bool>(type: "bit", nullable: false),
                    TimeZone_DST = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: true),
                    DST_Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DST_End = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Servers", x => x.ServerId);
                    table.ForeignKey(
                        name: "FK_Servers_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EmailTemplates",
                columns: table => new
                {
                    CompanyId = table.Column<int>(type: "int", nullable: false),
                    IncidentTypeId = table.Column<int>(type: "int", nullable: false),
                    SubjectLine = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    EmailBody = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    TimeTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    ThanksTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    LogTemplate = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    Template = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    FromServer = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailTemplates", x => new { x.CompanyId, x.IncidentTypeId });
                    table.ForeignKey(
                        name: "FK_EmailTemplates_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "CompanyId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmailTemplates_IncidentType_IncidentTypeId",
                        column: x => x.IncidentTypeId,
                        principalTable: "IncidentType",
                        principalColumn: "IncidentTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentNote",
                columns: table => new
                {
                    IncidentNoteId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NoteTypeId = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentNote", x => x.IncidentNoteId);
                    table.ForeignKey(
                        name: "FK_IncidentNote_NoteType_NoteTypeId",
                        column: x => x.NoteTypeId,
                        principalTable: "NoteType",
                        principalColumn: "NoteTypeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ApplicationUserApplicationServer",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", maxLength: 450, nullable: false),
                    ServerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApplicationUserApplicationServer", x => new { x.Id, x.ServerId });
                    table.ForeignKey(
                        name: "FK_ApplicationUserApplicationServer_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ApplicationUserApplicationServer_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Incident",
                columns: table => new
                {
                    IncidentId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NIC_Id = table.Column<string>(type: "nvarchar(16)", maxLength: 16, nullable: false),
                    NetworkName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    AbuseEmailAddress = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ISPTicketNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Mailed = table.Column<bool>(type: "bit", nullable: false),
                    Closed = table.Column<bool>(type: "bit", nullable: false),
                    Special = table.Column<bool>(type: "bit", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Incident", x => x.IncidentId);
                    table.ForeignKey(
                        name: "FK_Incident_NIC_NIC_Id",
                        column: x => x.NIC_Id,
                        principalTable: "NIC",
                        principalColumn: "NIC_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Incident_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "IncidentIncidentNotes",
                columns: table => new
                {
                    IncidentId = table.Column<long>(type: "bigint", nullable: false),
                    IncidentNoteId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IncidentIncidentNotes", x => new { x.IncidentId, x.IncidentNoteId });
                    table.ForeignKey(
                        name: "FK_IncidentIncidentNotes_Incident_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incident",
                        principalColumn: "IncidentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_IncidentIncidentNotes_IncidentNote_IncidentNoteId",
                        column: x => x.IncidentNoteId,
                        principalTable: "IncidentNote",
                        principalColumn: "IncidentNoteId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NetworkLog",
                columns: table => new
                {
                    NetworkLogId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerId = table.Column<int>(type: "int", nullable: false),
                    IncidentId = table.Column<long>(type: "bigint", nullable: true),
                    IPAddress = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    NetworkLogDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Log = table.Column<string>(type: "nvarchar(max)", maxLength: 1073741823, nullable: false),
                    IncidentTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NetworkLog", x => x.NetworkLogId);
                    table.ForeignKey(
                        name: "FK_NetworkLog_Incident_IncidentId",
                        column: x => x.IncidentId,
                        principalTable: "Incident",
                        principalColumn: "IncidentId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NetworkLog_IncidentType_IncidentTypeId",
                        column: x => x.IncidentTypeId,
                        principalTable: "IncidentType",
                        principalColumn: "IncidentTypeId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NetworkLog_Servers_ServerId",
                        column: x => x.ServerId,
                        principalTable: "Servers",
                        principalColumn: "ServerId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ApplicationUserApplicationServer_ServerId",
                table: "ApplicationUserApplicationServer",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CompanyId",
                table: "AspNetUsers",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "Idx_Companies_ShortName",
                table: "Companies",
                column: "CompanyShortName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailTemplates_IncidentTypeId",
                table: "EmailTemplates",
                column: "IncidentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_NIC_Id",
                table: "Incident",
                column: "NIC_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Incident_ServerId",
                table: "Incident",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentIncidentNotes_IncidentNoteId",
                table: "IncidentIncidentNotes",
                column: "IncidentNoteId");

            migrationBuilder.CreateIndex(
                name: "IX_IncidentNote_NoteTypeId",
                table: "IncidentNote",
                column: "NoteTypeId");

            migrationBuilder.CreateIndex(
                name: "Idx_IncidentType_ShortDesc",
                table: "IncidentType",
                column: "IncidentTypeShortDesc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_NetworkLog_IncidentId",
                table: "NetworkLog",
                column: "IncidentId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkLog_IncidentTypeId",
                table: "NetworkLog",
                column: "IncidentTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_NetworkLog_ServerId",
                table: "NetworkLog",
                column: "ServerId");

            migrationBuilder.CreateIndex(
                name: "Idx_NoteType_ShortDesc",
                table: "NoteType",
                column: "NoteTypeShortDesc",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Idx_AspNetServers_ShortName",
                table: "Servers",
                column: "ServerShortName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Servers_CompanyId",
                table: "Servers",
                column: "CompanyId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApplicationUserApplicationServer");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "EmailTemplates");

            migrationBuilder.DropTable(
                name: "IncidentIncidentNotes");

            migrationBuilder.DropTable(
                name: "Log");

            migrationBuilder.DropTable(
                name: "NetworkLog");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "IncidentNote");

            migrationBuilder.DropTable(
                name: "Incident");

            migrationBuilder.DropTable(
                name: "IncidentType");

            migrationBuilder.DropTable(
                name: "NoteType");

            migrationBuilder.DropTable(
                name: "NIC");

            migrationBuilder.DropTable(
                name: "Servers");

            migrationBuilder.DropTable(
                name: "Companies");
        }
    }
}
