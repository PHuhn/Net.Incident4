// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: ApplicationUserApplicationServers
	/// </summary>
	public class ApplicationUserServerConfiguration : IEntityTypeConfiguration<ApplicationUserServer>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserServer> builder)
		{
			builder.ToTable("ApplicationUserApplicationServers");
			// propteries
			builder.HasKey(us => new { us.Id, us.ServerId });
			builder.Property(us => us.Id).HasMaxLength(450);
			// indexes
			builder.HasIndex(us => us.ServerId)
				.HasDatabaseName("IX_ApplicationUserApplicationServer_ServerId");
			// relationships
			builder.HasOne<ApplicationUser>(ft => ft.User)
				.WithMany(u => u.UserServers)
				.HasForeignKey(us => us.Id)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_ApplicationUserApplicationServer_AspNetUsers_Id");
			builder.HasOne<Server>(ft => ft.Server)
				.WithMany(us => us.UserServers)
				.HasForeignKey(a => a.ServerId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_ApplicationUserApplicationServer_Servers_ServerId");
		} // Configure
	} // ApplicationUserApplicationServerConfiguration
}
// ===========================================================================
