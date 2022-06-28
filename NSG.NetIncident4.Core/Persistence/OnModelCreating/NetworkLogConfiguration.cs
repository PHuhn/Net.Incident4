// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: NetworkLogs
	/// </summary>
	public class NetworkLogConfiguration : IEntityTypeConfiguration<NetworkLog>
	{
		public void Configure(EntityTypeBuilder<NetworkLog> builder)
		{
			builder.ToTable("NetworkLogs");
			// propteries
			builder.HasKey(n => n.NetworkLogId);
			builder.Property(n => n.ServerId)
				.IsRequired()
				.HasColumnName("ServerId")
				.HasColumnType("int");
			builder.Property(n => n.IncidentId)
				.HasColumnName("IncidentId")
				.HasColumnType("bigint");
			builder.Property(n => n.IPAddress)
				.IsRequired()
				.HasMaxLength(50)
				.HasColumnName("IPAddress")
				.HasColumnType("nvarchar");
			builder.Property(n => n.NetworkLogDate)
				.IsRequired()
				.HasColumnName("NetworkLogDate")
				.HasColumnType("datetime2");
			builder.Property(n => n.Log)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("Log")
				.HasColumnType("nvarchar");
			builder.Property(n => n.IncidentTypeId)
				.IsRequired()
				.HasColumnName("IncidentTypeId")
				.HasColumnType("int");
			// indexes
			builder.HasIndex(n => n.IncidentId)
				.HasDatabaseName("IX_NetworkLog_IncidentId");
			builder.HasIndex(n => n.IncidentTypeId)
				.HasDatabaseName("IX_NetworkLog_IncidentTypeId");
			builder.HasIndex(n => n.ServerId)
				.HasDatabaseName("IX_NetworkLog_ServerId");
			// relationships
			// Incident is an optional nullable relationship
			builder.HasOne(ft => ft.Incident)
				.WithMany(n => n.NetworkLogs)
				.HasForeignKey(n => n.IncidentId)
				.IsRequired(false)
				.HasConstraintName("FK_NetworkLog_Incident_IncidentId");
			builder.HasOne(ft => ft.IncidentType)
				.WithMany(n => n.NetworkLogs)
				.HasForeignKey(n => n.IncidentTypeId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_NetworkLog_IncidentType_IncidentTypeId");
			builder.HasOne(ft => ft.Server)
				.WithMany(n => n.NetworkLogs)
				.HasForeignKey(n => n.ServerId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_NetworkLog_Servers_ServerId");

		} // Configure
	} // NetworkLogConfiguration
}
// ===========================================================================
