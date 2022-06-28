// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Microsoft.AspNetCore.Identity;
	//
	using NSG.NetIncident4.Core.Domain.Entities.Authentication;
	//
	/// <summary>
	/// Table: AspNetUserClaims
	/// </summary>
	public class IdentityUserClaimConfiguration : IEntityTypeConfiguration<IdentityUserClaim<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserClaim<string>> builder)
		{
			builder.ToTable("AspNetUserClaims");
			// propteries
			builder.HasKey(a => a.Id);
			builder.Property<string>(a => a.UserId)
				.IsRequired()
				.HasMaxLength(450)
				.HasColumnName("UserId")
				.HasColumnType("nvarchar");
			builder.Property<string>(a => a.ClaimType)
				.HasMaxLength(1073741823)
				.HasColumnName("ClaimType")
				.HasColumnType("nvarchar");
			builder.Property<string>(a => a.ClaimValue)
				.HasMaxLength(1073741823)
				.HasColumnName("ClaimValue")
				.HasColumnType("nvarchar");
			// indexes
			builder.HasIndex(a => a.UserId)
				.HasDatabaseName("IX_AspNetUserClaims_UserId");
			// relationships
			//builder.HasOne(ft => ft.User)
			//	.WithMany(a => a.UserClaims)
			//	.HasForeignKey(u => u.UserId)
			//	.OnDelete(DeleteBehavior.Restrict)
			//	.HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
		} // Configure
	} // IdentityUserClaimConfiguration
}
// ===========================================================================
