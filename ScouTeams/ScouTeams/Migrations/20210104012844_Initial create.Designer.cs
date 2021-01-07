﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ScouTeams.Data;

namespace ScouTeams.Migrations
{
    [DbContext(typeof(ScouTDBContext))]
    [Migration("20210104012844_Initial create")]
    partial class Initialcreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(128)")
                        .HasMaxLength(128);

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("ScouTeams.Areas.Identity.Data.Scout", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InstructorDegree")
                        .HasColumnType("int");

                    b.Property<int?>("KwateraGlownaId")
                        .HasColumnType("int");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("Recruitment")
                        .HasColumnType("bit");

                    b.Property<int>("ScoutDegree")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(256)")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("KwateraGlownaId");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("ScouTeams.Models.Choragiew", b =>
                {
                    b.Property<int>("ChoragiewId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("KwateraGlownaId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ChoragiewId");

                    b.HasIndex("KwateraGlownaId");

                    b.ToTable("Choragiews");
                });

            modelBuilder.Entity("ScouTeams.Models.Contribution", b =>
                {
                    b.Property<int>("ContributionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<float>("Amount")
                        .HasColumnType("real");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("ContributionId");

                    b.HasIndex("ScoutId");

                    b.ToTable("Contributions");
                });

            modelBuilder.Entity("ScouTeams.Models.Druzyna", b =>
                {
                    b.Property<int>("HufiecId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DruzynaId")
                        .HasColumnType("int");

                    b.Property<int>("HufiecId1")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HufiecId");

                    b.HasIndex("HufiecId1");

                    b.ToTable("Druzynas");
                });

            modelBuilder.Entity("ScouTeams.Models.FunctionInOrganization", b =>
                {
                    b.Property<int>("FunctionInOrganizationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChorągiewId")
                        .HasColumnType("int");

                    b.Property<int>("DruzynaId")
                        .HasColumnType("int");

                    b.Property<int>("FunctionName")
                        .HasColumnType("int");

                    b.Property<int>("HufiecId")
                        .HasColumnType("int");

                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ZastepId")
                        .HasColumnType("int");

                    b.HasKey("FunctionInOrganizationId");

                    b.HasIndex("ScoutId");

                    b.ToTable("FunctionInOrganizations");
                });

            modelBuilder.Entity("ScouTeams.Models.Hufiec", b =>
                {
                    b.Property<int>("HufiecId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ChoragiewId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("HufiecId");

                    b.HasIndex("ChoragiewId");

                    b.ToTable("Hufiecs");
                });

            modelBuilder.Entity("ScouTeams.Models.KwateraGlowna", b =>
                {
                    b.Property<int>("KwateraGlownaId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.HasKey("KwateraGlownaId");

                    b.ToTable("KwateraGlowna");
                });

            modelBuilder.Entity("ScouTeams.Models.Meeting", b =>
                {
                    b.Property<int>("MeetingId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ZastepId")
                        .HasColumnType("int");

                    b.HasKey("MeetingId");

                    b.HasIndex("ScoutId");

                    b.ToTable("Meetings");
                });

            modelBuilder.Entity("ScouTeams.Models.ScoutPresence", b =>
                {
                    b.Property<int>("ScoutPresenceId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("MeetingId")
                        .HasColumnType("int");

                    b.Property<int>("Presence")
                        .HasColumnType("int");

                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ScoutPresenceId");

                    b.HasIndex("MeetingId");

                    b.ToTable("ScoutPresence");
                });

            modelBuilder.Entity("ScouTeams.Models.Skill", b =>
                {
                    b.Property<int>("SkillId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Difficulty")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("SkillId");

                    b.HasIndex("ScoutId");

                    b.ToTable("Skills");
                });

            modelBuilder.Entity("ScouTeams.Models.UserChoragiew", b =>
                {
                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ChoragiewId")
                        .HasColumnType("int");

                    b.HasKey("ScoutId", "ChoragiewId");

                    b.HasIndex("ChoragiewId");

                    b.ToTable("UserChoragiew");
                });

            modelBuilder.Entity("ScouTeams.Models.UserDruzyna", b =>
                {
                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("DruzynaId")
                        .HasColumnType("int");

                    b.HasKey("ScoutId", "DruzynaId");

                    b.HasIndex("DruzynaId");

                    b.ToTable("UserDruzyna");
                });

            modelBuilder.Entity("ScouTeams.Models.UserHufiec", b =>
                {
                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("HufiecId")
                        .HasColumnType("int");

                    b.HasKey("ScoutId", "HufiecId");

                    b.HasIndex("HufiecId");

                    b.ToTable("UserHufiec");
                });

            modelBuilder.Entity("ScouTeams.Models.UserZastep", b =>
                {
                    b.Property<string>("ScoutId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ZastepId")
                        .HasColumnType("int");

                    b.HasKey("ScoutId", "ZastepId");

                    b.HasIndex("ZastepId");

                    b.ToTable("UserZastep");
                });

            modelBuilder.Entity("ScouTeams.Models.Zastep", b =>
                {
                    b.Property<int>("ZastepId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("DruzynaId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("ZastepId");

                    b.HasIndex("DruzynaId");

                    b.ToTable("Zastep");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Areas.Identity.Data.Scout", b =>
                {
                    b.HasOne("ScouTeams.Models.KwateraGlowna", "KwateraGlowna")
                        .WithMany("Scouts")
                        .HasForeignKey("KwateraGlownaId");
                });

            modelBuilder.Entity("ScouTeams.Models.Choragiew", b =>
                {
                    b.HasOne("ScouTeams.Models.KwateraGlowna", "KwateraGlowna")
                        .WithMany("Choragiews")
                        .HasForeignKey("KwateraGlownaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.Contribution", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany("Contributions")
                        .HasForeignKey("ScoutId");
                });

            modelBuilder.Entity("ScouTeams.Models.Druzyna", b =>
                {
                    b.HasOne("ScouTeams.Models.Hufiec", "Hufiec")
                        .WithMany("Druzynas")
                        .HasForeignKey("HufiecId1")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.FunctionInOrganization", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany("FunctionInOrganizations")
                        .HasForeignKey("ScoutId");
                });

            modelBuilder.Entity("ScouTeams.Models.Hufiec", b =>
                {
                    b.HasOne("ScouTeams.Models.Choragiew", "Choragiew")
                        .WithMany("Hufiecs")
                        .HasForeignKey("ChoragiewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.Meeting", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany("Meetings")
                        .HasForeignKey("ScoutId");
                });

            modelBuilder.Entity("ScouTeams.Models.ScoutPresence", b =>
                {
                    b.HasOne("ScouTeams.Models.Meeting", "Meeting")
                        .WithMany("ScoutPresences")
                        .HasForeignKey("MeetingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.Skill", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", null)
                        .WithMany("Skills")
                        .HasForeignKey("ScoutId");
                });

            modelBuilder.Entity("ScouTeams.Models.UserChoragiew", b =>
                {
                    b.HasOne("ScouTeams.Models.Choragiew", "Choragiew")
                        .WithMany("Scouts")
                        .HasForeignKey("ChoragiewId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", "MyScout")
                        .WithMany("Choragiews")
                        .HasForeignKey("ScoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.UserDruzyna", b =>
                {
                    b.HasOne("ScouTeams.Models.Druzyna", "Druzyna")
                        .WithMany("Scouts")
                        .HasForeignKey("DruzynaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", "MyScout")
                        .WithMany("Druzynas")
                        .HasForeignKey("ScoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.UserHufiec", b =>
                {
                    b.HasOne("ScouTeams.Models.Hufiec", "Hufiec")
                        .WithMany("Scouts")
                        .HasForeignKey("HufiecId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", "MyScout")
                        .WithMany("Hufiecs")
                        .HasForeignKey("ScoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.UserZastep", b =>
                {
                    b.HasOne("ScouTeams.Areas.Identity.Data.Scout", "MyScout")
                        .WithMany("Zasteps")
                        .HasForeignKey("ScoutId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ScouTeams.Models.Zastep", "Zastep")
                        .WithMany("Scouts")
                        .HasForeignKey("ZastepId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ScouTeams.Models.Zastep", b =>
                {
                    b.HasOne("ScouTeams.Models.Druzyna", "Druzyna")
                        .WithMany("Zasteps")
                        .HasForeignKey("DruzynaId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
