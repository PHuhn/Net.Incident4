// ===========================================================================
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NSG.NetIncident4.Core.Domain.Entities;
//
namespace NSG.NetIncident4.Core.Persistence
{
    public partial class ApplicationDbContext : IdentityDbContext<
            ApplicationUser, // TUser
            ApplicationRole, // TRole
            string, // TKey
            IdentityUserClaim<string>, // TUserClaim
            ApplicationUserRole, // TUserRole,
            IdentityUserLogin<string>, // TUserLogin
            IdentityRoleClaim<string>, // TRoleClaim
            IdentityUserToken<string> // TUserToken
        >
    {
        public object _options;
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            _options = options;
        }
        //
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // base.OnModelCreating(modelBuilder);
            //
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            //
            //modelBuilder.Entity<ApplicationUser>((item) =>
            //{
            //    item.Property(u => u.Id).HasMaxLength(450);
            //    item.HasMany(u => u.UserServers).WithOne(u => u.User)
            //        .HasForeignKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);
            //    item.HasOne(u => u.Company).WithMany(c => c.Users)
            //        .HasForeignKey(u => u.CompanyId).OnDelete(DeleteBehavior.Restrict);
            //    // Each User can have many UserLogins
            //    item.HasMany(e => e.Logins).WithOne()
            //        .HasForeignKey(ul => ul.UserId).IsRequired();
            //    // Each User can have many UserTokens
            //    item.HasMany(e => e.Tokens).WithOne()
            //        .HasForeignKey(ut => ut.UserId).IsRequired();
            //    item.HasMany(u => u.UserRoles).WithOne(u => u.User)
            //        .HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Cascade);
            //    // index
            //    item.HasIndex(u => u.FullName).IsUnique()
            //        .HasDatabaseName("Idx_AspNetUsers_FullName");
            //});
            ////
            //modelBuilder.Entity<ApplicationRole>((item) =>
            //{
            //    item.Property(u => u.Id);
            //    // Each Role can have many entries in the UserRole join table
            //    item.HasMany(e => e.UserRoles).WithOne(e => e.Role)
            //        .HasForeignKey(ur => ur.RoleId).IsRequired();
            //});
            ////
            //// ApplicationUserRole
            //modelBuilder.Entity<ApplicationUserRole>((item) =>
            //{
            //    item.Property(ur => ur.UserId);
            //    item.Property(ur => ur.RoleId);
            //    item.HasKey(ur => new { ur.UserId, ur.RoleId });
            //    item.HasOne(ur => ur.Role).WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.RoleId).IsRequired();
            //    item.HasOne(ur => ur.User).WithMany(r => r.UserRoles)
            //        .HasForeignKey(ur => ur.UserId).IsRequired();
            //});
            //// ApplicationUserClaims
            //modelBuilder.Entity<IdentityUserClaim<string>>((item) =>
            //{
            //    item.HasKey(a => a.Id);
            //    item.Property(a => a.UserId)
            //        .IsRequired()
            //        .HasMaxLength(450)
            //        .HasColumnName("UserId")
            //        .HasColumnType("nvarchar");
            //});
            //// ApplicationUserLogins
            //modelBuilder.Entity<IdentityUserLogin<string>>()
            //    .Property(u => u.UserId);
            //// ApplicationUserTokens
            //modelBuilder.Entity<IdentityUserToken<string>>()
            //    .Property(u => u.UserId);
            //// AspNetRoleClaims
            //modelBuilder.Entity<IdentityRoleClaim<string>>()
            //    .Property(u => u.RoleId);
            ////
            //modelBuilder.Entity<ApplicationUserServer>((item) =>
            //{
            //    item.Property(us => us.Id).HasMaxLength(450);
            //    item.HasKey(us => new { us.Id, us.ServerId });
            //    item.HasOne<Server>(srv => srv.Server)
            //        .WithMany(s => s.UserServers).HasForeignKey(srv => srv.ServerId).IsRequired();
            //    item.HasOne<ApplicationUser>(usr => usr.User)
            //        .WithMany(u => u.UserServers).HasForeignKey(usr => usr.Id).IsRequired();
            //});
            ////
            //modelBuilder.Entity<Company>((item) =>
            //{
            //    item.HasKey(c => c.CompanyId);
            //    // properties
            //    item.Property(c => c.CompanyShortName)
            //        .IsRequired()
            //        .HasMaxLength(12);
            //    item.Property(c => c.CompanyName)
            //        .IsRequired()
            //        .HasMaxLength(80);
            //    item.Property(c => c.Address)
            //        .HasMaxLength(80);
            //    item.Property(c => c.City)
            //        .HasMaxLength(50);
            //    item.Property(c => c.State)
            //        .HasMaxLength(4);
            //    item.Property(c => c.PostalCode)
            //        .HasMaxLength(15);
            //    item.Property(c => c.Country)
            //        .HasMaxLength(50);
            //    item.Property(c => c.PhoneNumber)
            //        .HasMaxLength(50);
            //    // relationships
            //    item.HasMany(u => u.Servers).WithOne(s => s.Company).HasForeignKey(s => s.CompanyId);
            //    item.HasIndex(c => c.CompanyShortName).IsUnique()
            //        .HasDatabaseName("Idx_Companies_ShortName");
            //});
            ////
            //modelBuilder.Entity<Server>((item) =>
            //{
            //    item.HasKey(s => s.ServerId);
            //    // properties
            //    item.Property(e => e.CompanyId)
            //        .IsRequired()
            //        .HasColumnName("CompanyId");
            //    item.Property(c => c.ServerShortName)
            //        .IsRequired()
            //        .HasMaxLength(12);
            //    item.Property(c => c.ServerName)
            //        .IsRequired()
            //        .HasMaxLength(80);
            //    item.Property(c => c.ServerDescription)
            //        .IsRequired()
            //        .HasMaxLength(255);
            //    item.Property(c => c.WebSite)
            //        .IsRequired()
            //        .HasMaxLength(255);
            //    item.Property(c => c.ServerLocation)
            //        .IsRequired()
            //        .HasMaxLength(255);
            //    item.Property(c => c.FromName)
            //        .IsRequired()
            //        .HasMaxLength(255);
            //    item.Property(c => c.FromNicName)
            //        .IsRequired()
            //        .HasMaxLength(16);
            //    item.Property(c => c.FromEmailAddress)
            //        .IsRequired()
            //        .HasMaxLength(255);
            //    item.Property(c => c.TimeZone)
            //        .IsRequired()
            //        .HasMaxLength(16);
            //    item.Property(c => c.DST)
            //        .IsRequired();
            //    item.Property(c => c.TimeZone_DST)
            //        .IsRequired()
            //        .HasMaxLength(16);
            //    item.Property(e => e.DST_Start)
            //        .HasColumnType("datetime");
            //    item.Property(e => e.DST_End)
            //        .HasColumnType("datetime");
            //    // relationships
            //    item.HasMany(u => u.UserServers).WithOne(u => u.Server).HasForeignKey(u => u.ServerId);
            //    item.HasOne(c => c.Company).WithMany(s => s.Servers)
            //        .HasForeignKey(s => s.CompanyId).OnDelete(DeleteBehavior.Restrict);
            //    item.HasMany(i => i.Incidents)
            //        .WithOne(s => s.Server)
            //        .HasForeignKey(u => u.ServerId)
            //        .HasConstraintName("FK_Incident_Servers_ServerId");
            //    item.HasMany(i => i.NetworkLogs)
            //        .WithOne(s => s.Server)
            //        .HasForeignKey(u => u.ServerId)
            //        .HasConstraintName("FK_NetworkLog_Servers_ServerId");
            //    item.HasOne(ft => ft.Company)
            //        .WithMany(s => s.Servers)
            //        .HasForeignKey(s => s.CompanyId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_Servers_Companies_CompanyId");
            //    // index
            //    item.HasIndex(s => s.ServerShortName).IsUnique()
            //        .HasDatabaseName("Idx_AspNetServers_ShortName");
            //});
            ////
            //modelBuilder.Entity<IncidentType>((item) =>
            //{
            //    item.HasKey(it => it.IncidentTypeId);
            //    item.HasMany(it => it.NetworkLogs).WithOne(l => l.IncidentType)
            //        .HasForeignKey(it => it.IncidentTypeId).OnDelete(DeleteBehavior.Restrict);
            //    // index
            //    item.HasIndex(i => i.IncidentTypeShortDesc)
            //        .IsUnique().HasDatabaseName("Idx_IncidentType_ShortDesc");
            //});
            ////
            //modelBuilder.Entity<NetworkLog>((item) =>
            //{
            //    item.HasKey(nl => nl.NetworkLogId);
            //    item.HasOne(nl => nl.IncidentType).WithMany(it => it.NetworkLogs)
            //        .HasForeignKey(nl => nl.IncidentTypeId).OnDelete(DeleteBehavior.Restrict);
            //    item.HasOne(nl => nl.Server).WithMany(s => s.NetworkLogs)
            //        .HasForeignKey(nl => nl.ServerId).OnDelete(DeleteBehavior.Restrict);
            //    // Incident is an optional nullable relationship
            //    item.HasOne(nl => nl.Incident).WithMany(i => i.NetworkLogs)
            //        .HasForeignKey(nl => nl.IncidentId).IsRequired(false);
            //});
            ////
            //modelBuilder.Entity<NIC>((item) =>
            //{
            //    item.ToTable("NIC");
            //    // propteries
            //    item.HasKey(n => n.NIC_Id);
            //    item.Property(n => n.NICDescription)
            //        .IsRequired()
            //        .HasMaxLength(255)
            //        .HasColumnName("NICDescription")
            //        .HasColumnType("nvarchar");
            //    item.Property(n => n.NICAbuseEmailAddress)
            //        .HasMaxLength(50)
            //        .HasColumnName("NICAbuseEmailAddress")
            //        .HasColumnType("nvarchar");
            //    item.Property(n => n.NICRestService)
            //        .HasMaxLength(255)
            //        .HasColumnName("NICRestService")
            //        .HasColumnType("nvarchar");
            //    item.Property(n => n.NICWebSite)
            //        .HasMaxLength(255)
            //        .HasColumnName("NICWebSite")
            //        .HasColumnType("nvarchar");
            //    // relationships
            //    item.HasMany(n => n.Incidents)
            //        .WithOne(i => i.NIC)
            //        .HasForeignKey(n => n.NIC_Id)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_Incident_NIC_NIC_Id");
            //});
            ////
            //modelBuilder.Entity<EmailTemplate>((item) =>
            //{
            //    item.ToTable("EmailTemplates");
            //    // propteries
            //    item.HasKey(e => new { e.CompanyId, e.IncidentTypeId });
            //    item.Property(e => e.SubjectLine)
            //        .IsRequired()
            //        .HasColumnName("SubjectLine")
            //        .HasColumnType("nvarchar");
            //    item.Property(e => e.EmailBody)
            //        .IsRequired()
            //        .HasColumnName("EmailBody")
            //        .HasColumnType("nvarchar");
            //    item.Property(e => e.TimeTemplate)
            //        .IsRequired()
            //        .HasColumnName("TimeTemplate")
            //        .HasColumnType("nvarchar");
            //    item.Property(e => e.ThanksTemplate)
            //        .IsRequired()
            //        .HasColumnName("ThanksTemplate")
            //        .HasColumnType("nvarchar");
            //    item.Property(e => e.LogTemplate)
            //        .IsRequired()
            //        .HasColumnName("LogTemplate")
            //        .HasColumnType("nvarchar");
            //    item.Property(e => e.Template)
            //        .IsRequired()
            //        .HasColumnName("Template")
            //        .HasColumnType("nvarchar");
            //    item.Property(e => e.FromServer)
            //        .IsRequired()
            //        .HasColumnName("FromServer")
            //        .HasColumnType("bit");
            //    // indexes
            //    item.HasIndex(e => e.IncidentTypeId)
            //        .HasDatabaseName("IX_EmailTemplates_IncidentTypeId");
            //    // relationships
            //    item.HasOne(ft => ft.Company)
            //        .WithMany(e => e.EmailTemplates)
            //        .HasForeignKey(e => e.CompanyId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_EmailTemplates_Companies_CompanyId");
            //    item.HasOne(ft => ft.IncidentType)
            //        .WithMany(e => e.EmailTemplates)
            //        .HasForeignKey(e => e.IncidentTypeId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_EmailTemplates_IncidentType_IncidentTypeId");
            //});
            ////
            //modelBuilder.Entity<Incident>((item) =>
            //{
            //    item.ToTable("Incident");
            //    // propteries
            //    item.HasKey(i => i.IncidentId);
            //    item.Property(i => i.IncidentId)
            //        .IsRequired()
            //        .HasColumnName("IncidentId")
            //        .HasColumnType("bigint");
            //    item.Property(i => i.ServerId)
            //        .IsRequired()
            //        .HasColumnName("ServerId")
            //        .HasColumnType("int");
            //    item.Property(i => i.IPAddress)
            //        .IsRequired()
            //        .HasMaxLength(50)
            //        .HasColumnName("IPAddress")
            //        .HasColumnType("nvarchar");
            //    item.Property(i => i.NIC_Id)
            //        .IsRequired()
            //        .HasMaxLength(16)
            //        .HasColumnName("NIC_Id")
            //        .HasColumnType("nvarchar");
            //    item.Property(i => i.NetworkName)
            //        .HasMaxLength(255)
            //        .HasColumnName("NetworkName")
            //        .HasColumnType("nvarchar");
            //    item.Property(i => i.AbuseEmailAddress)
            //        .HasMaxLength(255)
            //        .HasColumnName("AbuseEmailAddress")
            //        .HasColumnType("nvarchar");
            //    item.Property(i => i.ISPTicketNumber)
            //        .HasMaxLength(50)
            //        .HasColumnName("ISPTicketNumber")
            //        .HasColumnType("nvarchar");
            //    item.Property(i => i.Mailed)
            //        .IsRequired()
            //        .HasColumnName("Mailed")
            //        .HasColumnType("bit");
            //    item.Property(i => i.Closed)
            //        .IsRequired()
            //        .HasColumnName("Closed")
            //        .HasColumnType("bit");
            //    item.Property(i => i.Special)
            //        .IsRequired()
            //        .HasColumnName("Special")
            //        .HasColumnType("bit");
            //    item.Property(i => i.Notes)
            //        .HasMaxLength(1073741823)
            //        .HasColumnName("Notes")
            //        .HasColumnType("nvarchar");
            //    item.Property(i => i.CreatedDate)
            //        .IsRequired()
            //        .HasColumnName("CreatedDate")
            //        .HasColumnType("datetime2");
            //    // indexes
            //    item.HasIndex(i => i.NIC_Id)
            //        .HasDatabaseName("IX_Incident_NIC_Id");
            //    item.HasIndex(i => i.ServerId)
            //        .HasDatabaseName("IX_Incident_ServerId");
            //    // relationships
            //    item.HasMany(iin => iin.IncidentIncidentNotes)
            //        .WithOne(i => i.Incident)
            //        .HasForeignKey(i => i.IncidentId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_IncidentIncidentNotes_Incident_IncidentId");
            //    item.HasMany(nl => nl.NetworkLogs)
            //        .WithOne(i => i.Incident)
            //        .HasForeignKey(i => i.IncidentId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_NetworkLog_Incident_IncidentId");
            //    item.HasOne(ft => ft.NIC)
            //        .WithMany(i => i.Incidents)
            //        .HasForeignKey(i => i.NIC_Id)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_Incident_NIC_NIC_Id");
            //    item.HasOne(ft => ft.Server)
            //        .WithMany(i => i.Incidents)
            //        .HasForeignKey(i => i.ServerId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_Incident_Servers_ServerId");
            //});
            ////
            //modelBuilder.Entity<NoteType>((item) =>
            //{
            //    item.ToTable("NoteType");
            //    // propteries
            //    item.HasKey(nt => nt.NoteTypeId);
            //    item.Property(n => n.NoteTypeShortDesc)
            //        .IsRequired()
            //        .HasMaxLength(8)
            //        .HasColumnName("NoteTypeShortDesc")
            //        .HasColumnType("nvarchar");
            //    item.Property(n => n.NoteTypeDesc)
            //        .IsRequired()
            //        .HasMaxLength(50)
            //        .HasColumnName("NoteTypeDesc")
            //        .HasColumnType("nvarchar");
            //    item.Property(n => n.NoteTypeClientScript)
            //        .HasMaxLength(12)
            //        .HasColumnName("NoteTypeClientScript")
            //        .HasColumnType("nvarchar");
            //    // indexes
            //    item.HasIndex(n => n.NoteTypeShortDesc)
            //        .IsUnique()
            //        .HasDatabaseName("Idx_NoteType_ShortDesc");
            //    // relationships
            //    item.HasMany(nt => nt.IncidentNotes)
            //        .WithOne(n => n.NoteType)
            //        .HasForeignKey(nt => nt.NoteTypeId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_IncidentNote_NoteType_NoteTypeId");
            //});
            ////
            //modelBuilder.Entity<IncidentNote>((item) =>
            //{
            //    item.ToTable("IncidentNote");
            //    // propteries
            //    item.HasKey(i => i.IncidentNoteId);
            //    item.Property(i => i.IncidentNoteId)
            //        .IsRequired()
            //        .HasColumnName("IncidentNoteId")
            //        .HasColumnType("bigint");
            //    item.Property(i => i.NoteTypeId)
            //        .IsRequired()
            //        .HasColumnName("NoteTypeId")
            //        .HasColumnType("int");
            //    item.Property(i => i.Note)
            //        .IsRequired()
            //        .HasMaxLength(1073741823)
            //        .HasColumnName("Note")
            //        .HasColumnType("nvarchar");
            //    item.Property(i => i.CreatedDate)
            //        .IsRequired()
            //        .HasColumnName("CreatedDate")
            //        .HasColumnType("datetime2");
            //    // indexes
            //    item.HasIndex(i => i.NoteTypeId)
            //        .HasDatabaseName("IX_IncidentNote_NoteTypeId");
            //    // relationships
            //    item.HasMany(iin => iin.IncidentIncidentNotes)
            //        .WithOne(s => s.IncidentNote)
            //        .HasForeignKey(i => i.IncidentNoteId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_IncidentIncidentNotes_IncidentNote_IncidentNoteId");
            //    item.HasOne(ft => ft.NoteType)
            //        .WithMany(i => i.IncidentNotes)
            //        .HasForeignKey(i => i.NoteTypeId)
            //        .OnDelete(DeleteBehavior.Restrict)
            //        .HasConstraintName("FK_IncidentNote_NoteType_NoteTypeId");
            //});
            ////
            //modelBuilder.Entity<IncidentIncidentNote>((item) =>
            //{
            //    item.HasKey(iin => new { iin.IncidentId, iin.IncidentNoteId });
            //    item.HasOne(iin => iin.IncidentNote).WithMany(n => n.IncidentIncidentNotes)
            //        .HasForeignKey(iin => iin.IncidentNoteId).OnDelete(DeleteBehavior.Cascade);
            //    item.HasOne(iin => iin.Incident).WithMany(i => i.IncidentIncidentNotes)
            //        .HasForeignKey(iin => iin.IncidentId).OnDelete(DeleteBehavior.Cascade);
            //});
            //
        } // OnModelCreating
        /*
        ** support
        */
        public DbSet<LogData> Logs { get; set; } = default!;
        //
        public DbSet<Company> Companies { get; set; } = default!;
        //
        public DbSet<Server> Servers { get; set; } = default!;
        //
        public DbSet<ApplicationUserServer> UserServers { get; set; } = default!;
        // types
        public DbSet<IncidentType> IncidentTypes { get; set; } = default!;
        //
        public DbSet<NIC> NICs { get; set; } = default!;
        //
        public DbSet<NoteType> NoteTypes { get; set; } = default!;
        //
        public DbSet<EmailTemplate> EmailTemplates { get; set; } = default!;
        /*
        ** incidents
        */
        public DbSet<NetworkLog> NetworkLogs { get; set; } = default!;
        //
        public DbSet<Incident> Incidents { get; set; } = default!;
        //
        public DbSet<IncidentNote> IncidentNotes { get; set; } = default!;
        //
        public DbSet<IncidentIncidentNote> IncidentIncidentNotes { get; set; } = default!;
        //
    } // ApplicationDbContext
} // namespace
// ===========================================================================
