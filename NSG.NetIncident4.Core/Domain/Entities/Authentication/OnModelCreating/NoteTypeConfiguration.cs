﻿// ===========================================================================
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: NoteTypes
	/// </summary>
	public class NoteTypeConfiguration : IEntityTypeConfiguration<NoteType>
	{
		public void Configure(EntityTypeBuilder<NoteType> builder)
		{
			builder.ToTable("NoteTypes");
			// propteries
			builder.HasKey(n => n.NoteTypeId);
			builder.Property(n => n.NoteTypeShortDesc)
				.IsRequired()
				.HasMaxLength(8)
				.HasColumnName("NoteTypeShortDesc")
				.HasColumnType("nvarchar");
			builder.Property(n => n.NoteTypeDesc)
				.IsRequired()
				.HasMaxLength(50)
				.HasColumnName("NoteTypeDesc")
				.HasColumnType("nvarchar");
			builder.Property(n => n.NoteTypeClientScript)
				.HasMaxLength(12)
				.HasColumnName("NoteTypeClientScript")
				.HasColumnType("nvarchar");
			// indexes
			builder.HasIndex(n => n.NoteTypeShortDesc)
				.IsUnique()
				.HasDatabaseName("Idx_NoteType_ShortDesc");
			// relationships
			builder.HasMany(nt => nt.IncidentNotes)
				.WithOne(n => n.NoteType)
				.HasForeignKey(nt => nt.IncidentNoteId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_IncidentNote_NoteType_NoteTypeId");
		} // Configure
	} // NoteTypeConfiguration
}
// ===========================================================================
