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
	/// Table: NICs
	/// </summary>
	public class NIC_Configuration : IEntityTypeConfiguration<NIC>
	{
		public void Configure(EntityTypeBuilder<NIC> builder)
		{
			builder.ToTable("NICs");
			// propteries
			builder.HasKey(n => n.NIC_Id);
			builder.Property(n => n.NICDescription)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("NICDescription")
				.HasColumnType("nvarchar");
			builder.Property(n => n.NICAbuseEmailAddress)
				.HasMaxLength(50)
				.HasColumnName("NICAbuseEmailAddress")
				.HasColumnType("nvarchar");
			builder.Property(n => n.NICRestService)
				.HasMaxLength(255)
				.HasColumnName("NICRestService")
				.HasColumnType("nvarchar");
			builder.Property(n => n.NICWebSite)
				.HasMaxLength(255)
				.HasColumnName("NICWebSite")
				.HasColumnType("nvarchar");
			// relationships
			builder.HasMany(n => n.Incidents)
				.WithOne(i => i.NIC)
				.HasForeignKey(n => n.NIC_Id)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_Incident_NIC_NIC_Id");
		} // Configure
	} // NICConfiguration
}
// ===========================================================================
