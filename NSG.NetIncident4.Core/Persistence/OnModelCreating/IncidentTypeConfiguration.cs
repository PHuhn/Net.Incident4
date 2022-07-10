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
			builder.Property(i => i.IncidentTypeShortDesc)
				.IsRequired()
				.HasMaxLength(8)
				.HasColumnName("IncidentTypeShortDesc")
				.HasColumnType("nvarchar");
			builder.Property(i => i.IncidentTypeDesc)
				.IsRequired()
				.HasMaxLength(50)
				.HasColumnName("IncidentTypeDesc")
				.HasColumnType("nvarchar");
			builder.Property(i => i.IncidentTypeFromServer)
				.IsRequired()
				.HasColumnName("IncidentTypeFromServer")
				.HasColumnType("bit");
			builder.Property(i => i.IncidentTypeSubjectLine)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("IncidentTypeSubjectLine")
				.HasColumnType("nvarchar");
			builder.Property(i => i.IncidentTypeEmailTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("IncidentTypeEmailTemplate")
				.HasColumnType("nvarchar");
			builder.Property(i => i.IncidentTypeTimeTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("IncidentTypeTimeTemplate")
				.HasColumnType("nvarchar");
			builder.Property(i => i.IncidentTypeThanksTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("IncidentTypeThanksTemplate")
				.HasColumnType("nvarchar");
			builder.Property(i => i.IncidentTypeLogTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("IncidentTypeLogTemplate")
				.HasColumnType("nvarchar");
			builder.Property(i => i.IncidentTypeTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("IncidentTypeTemplate")
				.HasColumnType("nvarchar");
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
