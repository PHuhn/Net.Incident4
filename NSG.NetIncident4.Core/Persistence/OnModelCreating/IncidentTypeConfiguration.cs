// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Microsoft.AspNetCore.Identity;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: IncidentTypes
	/// </summary>
	public class IncidentTypeConfiguration : IEntityTypeConfiguration<IncidentType>
	{
		public void Configure(EntityTypeBuilder<IncidentType> builder)
		{
			builder.ToTable("IncidentType");
            // propteries
            builder.HasKey(i => i.IncidentTypeId);
            builder.Property(i => i.IncidentTypeId)
                .IsRequired()
                .HasAnnotation("Sqlite:Autoincrement", false)
                .HasColumnName("IncidentTypeId");
            builder.Property(i => i.IncidentTypeShortDesc)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnType("nvarchar")
                .HasColumnName("IncidentTypeShortDesc");
            builder.Property(i => i.IncidentTypeDesc)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnType("nvarchar")
                .HasColumnName("IncidentTypeDesc");
            builder.Property(i => i.IncidentTypeFromServer)
                .IsRequired()
                .HasColumnType("bit")
                .HasColumnName("IncidentTypeFromServer");
            builder.Property(i => i.IncidentTypeSubjectLine)
                .IsRequired()
                .HasColumnName("IncidentTypeSubjectLine");
            builder.Property(i => i.IncidentTypeEmailTemplate)
                .IsRequired()
                .HasColumnName("IncidentTypeEmailTemplate");
            builder.Property(i => i.IncidentTypeTimeTemplate)
                .IsRequired()
                .HasColumnName("IncidentTypeTimeTemplate");
            builder.Property(i => i.IncidentTypeThanksTemplate)
                .IsRequired()
                .HasColumnName("IncidentTypeThanksTemplate");
            builder.Property(i => i.IncidentTypeLogTemplate)
                .IsRequired()
                .HasColumnName("IncidentTypeLogTemplate");
            builder.Property(i => i.IncidentTypeTemplate)
                .IsRequired()
                .HasColumnName("IncidentTypeTemplate");
            // indexes
            builder.HasIndex(i => i.IncidentTypeShortDesc)
				.IsUnique()
				.HasDatabaseName("Idx_IncidentType_ShortDesc");
			// relationships
			builder.HasMany(it => it.EmailTemplates)
				.WithOne(l => l.IncidentType)
				.HasForeignKey(it => it.IncidentTypeId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_EmailTemplates_IncidentType_IncidentTypeId");
			builder.HasMany(it => it.NetworkLogs)
				.WithOne(l => l.IncidentType)
				.HasForeignKey(it => it.IncidentTypeId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_NetworkLog_IncidentType_IncidentTypeId");
		} // Configure
	} // IncidentTypeConfiguration
}
// ===========================================================================
