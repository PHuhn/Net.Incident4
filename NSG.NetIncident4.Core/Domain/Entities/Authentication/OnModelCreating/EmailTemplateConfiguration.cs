// ===========================================================================
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: EmailTemplates
	/// </summary>
	public class EmailTemplateConfiguration : IEntityTypeConfiguration<EmailTemplate>
	{
		public void Configure(EntityTypeBuilder<EmailTemplate> builder)
		{
			builder.ToTable("EmailTemplates");
			// propteries
			builder.HasKey(e => new { e.CompanyId, e.IncidentTypeId });
			builder.Property(e => e.SubjectLine)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("SubjectLine")
				.HasColumnType("nvarchar");
			builder.Property(e => e.EmailBody)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("EmailBody")
				.HasColumnType("nvarchar");
			builder.Property(e => e.TimeTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("TimeTemplate")
				.HasColumnType("nvarchar");
			builder.Property(e => e.ThanksTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("ThanksTemplate")
				.HasColumnType("nvarchar");
			builder.Property(e => e.LogTemplate)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("LogTemplate")
				.HasColumnType("nvarchar");
			builder.Property(e => e.Template)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("Template")
				.HasColumnType("nvarchar");
			builder.Property(e => e.FromServer)
				.IsRequired()
				.HasColumnName("FromServer")
				.HasColumnType("bit");
			// indexes
			builder.HasIndex(e => e.IncidentTypeId)
				.HasDatabaseName("IX_EmailTemplates_IncidentTypeId");
			// relationships
			builder.HasOne(ft => ft.Company)
				.WithMany(e => e.EmailTemplates)
				.HasForeignKey(e => e.CompanyId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_EmailTemplates_Companies_CompanyId");
			builder.HasOne(ft => ft.IncidentType)
				.WithMany(e => e.EmailTemplates)
				.HasForeignKey(e => e.IncidentTypeId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_EmailTemplates_IncidentType_IncidentTypeId");
		} // Configure
	} // EmailTemplateConfiguration
}
// ===========================================================================
