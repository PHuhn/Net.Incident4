// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: ApplicationRole
	/// </summary>
	public class ApplicationRoleConfiguration : IEntityTypeConfiguration<ApplicationRole>
	{
		public void Configure(EntityTypeBuilder<ApplicationRole> builder)
		{
			builder.ToTable("AspNetRoles");
			// propteries
			builder.HasKey(a => a.Id);
			builder.Property(a => a.Name)
				.HasMaxLength(256)
				.HasColumnName("Name")
				.HasColumnType("nvarchar");
			builder.Property(a => a.NormalizedName)
				.HasMaxLength(256)
				.HasColumnName("NormalizedName")
				.HasColumnType("nvarchar");
			builder.Property(a => a.ConcurrencyStamp)
				.HasColumnName("ConcurrencyStamp");
			// indexes
			builder.HasIndex(a => a.NormalizedName)
				.IsUnique()
				.HasDatabaseName("RoleNameIndex");
			// relationships
			//builder.HasMany(ft => ft.RoleClaims)
			//	.WithOne(a => a.Role)
			//	.HasForeignKey(a => a.RoleId)
			//	.OnDelete(DeleteBehavior.Restrict)
			//	.HasConstraintName("FK_AspNetRoleClaims_AspNetRoles_RoleId");
			builder.HasMany(ur => ur.UserRoles)
				.WithOne(r => r.Role)
				.HasForeignKey(r => r.RoleId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_AspNetUserRoles_AspNetRoles_RoleId");
		} // Configure
	} // AspNetRoleConfiguration
}
// ===========================================================================
