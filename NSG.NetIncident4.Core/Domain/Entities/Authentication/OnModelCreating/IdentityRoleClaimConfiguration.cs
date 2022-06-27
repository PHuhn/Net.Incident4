// ===========================================================================
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Microsoft.AspNetCore.Identity;
	//
	using NSG.NetIncident4.Core.Domain.Entities.Authentication;
	//
	/// <summary>
	/// Table: AspNetRoleClaims
	/// </summary>
	public class IdentityRoleClaimConfiguration : IEntityTypeConfiguration<IdentityRoleClaim<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityRoleClaim<string>> builder)
		{
			builder.ToTable("AspNetRoleClaims");
			// propteries
			builder.HasKey(a => a.Id);
			builder.Property(a => a.RoleId)
				.IsRequired()
				.HasMaxLength(450)
				.HasColumnName("RoleId")
				.HasColumnType("nvarchar");
			builder.Property(a => a.ClaimType)
				.HasMaxLength(1073741823)
				.HasColumnName("ClaimType")
				.HasColumnType("nvarchar");
			builder.Property(a => a.ClaimValue)
				.HasMaxLength(1073741823)
				.HasColumnName("ClaimValue")
				.HasColumnType("nvarchar");
			// indexes
			builder.HasIndex(a => a.RoleId)
				.HasDatabaseName("IX_AspNetRoleClaims_RoleId");
			// relationships
			//builder.HasOne(ft => ft.Role)
			//	.WithMany(a => a.IdentityRoleClaim<string>)
			//	.HasForeignKey(a => a.RoleId)
			//	.OnDelete(DeleteBehavior.Restrict)
			//	.HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
		} // Configure
	} // AspNetRoleClaimConfiguration
}
// ===========================================================================
