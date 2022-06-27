// ===========================================================================
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: Logs
	/// </summary>
	public class LogConfiguration : IEntityTypeConfiguration<LogData>
	{
		public void Configure(EntityTypeBuilder<LogData> builder)
		{
			builder.ToTable("Logs");
			// propteries
			builder.HasKey(l => l.Id);
			builder.Property(l => l.Date)
				.IsRequired()
				.HasColumnName("Date")
				.HasColumnType("datetime2");
			builder.Property(l => l.Application)
				.IsRequired()
				.HasMaxLength(30)
				.HasColumnName("Application")
				.HasColumnType("nvarchar");
			builder.Property(l => l.Method)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("Method")
				.HasColumnType("nvarchar");
			builder.Property(l => l.LogLevel)
				.IsRequired()
				.HasColumnName("LogLevel")
				.HasColumnType("tinyint");
			builder.Property(l => l.Level)
				.IsRequired()
				.HasMaxLength(8)
				.HasColumnName("Level")
				.HasColumnType("nvarchar");
			builder.Property(l => l.UserAccount)
				.IsRequired()
				.HasMaxLength(255)
				.HasColumnName("UserAccount")
				.HasColumnType("nvarchar");
			builder.Property(l => l.Message)
				.IsRequired()
				.HasMaxLength(4000)
				.HasColumnName("Message")
				.HasColumnType("nvarchar");
			builder.Property(l => l.Exception)
				.HasMaxLength(4000)
				.HasColumnName("Exception")
				.HasColumnType("nvarchar");
			// relationships
		} // Configure
	} // LogConfiguration
}
// ===========================================================================
