// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    //
    using NSG.NetIncident4.Core.Domain.Entities;
    //
    /// <summary>
    /// Table: Audit
    /// </summary>
    public class AuditConfiguration : IEntityTypeConfiguration<Audit>
    {
        public void Configure(EntityTypeBuilder<Audit> builder)
        {
            builder.ToTable("Audit");
            // propteries
            builder.HasKey(a => a.Id);
            builder.Property(i => i.Id)
                .IsRequired()
                .HasAnnotation("Sqlite:Autoincrement", false)
                .HasColumnName("Id");
            builder.Property(a => a.ChangeDate)
                .IsRequired()
                .HasColumnType("datetime2")
                .HasColumnName("ChangeDate");
            builder.Property(a => a.UserName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("nvarchar")
                .HasColumnName("UserName");
            builder.Property(a => a.Program)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("nvarchar")
                .HasColumnName("Program");
            builder.Property(a => a.TableName)
                .IsRequired()
                .HasMaxLength(256)
                .HasColumnType("nvarchar")
                .HasColumnName("TableName");
            builder.Property(a => a.UpdateType)
                .IsRequired()
                .HasMaxLength(1)
                .HasColumnType("char")
                .HasColumnName("UpdateType");
            builder.Property(a => a.Keys)
                .IsRequired()
                .HasMaxLength(512)
                .HasColumnType("nvarchar")
                .HasColumnName("Keys");
            builder.Property(a => a.Before)
                .IsRequired()
                .HasMaxLength(4000)
                .HasColumnType("nvarchar")
                .HasColumnName("Before");
            builder.Property(a => a.After)
                .IsRequired()
                .HasMaxLength(4000)
                .HasColumnType("nvarchar")
                .HasColumnName("After");
            // relationships
        } // Configure
    } // AuditConfiguration
}
// ===========================================================================
