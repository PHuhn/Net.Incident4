// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
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
			builder.ToTable("Incident");
            // propteries
            builder.HasKey(i => i.IncidentId);
            builder.Property(i => i.IncidentId)
                .IsRequired()
                .HasAnnotation("Sqlite:Autoincrement", false)
                .HasColumnName("IncidentId");
            builder.Property(i => i.ServerId)
                .IsRequired()
                .HasColumnType("int")
                .HasColumnName("ServerId");
            builder.Property(i => i.IPAddress)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar")
                .HasColumnName("IPAddress");
            builder.Property(i => i.NIC_Id)
                .IsRequired()
                .HasMaxLength(16)
                .HasColumnType("nvarchar")
                .HasColumnName("NIC_Id");
            builder.Property(i => i.NetworkName)
                .HasMaxLength(255)
                .HasColumnType("nvarchar")
                .HasColumnName("NetworkName");
            builder.Property(i => i.AbuseEmailAddress)
                .HasMaxLength(255)
                .HasColumnType("nvarchar")
                .HasColumnName("AbuseEmailAddress");
            builder.Property(i => i.ISPTicketNumber)
                .HasMaxLength(50)
                .HasColumnType("nvarchar")
                .HasColumnName("ISPTicketNumber");
            builder.Property(i => i.Mailed)
                .IsRequired()
                .HasColumnType("bit")
                .HasColumnName("Mailed");
            builder.Property(i => i.Closed)
                .IsRequired()
                .HasColumnType("bit")
                .HasColumnName("Closed");
            builder.Property(i => i.Special)
                .IsRequired()
                .HasColumnType("bit")
                .HasColumnName("Special");
            builder.Property(i => i.Notes)
                .HasColumnName("Notes");
            builder.Property(i => i.CreatedDate)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasColumnName("CreatedDate");
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
