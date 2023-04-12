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
			builder.ToTable("NetworkLog");
            // propteries
            builder.HasKey(n => n.NetworkLogId);
            builder.Property(i => i.NetworkLogId)
                .IsRequired()
                .HasAnnotation("Sqlite:Autoincrement", false)
                .HasColumnName("NetworkLogId");
            builder.Property(n => n.ServerId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("ServerId");
            builder.Property(n => n.IncidentId)
                .HasColumnType("bigint")
                .HasColumnName("IncidentId");
            builder.Property(n => n.IPAddress)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar")
                .HasColumnName("IPAddress");
            builder.Property(n => n.NetworkLogDate)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasColumnName("NetworkLogDate");
            builder.Property(n => n.Log)
                .IsRequired()
                .HasColumnName("Log");
            builder.Property(n => n.IncidentTypeId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("IncidentTypeId");
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
