// ===========================================================================
using System.Linq;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using NSG.NetIncident4.Core.Domain.Entities;
//
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication
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
            base.OnModelCreating(modelBuilder);
            //
            modelBuilder.Entity<ApplicationUser>((item) =>
            {
                item.Property(u => u.Id).HasMaxLength(450);
                item.HasMany(u => u.UserServers).WithOne(u => u.User)
                    .HasForeignKey(u => u.Id).OnDelete(DeleteBehavior.Cascade);
                item.HasOne(u => u.Company).WithMany(c => c.Users)
                    .HasForeignKey(u => u.CompanyId).OnDelete(DeleteBehavior.Restrict);
                // Each User can have many UserLogins
                item.HasMany(e => e.Logins).WithOne()
                    .HasForeignKey(ul => ul.UserId).IsRequired();
                // Each User can have many UserTokens
                item.HasMany(e => e.Tokens).WithOne()
                    .HasForeignKey(ut => ut.UserId).IsRequired();
                item.HasMany(u => u.UserRoles).WithOne(u => u.User)
                    .HasForeignKey(u => u.RoleId).OnDelete(DeleteBehavior.Cascade);
                // index
                item.HasIndex(u => u.FullName).IsUnique()
                    .HasDatabaseName("Idx_AspNetUsers_FullName");
            });
            //
            modelBuilder.Entity<ApplicationRole>((item) =>
            {
                item.Property(u => u.Id);
                // Each Role can have many entries in the UserRole join table
                item.HasMany(e => e.UserRoles).WithOne(e => e.Role)
                    .HasForeignKey(ur => ur.RoleId).IsRequired();
            });
            //
            // ApplicationUserRole
            modelBuilder.Entity<ApplicationUserRole>((item) =>
            {
                item.Property(ur => ur.UserId);
                item.Property(ur => ur.RoleId);
                item.HasKey(ur => new { ur.UserId, ur.RoleId });
                item.HasOne(ur => ur.Role).WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId).IsRequired();
                item.HasOne(ur => ur.User).WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId).IsRequired();
            });
            // ApplicationUserClaims
            modelBuilder.Entity<IdentityUserClaim<string>>()
                .Property(u => u.UserId);
            // ApplicationUserLogins
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .Property(u => u.UserId);
            // ApplicationUserTokens
            modelBuilder.Entity<IdentityUserToken<string>>()
                .Property(u => u.UserId);
            // AspNetRoleClaims
            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .Property(u => u.RoleId);
            //
            modelBuilder.Entity<ApplicationUserServer>((item) =>
            {
                item.Property(us => us.Id).HasMaxLength(450);
                item.HasKey(us => new { us.Id, us.ServerId });
                item.HasOne<Server>(srv => srv.Server)
                    .WithMany(s => s.UserServers).HasForeignKey(srv => srv.ServerId).IsRequired();
                item.HasOne<ApplicationUser>(usr => usr.User)
                    .WithMany(u => u.UserServers).HasForeignKey(usr => usr.Id).IsRequired();
            });
            //
            modelBuilder.Entity<Company>((item) =>
            {
                item.HasKey(c => c.CompanyId);
                item.HasMany(u => u.Servers).WithOne(s => s.Company).HasForeignKey(s => s.CompanyId);
                item.HasIndex(c => c.CompanyShortName).IsUnique()
                    .HasDatabaseName("Idx_Companies_ShortName");
            });
            //
            modelBuilder.Entity<Server>((item) =>
            {
                item.HasKey(s => s.ServerId);
                item.HasMany(u => u.UserServers).WithOne(u => u.Server).HasForeignKey(u => u.ServerId);
                item.HasOne(c => c.Company).WithMany(s => s.Servers)
                    .HasForeignKey(s => s.CompanyId).OnDelete(DeleteBehavior.Restrict);
                // index
                item.HasIndex(s => s.ServerShortName).IsUnique()
                    .HasDatabaseName("Idx_AspNetServers_ShortName");
            });
            //
            modelBuilder.Entity<IncidentType>((item) =>
            {
                item.HasKey(it => it.IncidentTypeId);
                item.HasMany(it => it.NetworkLogs).WithOne(l => l.IncidentType)
                    .HasForeignKey(it => it.IncidentTypeId).OnDelete(DeleteBehavior.Restrict);
                // index
                item.HasIndex(i => i.IncidentTypeShortDesc)
                    .IsUnique().HasDatabaseName("Idx_IncidentType_ShortDesc");
            });
            //
            modelBuilder.Entity<NetworkLog>((item) =>
            {
                item.HasKey(nl => nl.NetworkLogId);
                item.HasOne(nl => nl.IncidentType).WithMany(it => it.NetworkLogs)
                    .HasForeignKey(nl => nl.IncidentTypeId).OnDelete(DeleteBehavior.Restrict);
                item.HasOne(nl => nl.Server).WithMany(s => s.NetworkLogs)
                    .HasForeignKey(nl => nl.ServerId).OnDelete(DeleteBehavior.Restrict);
                // Incident is an optional nullable relationship
                item.HasOne(nl => nl.Incident).WithMany(i => i.NetworkLogs)
                    .HasForeignKey(nl => nl.IncidentId).IsRequired(false);
            });
            //
            modelBuilder.Entity<NIC>((item) =>
            {
                item.HasKey(n => n.NIC_Id);
                item.HasMany(n => n.Incidents).WithOne(i => i.NIC)
                    .HasForeignKey(n => n.NIC_Id).OnDelete(DeleteBehavior.Restrict);
            });
            //
            modelBuilder.Entity<EmailTemplate>((item) =>
            {
                item.HasKey(et => new { et.CompanyId, et.IncidentTypeId });
                // Company Company { get; set; }
                item.HasOne(nl => nl.Company).WithMany(c => c.EmailTemplates)
                    .HasForeignKey(nl => nl.CompanyId).OnDelete(DeleteBehavior.Restrict);
                item.HasOne(nl => nl.IncidentType).WithMany(et => et.EmailTemplates)
                    .HasForeignKey(nl => nl.IncidentTypeId).OnDelete(DeleteBehavior.Restrict);
            });
            //
            modelBuilder.Entity<Incident>((item) =>
            {
                item.HasKey(i => i.IncidentId);
                item.HasOne(i => i.NIC).WithMany(n => n.Incidents)
                    .HasForeignKey(i => i.NIC_Id).OnDelete(DeleteBehavior.Restrict);
                item.HasOne(i => i.Server).WithMany(s => s.Incidents)
                    .HasForeignKey(i => i.ServerId).OnDelete(DeleteBehavior.Restrict);
            });
            //
            modelBuilder.Entity<NoteType>((item) =>
            {
                item.HasKey(nt => nt.NoteTypeId);
                item.HasMany(nt => nt.IncidentNotes).WithOne(n => n.NoteType)
                    .HasForeignKey(nt => nt.IncidentNoteId).OnDelete(DeleteBehavior.Restrict);
                // index
                item.HasIndex(nt => nt.NoteTypeShortDesc)
                    .IsUnique().HasDatabaseName("Idx_NoteType_ShortDesc");
            });
            //
            modelBuilder.Entity<IncidentNote>((item) =>
            {
                item.HasKey(n => n.IncidentNoteId);
                item.HasOne(n => n.NoteType).WithMany(nt => nt.IncidentNotes)
                    .HasForeignKey(n => n.NoteTypeId).OnDelete(DeleteBehavior.Restrict);
            });
            //
            modelBuilder.Entity<IncidentIncidentNote>((item) =>
            {
                item.HasKey(iin => new { iin.IncidentId, iin.IncidentNoteId });
                item.HasOne(iin => iin.IncidentNote).WithMany(n => n.IncidentIncidentNotes)
                    .HasForeignKey(iin => iin.IncidentNoteId).OnDelete(DeleteBehavior.Cascade);
                item.HasOne(iin => iin.Incident).WithMany(i => i.IncidentIncidentNotes)
                    .HasForeignKey(iin => iin.IncidentId).OnDelete(DeleteBehavior.Cascade);
            });
            //
        }
        // support
        public DbSet<LogData> Logs { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<ApplicationUserServer> UserServers { get; set; }
        // types
        public DbSet<IncidentType> IncidentTypes { get; set; }
        public DbSet<NIC> NICs { get; set; }
        public DbSet<NoteType> NoteTypes { get; set; }
        public DbSet<EmailTemplate> EmailTemplates { get; set; }
        // incidents
        public DbSet<NetworkLog> NetworkLogs { get; set; }
        public DbSet<Incident> Incidents { get; set; }
        public DbSet<IncidentNote> IncidentNotes { get; set; }
        public DbSet<IncidentIncidentNote> IncidentIncidentNotes { get; set; }
        //
    }
}
// ===========================================================================
