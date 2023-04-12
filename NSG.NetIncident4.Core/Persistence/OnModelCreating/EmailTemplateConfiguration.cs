// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
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
                .HasColumnName("SubjectLine");
            builder.Property(e => e.EmailBody)
                .IsRequired()
                .HasColumnName("EmailBody");
            builder.Property(e => e.TimeTemplate)
                .IsRequired()
                .HasColumnName("TimeTemplate");
            builder.Property(e => e.ThanksTemplate)
                .IsRequired()
                .HasColumnName("ThanksTemplate");
            builder.Property(e => e.LogTemplate)
                .IsRequired()
                .HasColumnName("LogTemplate");
            builder.Property(e => e.Template)
                .IsRequired()
                .HasColumnName("Template");
            builder.Property(e => e.FromServer)
                .IsRequired()
                .HasColumnType("bit")
                .HasColumnName("FromServer");
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
