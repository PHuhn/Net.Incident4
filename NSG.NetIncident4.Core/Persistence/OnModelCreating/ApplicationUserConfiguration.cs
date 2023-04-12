// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
    // ApplicationUser
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Microsoft.AspNetCore.Identity;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: AspNetUsers
	/// </summary>
	public class ApplicationUserConfiguration : IEntityTypeConfiguration<ApplicationUser>
	{
		public void Configure(EntityTypeBuilder<ApplicationUser> builder)
		{
			builder.ToTable("AspNetUsers");
			// propteries
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Id)
				.IsRequired()
				.HasMaxLength(450);
			builder.Property(a => a.FirstName)
				.IsRequired()
				.HasMaxLength(100)
				.HasColumnName("FirstName")
				.HasColumnType("nvarchar");
			builder.Property(a => a.LastName)
				.IsRequired()
				.HasMaxLength(100)
				.HasColumnName("LastName")
				.HasColumnType("nvarchar");
			builder.Property(a => a.FullName)
				.IsRequired()
				.HasMaxLength(100)
				.HasColumnName("FullName")
				.HasColumnType("nvarchar");
			builder.Property(a => a.UserNicName)
				.IsRequired()
				.HasMaxLength(16)
				.HasColumnName("UserNicName")
				.HasColumnType("nvarchar");
			builder.Property(a => a.CompanyId)
				.IsRequired()
				.HasColumnName("CompanyId")
				.HasColumnType("int");
			builder.Property(a => a.CreateDate)
				.IsRequired()
				.HasColumnName("CreateDate")
				.HasColumnType("datetime2");
			// indexes
			builder.HasIndex(a => a.NormalizedEmail)
				.HasDatabaseName("EmailIndex");
			builder.HasIndex(a => a.CompanyId)
				.HasDatabaseName("IX_AspNetUsers_CompanyId");
			builder.HasIndex(a => a.FullName)
				.IsUnique()
				.HasDatabaseName("Idx_AspNetUsers_FullName");
			builder.HasIndex(a => a.NormalizedUserName)
				.IsUnique()
				.HasDatabaseName("UserNameIndex");
			// relationships
			builder.HasOne(ft => ft.Company)
				.WithMany(a => a.Users)
				.HasForeignKey(a => a.CompanyId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUsers_Companies_CompanyId");
			builder.HasMany(ft => ft.UserServers)
				.WithOne(a => a.User)
				.HasForeignKey(a => a.Id)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_ApplicationUserApplicationServer_AspNetUsers_Id");
			//builder.HasMany(ft => ft.UserClaims)
			//	.WithOne(a => a.User)
			//	.HasForeignKey(a => a.UserId)
			//	.OnDelete(DeleteBehavior.Restrict)
			//	.HasConstraintName("FK_AspNetUserClaims_AspNetUsers_UserId");
			// Each User can have many UserLogins
			builder.HasMany(ft => ft.Logins)
				.WithOne()
				.HasForeignKey(a => a.UserId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUserLogins_AspNetUsers_UserId");
			builder.HasMany(ft => ft.UserRoles)
				.WithOne(u => u.User)
				.HasForeignKey(u => u.UserId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId");
			// Each User can have many UserTokens
			builder.HasMany(ft => ft.Tokens)
				.WithOne()
				.HasForeignKey(u => u.UserId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUserTokens_AspNetUsers_UserId");
		} // Configure
	} // AspNetUserConfiguration
}
// ===========================================================================
