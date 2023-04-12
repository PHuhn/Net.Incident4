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
	/// Table: Companies
	/// </summary>
	public class CompanyConfiguration : IEntityTypeConfiguration<Company>
	{
		public void Configure(EntityTypeBuilder<Company> builder)
		{
			builder.ToTable("Companies");
            // propteries
            builder.HasKey(c => c.CompanyId);
            builder.Property(c => c.CompanyShortName)
                .IsRequired()
                .HasMaxLength(12)
                .HasColumnType("nvarchar")
                .HasColumnName("CompanyShortName");
            builder.Property(c => c.CompanyName)
                .IsRequired()
                .HasMaxLength(80)
                .HasColumnType("nvarchar")
                .HasColumnName("CompanyName");
            builder.Property(c => c.Address)
                .HasMaxLength(80)
                .HasColumnType("nvarchar")
                .HasColumnName("Address");
            builder.Property(c => c.City)
                .HasMaxLength(50)
                .HasColumnType("nvarchar")
                .HasColumnName("City");
            builder.Property(c => c.State)
                .HasMaxLength(4)
                .HasColumnType("nvarchar")
                .HasColumnName("State");
            builder.Property(c => c.PostalCode)
                .HasMaxLength(15)
                .HasColumnType("nvarchar")
                .HasColumnName("PostalCode");
            builder.Property(c => c.Country)
                .HasMaxLength(50)
                .HasColumnType("nvarchar")
                .HasColumnName("Country");
            builder.Property(c => c.PhoneNumber)
                .HasMaxLength(50)
                .HasColumnType("nvarchar")
                .HasColumnName("PhoneNumber");
            builder.Property(c => c.Notes)
                .HasColumnName("Notes");
            // indexes
            builder.HasIndex(c => c.CompanyShortName)
				.IsUnique()
				.HasDatabaseName("Idx_Companies_ShortName");
			// relationships
			builder.HasMany(ft => ft.Users)
				.WithOne(c => c.Company)
				.HasForeignKey(c => c.CompanyId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUsers_Companies_CompanyId");
			builder.HasMany(et => et.EmailTemplates)
				.WithOne(c => c.Company)
				.HasForeignKey(et => et.CompanyId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_EmailTemplates_Companies_CompanyId");
			builder.HasMany(s => s.Servers)
				.WithOne(c => c.Company)
				.HasForeignKey(s => s.ServerId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_Servers_Companies_CompanyId");
		} // Configure
	} // CompanyConfiguration
}
// ===========================================================================
