﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NSG.NetIncident4.Core.Domain.Entities.Authentication;

namespace NSG.NetIncident4.Core.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211129204806_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

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
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.ApplicationUserServer", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("ServerId")
                        .HasColumnType("int");

                    b.HasKey("Id", "ServerId");

                    b.HasIndex("ServerId");

                    b.ToTable("ApplicationUserApplicationServer");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserNicName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUserRole", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Company", b =>
                {
                    b.Property<int>("CompanyId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("City")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("CompanyShortName")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("Country")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.Property<string>("State")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("CompanyId");

                    b.HasIndex("CompanyShortName")
                        .IsUnique()
                        .HasDatabaseName("Idx_Companies_ShortName");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.EmailTemplate", b =>
                {
                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("IncidentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("EmailBody")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("FromServer")
                        .HasColumnType("bit");

                    b.Property<string>("LogTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectLine")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Template")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ThanksTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TimeTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CompanyId", "IncidentTypeId");

                    b.HasIndex("IncidentTypeId");

                    b.ToTable("EmailTemplates");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Incident", b =>
                {
                    b.Property<long>("IncidentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AbuseEmailAddress")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<bool>("Closed")
                        .HasColumnType("bit");

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ISPTicketNumber")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<bool>("Mailed")
                        .HasColumnType("bit");

                    b.Property<string>("NIC_Id")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("NetworkName")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("Notes")
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ServerId")
                        .HasColumnType("int");

                    b.Property<bool>("Special")
                        .HasColumnType("bit");

                    b.HasKey("IncidentId");

                    b.HasIndex("NIC_Id");

                    b.HasIndex("ServerId");

                    b.ToTable("Incident");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.IncidentIncidentNote", b =>
                {
                    b.Property<long>("IncidentId")
                        .HasColumnType("bigint");

                    b.Property<long>("IncidentNoteId")
                        .HasColumnType("bigint");

                    b.HasKey("IncidentId", "IncidentNoteId");

                    b.HasIndex("IncidentNoteId");

                    b.ToTable("IncidentIncidentNotes");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.IncidentNote", b =>
                {
                    b.Property<long>("IncidentNoteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("CreatedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Note")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("NoteTypeId")
                        .HasColumnType("int");

                    b.HasKey("IncidentNoteId");

                    b.HasIndex("NoteTypeId");

                    b.ToTable("IncidentNote");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.IncidentType", b =>
                {
                    b.Property<int>("IncidentTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IncidentTypeDesc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("IncidentTypeEmailTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IncidentTypeFromServer")
                        .HasColumnType("bit");

                    b.Property<string>("IncidentTypeLogTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncidentTypeShortDesc")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<string>("IncidentTypeSubjectLine")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncidentTypeTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncidentTypeThanksTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IncidentTypeTimeTemplate")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IncidentTypeId");

                    b.HasIndex("IncidentTypeShortDesc")
                        .IsUnique()
                        .HasDatabaseName("Idx_IncidentType_ShortDesc");

                    b.ToTable("IncidentType");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.LogData", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Application")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Exception")
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Level")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.Property<byte>("LogLevel")
                        .HasColumnType("tinyint");

                    b.Property<string>("Message")
                        .IsRequired()
                        .HasMaxLength(4000)
                        .HasColumnType("nvarchar(4000)");

                    b.Property<string>("Method")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("UserAccount")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("Id");

                    b.ToTable("Log");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.NIC", b =>
                {
                    b.Property<string>("NIC_Id")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("NICAbuseEmailAddress")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NICDescription")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("NICRestService")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("NICWebSite")
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("NIC_Id");

                    b.ToTable("NIC");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.NetworkLog", b =>
                {
                    b.Property<long>("NetworkLogId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("IPAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<long?>("IncidentId")
                        .HasColumnType("bigint");

                    b.Property<int>("IncidentTypeId")
                        .HasColumnType("int");

                    b.Property<string>("Log")
                        .IsRequired()
                        .HasMaxLength(1073741823)
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("NetworkLogDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("ServerId")
                        .HasColumnType("int");

                    b.HasKey("NetworkLogId");

                    b.HasIndex("IncidentId");

                    b.HasIndex("IncidentTypeId");

                    b.HasIndex("ServerId");

                    b.ToTable("NetworkLog");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.NoteType", b =>
                {
                    b.Property<int>("NoteTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("NoteTypeClientScript")
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("NoteTypeDesc")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("NoteTypeShortDesc")
                        .IsRequired()
                        .HasMaxLength(8)
                        .HasColumnType("nvarchar(8)");

                    b.HasKey("NoteTypeId");

                    b.HasIndex("NoteTypeShortDesc")
                        .IsUnique()
                        .HasDatabaseName("Idx_NoteType_ShortDesc");

                    b.ToTable("NoteType");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Server", b =>
                {
                    b.Property<int>("ServerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<bool>("DST")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("DST_End")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("DST_Start")
                        .HasColumnType("datetime2");

                    b.Property<string>("FromEmailAddress")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FromName")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("FromNicName")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("ServerDescription")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ServerLocation")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("ServerName")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.Property<string>("ServerShortName")
                        .IsRequired()
                        .HasMaxLength(12)
                        .HasColumnType("nvarchar(12)");

                    b.Property<string>("TimeZone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("TimeZone_DST")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.Property<string>("WebSite")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.HasKey("ServerId");

                    b.HasIndex("CompanyId");

                    b.HasIndex("ServerShortName")
                        .IsUnique()
                        .HasDatabaseName("Idx_AspNetServers_ShortName");

                    b.ToTable("Servers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", null)
                        .WithMany("Logins")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", null)
                        .WithMany("Tokens")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.ApplicationUserServer", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", "User")
                        .WithMany("UserServers")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Server", "Server")
                        .WithMany("UserServers")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Server");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Company", "Company")
                        .WithMany("Users")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUserRole", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationRole", "Role")
                        .WithMany("UserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", "User")
                        .WithMany("UserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");

                    b.Navigation("User");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.EmailTemplate", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Company", "Company")
                        .WithMany("EmailTemplates")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.IncidentType", "IncidentType")
                        .WithMany("EmailTemplates")
                        .HasForeignKey("IncidentTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("IncidentType");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Incident", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.NIC", "NIC")
                        .WithMany("Incidents")
                        .HasForeignKey("NIC_Id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Server", "Server")
                        .WithMany("Incidents")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("NIC");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.IncidentIncidentNote", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Incident", "Incident")
                        .WithMany("IncidentIncidentNotes")
                        .HasForeignKey("IncidentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.IncidentNote", "IncidentNote")
                        .WithMany("IncidentIncidentNotes")
                        .HasForeignKey("IncidentNoteId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Incident");

                    b.Navigation("IncidentNote");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.IncidentNote", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.NoteType", "NoteType")
                        .WithMany("IncidentNotes")
                        .HasForeignKey("NoteTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("NoteType");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.NetworkLog", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Incident", "Incident")
                        .WithMany("NetworkLogs")
                        .HasForeignKey("IncidentId");

                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.IncidentType", "IncidentType")
                        .WithMany("NetworkLogs")
                        .HasForeignKey("IncidentTypeId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Server", "Server")
                        .WithMany("NetworkLogs")
                        .HasForeignKey("ServerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Incident");

                    b.Navigation("IncidentType");

                    b.Navigation("Server");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Server", b =>
                {
                    b.HasOne("NSG.NetIncident4.Core.Domain.Entities.Company", "Company")
                        .WithMany("Servers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationRole", b =>
                {
                    b.Navigation("UserRoles");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Authentication.ApplicationUser", b =>
                {
                    b.Navigation("Logins");

                    b.Navigation("Tokens");

                    b.Navigation("UserRoles");

                    b.Navigation("UserServers");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Company", b =>
                {
                    b.Navigation("EmailTemplates");

                    b.Navigation("Servers");

                    b.Navigation("Users");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Incident", b =>
                {
                    b.Navigation("IncidentIncidentNotes");

                    b.Navigation("NetworkLogs");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.IncidentNote", b =>
                {
                    b.Navigation("IncidentIncidentNotes");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.IncidentType", b =>
                {
                    b.Navigation("EmailTemplates");

                    b.Navigation("NetworkLogs");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.NIC", b =>
                {
                    b.Navigation("Incidents");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.NoteType", b =>
                {
                    b.Navigation("IncidentNotes");
                });

            modelBuilder.Entity("NSG.NetIncident4.Core.Domain.Entities.Server", b =>
                {
                    b.Navigation("Incidents");

                    b.Navigation("NetworkLogs");

                    b.Navigation("UserServers");
                });
#pragma warning restore 612, 618
        }
    }
}
