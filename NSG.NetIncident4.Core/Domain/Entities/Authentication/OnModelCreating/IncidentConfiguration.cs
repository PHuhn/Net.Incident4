// ===========================================================================
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: Incidents
	/// </summary>
	public class IncidentConfiguration : IEntityTypeConfiguration<Incident>
	{
		public void Configure(EntityTypeBuilder<Incident> builder)
		{
			builder.ToTable("Incidents");
			// propteries
			builder.HasKey(i => i.IncidentId);
			builder.Property(i => i.ServerId)
				.IsRequired()
				.HasColumnName("ServerId")
				.HasColumnType("int");
			builder.Property(i => i.IPAddress)
				.IsRequired()
				.HasMaxLength(50)
				.HasColumnName("IPAddress")
				.HasColumnType("nvarchar");
			builder.Property(i => i.NIC_Id)
				.IsRequired()
				.HasMaxLength(16)
				.HasColumnName("NIC_Id")
				.HasColumnType("nvarchar");
			builder.Property(i => i.NetworkName)
				.HasMaxLength(255)
				.HasColumnName("NetworkName")
				.HasColumnType("nvarchar");
			builder.Property(i => i.AbuseEmailAddress)
				.HasMaxLength(255)
				.HasColumnName("AbuseEmailAddress")
				.HasColumnType("nvarchar");
			builder.Property(i => i.ISPTicketNumber)
				.HasMaxLength(50)
				.HasColumnName("ISPTicketNumber")
				.HasColumnType("nvarchar");
			builder.Property(i => i.Mailed)
				.IsRequired()
				.HasColumnName("Mailed")
				.HasColumnType("bit");
			builder.Property(i => i.Closed)
				.IsRequired()
				.HasColumnName("Closed")
				.HasColumnType("bit");
			builder.Property(i => i.Special)
				.IsRequired()
				.HasColumnName("Special")
				.HasColumnType("bit");
			builder.Property(i => i.Notes)
				.HasMaxLength(1073741823)
				.HasColumnName("Notes")
				.HasColumnType("nvarchar");
			builder.Property(i => i.CreatedDate)
				.IsRequired()
				.HasColumnName("CreatedDate")
				.HasColumnType("datetime2");
			// indexes
			builder.HasIndex(i => i.NIC_Id)
				.HasDatabaseName("IX_Incident_NIC_Id");
			builder.HasIndex(i => i.ServerId)
				.HasDatabaseName("IX_Incident_ServerId");
			// relationships
			builder.HasMany(iin => iin.IncidentIncidentNotes)
				.WithOne(i => i.Incident)
				.HasForeignKey(i => i.IncidentId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_IncidentIncidentNotes_Incident_IncidentId");
			builder.HasMany(nl => nl.NetworkLogs)
				.WithOne(i => i.Incident)
				.HasForeignKey(i => i.IncidentId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_NetworkLog_Incident_IncidentId");
			builder.HasOne(ft => ft.NIC)
				.WithMany(i => i.Incidents)
				.HasForeignKey(i => i.NIC_Id)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_Incident_NIC_NIC_Id");
			builder.HasOne(ft => ft.Server)
				.WithMany(i => i.Incidents)
				.HasForeignKey(i => i.ServerId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_Incident_Servers_ServerId");
		} // Configure
	} // IncidentConfiguration
}
// ===========================================================================
