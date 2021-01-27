using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ScouTeams.Migrations
{
    public partial class Initialcreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "KwateraGlowna",
                columns: table => new
                {
                    KwateraGlownaId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_KwateraGlowna", x => x.KwateraGlownaId);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    DateOfBirth = table.Column<DateTime>(nullable: false),
                    KwateraGlownaId = table.Column<int>(nullable: true),
                    ScoutDegree = table.Column<int>(nullable: false),
                    InstructorDegree = table.Column<int>(nullable: false),
                    Recruitment = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUsers_KwateraGlowna_KwateraGlownaId",
                        column: x => x.KwateraGlownaId,
                        principalTable: "KwateraGlowna",
                        principalColumn: "KwateraGlownaId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Choragiews",
                columns: table => new
                {
                    ChoragiewId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    KwateraGlownaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Choragiews", x => x.ChoragiewId);
                    table.ForeignKey(
                        name: "FK_Choragiews_KwateraGlowna_KwateraGlownaId",
                        column: x => x.KwateraGlownaId,
                        principalTable: "KwateraGlowna",
                        principalColumn: "KwateraGlownaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
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
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
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
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(maxLength: 128, nullable: false),
                    Name = table.Column<string>(maxLength: 128, nullable: false),
                    Value = table.Column<string>(nullable: true)
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
                name: "Contributions",
                columns: table => new
                {
                    ContributionId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScoutId = table.Column<string>(nullable: true),
                    Amount = table.Column<float>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contributions", x => x.ContributionId);
                    table.ForeignKey(
                        name: "FK_Contributions_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "FunctionInOrganizations",
                columns: table => new
                {
                    FunctionInOrganizationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScoutId = table.Column<string>(nullable: true),
                    FunctionName = table.Column<int>(nullable: false),
                    ChorągiewId = table.Column<int>(nullable: false),
                    HufiecId = table.Column<int>(nullable: false),
                    DruzynaId = table.Column<int>(nullable: false),
                    ZastepId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FunctionInOrganizations", x => x.FunctionInOrganizationId);
                    table.ForeignKey(
                        name: "FK_FunctionInOrganizations_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Meetings",
                columns: table => new
                {
                    MeetingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZastepId = table.Column<int>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false),
                    ScoutId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Meetings", x => x.MeetingId);
                    table.ForeignKey(
                        name: "FK_Meetings_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    SkillId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Difficulty = table.Column<int>(nullable: false),
                    ScoutId = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.SkillId);
                    table.ForeignKey(
                        name: "FK_Skills_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Hufiecs",
                columns: table => new
                {
                    HufiecId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    ChoragiewId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hufiecs", x => x.HufiecId);
                    table.ForeignKey(
                        name: "FK_Hufiecs_Choragiews_ChoragiewId",
                        column: x => x.ChoragiewId,
                        principalTable: "Choragiews",
                        principalColumn: "ChoragiewId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserChoragiew",
                columns: table => new
                {
                    ScoutId = table.Column<string>(nullable: false),
                    ChoragiewId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserChoragiew", x => new { x.ScoutId, x.ChoragiewId });
                    table.ForeignKey(
                        name: "FK_UserChoragiew_Choragiews_ChoragiewId",
                        column: x => x.ChoragiewId,
                        principalTable: "Choragiews",
                        principalColumn: "ChoragiewId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserChoragiew_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ScoutPresence",
                columns: table => new
                {
                    ScoutPresenceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ScoutId = table.Column<string>(nullable: true),
                    MeetingId = table.Column<int>(nullable: false),
                    Presence = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoutPresence", x => x.ScoutPresenceId);
                    table.ForeignKey(
                        name: "FK_ScoutPresence_Meetings_MeetingId",
                        column: x => x.MeetingId,
                        principalTable: "Meetings",
                        principalColumn: "MeetingId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
               name: "Druzynas",
               columns: table => new
               {
                   DruzynaId = table.Column<int>(nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   Name = table.Column<string>(nullable: true),
                   HufiecId = table.Column<int>(nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Druzynas", x => x.DruzynaId);
                   table.ForeignKey(
                       name: "FK_Druzynas_Hufiecs_HufiecId",
                       column: x => x.HufiecId,
                       principalTable: "Hufiecs",
                       principalColumn: "HufiecId",
                       onDelete: ReferentialAction.Cascade);
               });

            migrationBuilder.CreateTable(
                name: "UserHufiec",
                columns: table => new
                {
                    ScoutId = table.Column<string>(nullable: false),
                    HufiecId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHufiec", x => new { x.ScoutId, x.HufiecId });
                    table.ForeignKey(
                        name: "FK_UserHufiec_Hufiecs_HufiecId",
                        column: x => x.HufiecId,
                        principalTable: "Hufiecs",
                        principalColumn: "HufiecId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserHufiec_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserDruzyna",
                columns: table => new
                {
                    ScoutId = table.Column<string>(nullable: false),
                    DruzynaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDruzyna", x => new { x.ScoutId, x.DruzynaId });
                    table.ForeignKey(
                        name: "FK_UserDruzyna_Druzynas_DruzynaId",
                        column: x => x.DruzynaId,
                        principalTable: "Druzynas",
                        principalColumn: "DruzynaId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDruzyna_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Zastep",
                columns: table => new
                {
                    ZastepId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    DruzynaId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Zastep", x => x.ZastepId);
                    table.ForeignKey(
                        name: "FK_Zastep_Druzynas_DruzynaId",
                        column: x => x.DruzynaId,
                        principalTable: "Druzynas",
                        principalColumn: "DruzynaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserZastep",
                columns: table => new
                {
                    ScoutId = table.Column<string>(nullable: false),
                    ZastepId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserZastep", x => new { x.ScoutId, x.ZastepId });
                    table.ForeignKey(
                        name: "FK_UserZastep_AspNetUsers_ScoutId",
                        column: x => x.ScoutId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserZastep_Zastep_ZastepId",
                        column: x => x.ZastepId,
                        principalTable: "Zastep",
                        principalColumn: "ZastepId",
                        onDelete: ReferentialAction.Cascade);
                });

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
                name: "IX_AspNetUsers_KwateraGlownaId",
                table: "AspNetUsers",
                column: "KwateraGlownaId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Choragiews_KwateraGlownaId",
                table: "Choragiews",
                column: "KwateraGlownaId");

            migrationBuilder.CreateIndex(
                name: "IX_Contributions_ScoutId",
                table: "Contributions",
                column: "ScoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Druzynas_DruzynaId",
                table: "Druzynas",
                column: "DruzynaId");

            migrationBuilder.CreateIndex(
                name: "IX_FunctionInOrganizations_ScoutId",
                table: "FunctionInOrganizations",
                column: "ScoutId");

            migrationBuilder.CreateIndex(
                name: "IX_Hufiecs_ChoragiewId",
                table: "Hufiecs",
                column: "ChoragiewId");

            migrationBuilder.CreateIndex(
                name: "IX_Meetings_ScoutId",
                table: "Meetings",
                column: "ScoutId");

            migrationBuilder.CreateIndex(
                name: "IX_ScoutPresence_MeetingId",
                table: "ScoutPresence",
                column: "MeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_Skills_ScoutId",
                table: "Skills",
                column: "ScoutId");

            migrationBuilder.CreateIndex(
                name: "IX_UserChoragiew_ChoragiewId",
                table: "UserChoragiew",
                column: "ChoragiewId");

            migrationBuilder.CreateIndex(
                name: "IX_UserDruzyna_DruzynaId",
                table: "UserDruzyna",
                column: "DruzynaId");

            migrationBuilder.CreateIndex(
                name: "IX_UserHufiec_HufiecId",
                table: "UserHufiec",
                column: "HufiecId");

            migrationBuilder.CreateIndex(
                name: "IX_UserZastep_ZastepId",
                table: "UserZastep",
                column: "ZastepId");

            migrationBuilder.CreateIndex(
                name: "IX_Zastep_DruzynaId",
                table: "Zastep",
                column: "DruzynaId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
                name: "Contributions");

            migrationBuilder.DropTable(
                name: "FunctionInOrganizations");

            migrationBuilder.DropTable(
                name: "ScoutPresence");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "UserChoragiew");

            migrationBuilder.DropTable(
                name: "UserDruzyna");

            migrationBuilder.DropTable(
                name: "UserHufiec");

            migrationBuilder.DropTable(
                name: "UserZastep");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Meetings");

            migrationBuilder.DropTable(
                name: "Zastep");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "Druzynas");

            migrationBuilder.DropTable(
                name: "Hufiecs");

            migrationBuilder.DropTable(
                name: "Choragiews");

            migrationBuilder.DropTable(
                name: "KwateraGlowna");
        }
    }
}
