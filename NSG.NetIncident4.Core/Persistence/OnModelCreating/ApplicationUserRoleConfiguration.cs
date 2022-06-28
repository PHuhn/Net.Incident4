// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: ApplicationUserRole
	/// </summary>
	public class ApplicationUserRoleConfiguration : IEntityTypeConfiguration<ApplicationUserRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationUserRole> builder)
		{
			builder.ToTable("AspNetUserRoles");
			// propteries
			builder.HasKey(a => new { a.UserId, a.RoleId });
			builder.Property(ur => ur.UserId)
				.HasMaxLength(450);
			builder.Property(ur => ur.RoleId)
				.HasMaxLength(450);
			// indexes
			builder.HasIndex(a => a.RoleId)
				.HasDatabaseName("IX_AspNetUserRoles_RoleId");
			// relationships
			builder.HasOne(ft => ft.Role)
				.WithMany(ur => ur.UserRoles)
				.HasForeignKey(a => a.RoleId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId");
			builder.HasOne(ft => ft.User)
				.WithMany(ur => ur.UserRoles)
				.HasForeignKey(a => a.UserId)
				.IsRequired()
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUserRoles_AspNetUsers_UserId");
		} // Configure
	} // AspNetUserRoleConfiguration
}
// ===========================================================================
