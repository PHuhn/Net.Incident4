// ===========================================================================
namespace NSG.NetIncident4.Core.Domain.Entities.Authentication.OnModelCreating
{
	using Microsoft.EntityFrameworkCore;
	using Microsoft.EntityFrameworkCore.Metadata.Builders;
	//
	using NSG.NetIncident4.Core.Domain.Entities;
	//
	/// <summary>
	/// Table: IncidentIncidentNotes
	/// </summary>
	public class IncidentIncidentNoteConfiguration : IEntityTypeConfiguration<IncidentIncidentNote>
	{
		public void Configure(EntityTypeBuilder<IncidentIncidentNote> builder)
		{
			builder.ToTable("IncidentIncidentNotes");
			// propteries
			builder.HasKey(i => new { i.IncidentId, i.IncidentNoteId });
			// indexes
			builder.HasIndex(i => i.IncidentNoteId)
				.HasDatabaseName("IX_IncidentIncidentNotes_IncidentNoteId");
			// relationships
			builder.HasOne(ft => ft.Incident)
				.WithMany(i => i.IncidentIncidentNotes)
				.HasForeignKey(i => i.IncidentId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_IncidentIncidentNotes_Incident_IncidentId");
			builder.HasOne(ft => ft.IncidentNote)
				.WithMany(i => i.IncidentIncidentNotes)
				.HasForeignKey(i => i.IncidentNoteId)
				.OnDelete(DeleteBehavior.Restrict)
				.HasConstraintName("FK_IncidentIncidentNotes_IncidentNote_IncidentNoteId");
		} // Configure
	} // IncidentIncidentNoteConfiguration
}
// ===========================================================================
