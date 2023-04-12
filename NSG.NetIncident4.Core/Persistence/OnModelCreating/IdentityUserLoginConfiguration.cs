// ===========================================================================
//namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
//{
//	using Microsoft.EntityFrameworkCore;
//	using Microsoft.EntityFrameworkCore.Metadata.Builders;
//	using Microsoft.AspNetCore.Identity;
//	//
//	/// <summary>
//	/// Table: AspNetUserLogins
//	/// </summary>
//	public class IdentityUserLoginConfiguration : IEntityTypeConfiguration<IdentityUserLogin<string>>
//	{
//		public void Configure(EntityTypeBuilder<IdentityUserLogin<string>> builder)
//		{
//			builder.ToTable("AspNetUserLogins");
//			// propteries
//			builder.HasKey(a => new { a.LoginProvider, a.ProviderKey });
//            builder.Property(a => a.ProviderDisplayName)
//            	.HasColumnName("ProviderDisplayName");
//            builder.Property(a => a.UserId)
//				.IsRequired()
//				.HasMaxLength(450)
//				.HasColumnName("UserId")
//				.HasColumnType("nvarchar");
//			// indexes
//			builder.HasIndex(a => a.UserId)
//				.HasDatabaseName("IX_AspNetUserLogins_UserId");
//			// relationships
//			//builder.HasOne(ft => ft.User)
//			//	.WithMany(a => a.IdentityUserLogins<string>)
//			//	.HasForeignKey(a => a.UserId)
//			//	.OnDelete(DeleteBehavior.Restrict)
//			//	.HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
//		} // Configure
//	} // IdentityUserLoginConfiguration
//}
// ===========================================================================
