// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: Servers
	/// </summary>
	public class ServerConfiguration : IEntityTypeConfiguration<Server>
	{
		public void Configure(EntityTypeBuilder<Server> builder)
		{
			builder.ToTable("Servers");
			// propteries
			builder.HasKey(s => s.ServerId);
			builder.Property(s => s.CompanyId)
				.IsRequired()
				.HasColumnName("CompanyId")
				.HasColumnType("int");
			builder.Property(s => s.ServerShortName)
				.IsRequired()
				.HasMaxLength(12)
				.HasColumnName("ServerShortName")
				.HasColumnType("nvarchar");
			builder.Property(s => s.ServerName)
				.IsRequired()
				.HasMaxLength(80)
				.HasColumnName("ServerName")
				.HasColumnType("nvarchar");
			builder.Property(s => s.ServerDescription)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("ServerDescription")
				.HasColumnType("nvarchar");
			builder.Property(s => s.WebSite)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("WebSite")
				.HasColumnType("nvarchar");
			builder.Property(s => s.ServerLocation)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("ServerLocation")
				.HasColumnType("nvarchar");
			builder.Property(s => s.FromName)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("FromName")
				.HasColumnType("nvarchar");
			builder.Property(s => s.FromNicName)
				.IsRequired()
				.HasMaxLength(16)
				.HasColumnName("FromNicName")
				.HasColumnType("nvarchar");
			builder.Property(s => s.FromEmailAddress)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("FromEmailAddress")
				.HasColumnType("nvarchar");
			builder.Property(s => s.TimeZone)
				.IsRequired()
				.HasMaxLength(16)
				.HasColumnName("TimeZone")
				.HasColumnType("nvarchar");
			builder.Property(s => s.DST)
				.IsRequired()
				.HasColumnName("DST")
				.HasColumnType("bit");
			builder.Property(s => s.TimeZone_DST)
				.HasMaxLength(16)
				.HasColumnName("TimeZone_DST")
				.HasColumnType("nvarchar");
			builder.Property(s => s.DST_Start)
				.HasColumnName("DST_Start")
				.HasColumnType("datetime2");
			builder.Property(s => s.DST_End)
				.HasColumnName("DST_End")
				.HasColumnType("datetime2");
			// indexes
			builder.HasIndex(s => s.CompanyId)
				.HasDatabaseName("IX_Servers_CompanyId");
			builder.HasIndex(s => s.ServerShortName)
				.IsUnique()
				.HasDatabaseName("Idx_AspNetServers_ShortName");
			// relationships
			builder.HasMany(u => u.UserServers)
				.WithOne(u => u.Server)
				.HasForeignKey(u => u.ServerId);
			builder.HasOne(c => c.Company)
				.WithMany(s => s.Servers)
				.HasForeignKey(s => s.CompanyId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasMany(i => i.Incidents)
				.WithOne(s => s.Server)
				.HasForeignKey(u => u.ServerId)
				.HasConstraintName("FK_Incident_Servers_ServerId");
			builder.HasMany(i => i.NetworkLogs)
				.WithOne(s => s.Server)
				.HasForeignKey(u => u.ServerId)
				.HasConstraintName("FK_NetworkLog_Servers_ServerId");
			builder.HasOne(ft => ft.Company)
				.WithMany(s => s.Servers)
				.HasForeignKey(s => s.CompanyId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_Servers_Companies_CompanyId");
		} // Configure
	} // ServerConfiguration
}
// ===========================================================================
