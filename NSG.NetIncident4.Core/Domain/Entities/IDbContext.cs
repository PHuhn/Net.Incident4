﻿// ===========================================================================
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using NSG.NetIncident4.Core.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//
namespace NSG.NetIncident4.Core.Domain.Entities
{
    //
    public interface IDb_Contextx
    {
        // in Authentication
        DbSet<ApplicationUser> Users { get; set; }
        DbSet<ApplicationRole> Roles { get; set; }
        DbSet<ApplicationUserRole> UserRoles { get; set; }
        DbSet<IdentityUserClaim<string>> UserClaims { get; set; }
        DbSet<IdentityUserLogin<string>> UserLogins { get; set; }
        DbSet<IdentityUserToken<string>> UserTokens { get; set; }
        DbSet<IdentityRoleClaim<string>> RoleClaims { get; set; }
        // support in this folder
        DbSet<LogData> Logs { get; set; }
        DbSet<Company> Companies { get; set; }
        DbSet<Server> Servers { get; set; }
        DbSet<ApplicationUserServer> UserServers { get; set; }
        // types
        DbSet<IncidentType> IncidentTypes { get; set; }
        DbSet<NIC> NICs { get; set; }
        DbSet<NoteType> NoteTypes { get; set; }
        DbSet<EmailTemplate> EmailTemplates { get; set; }
        // incidents
        DbSet<Incident> Incidents { get; set; }
        DbSet<NetworkLog> NetworkLogs { get; set; }
        DbSet<IncidentNote> IncidentNotes { get; set; }
        DbSet<IncidentIncidentNote> IncidentIncidentNotes { get; set; }
        //
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}
// ===========================================================================
