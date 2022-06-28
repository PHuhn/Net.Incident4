// ===========================================================================
namespace NSG.NetIncident4.Core.Persistence.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: IncidentNotes
	/// </summary>
	public class IncidentNoteConfiguration : IEntityTypeConfiguration<IncidentNote>
	{
		public void Configure(EntityTypeBuilder<IncidentNote> builder)
		{
			builder.ToTable("IncidentNotes");
			// propteries
			builder.HasKey(i => i.IncidentNoteId);
			builder.Property(i => i.NoteTypeId)
				.IsRequired()
				.HasColumnName("NoteTypeId")
				.HasColumnType("int");
			builder.Property(i => i.Note)
				.IsRequired()
				.HasMaxLength(1073741823)
				.HasColumnName("Note")
				.HasColumnType("nvarchar");
			builder.Property(i => i.CreatedDate)
				.IsRequired()
				.HasColumnName("CreatedDate")
				.HasColumnType("datetime2");
			// indexes
			builder.HasIndex(i => i.NoteTypeId)
				.HasDatabaseName("IX_IncidentNote_NoteTypeId");
			// relationships
			builder.HasMany(iin => iin.IncidentIncidentNotes)
				.WithOne(s => s.IncidentNote)
				.HasForeignKey(i => i.IncidentNoteId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_IncidentIncidentNotes_IncidentNote_IncidentNoteId");
			builder.HasOne(ft => ft.NoteType)
				.WithMany(i => i.IncidentNotes)
				.HasForeignKey(i => i.NoteTypeId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_IncidentNote_NoteType_NoteTypeId");
		} // Configure
	} // IncidentNoteConfiguration
}
// ===========================================================================
