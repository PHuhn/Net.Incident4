// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Microsoft.AspNetCore.Identity;
	//
	/// <summary>
	/// Table: AspNetUserTokens
	/// </summary>
	public class IdentityUserTokenConfiguration : IEntityTypeConfiguration<IdentityUserToken<string>>
	{
		public void Configure(EntityTypeBuilder<IdentityUserToken<string>> builder)
		{
			builder.ToTable("AspNetUserTokens");
			// propteries
			builder.HasKey(a => new { a.UserId, a.LoginProvider, a.Name });
			//builder.Property(a => a.Value)
			//  .HasMaxLength(1073741823)
			//	.HasColumnName("Value")
			//	.HasColumnType("nvarchar");
			// relationships
			//builder.HasOne(ft => ft.User)
			//	.WithMany(ut => ut.IdentityUserTokens<string>)
			//	.HasForeignKey(u => u.UserId)
			//	.OnDelete(DeleteBehavior.Restrict)
			//	.HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
		} // Configure
	} // IdentityUserTokenConfiguration
}
// ===========================================================================
