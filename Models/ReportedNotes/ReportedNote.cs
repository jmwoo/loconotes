using loconotes.Models.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models.ReportedNotes
{
	[Table("ReportedNotes")]
	public class ReportedNote
    {
	    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	    public int Id { get; set; }

	    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public Guid? Uid { get; set; }

	    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	    public int NoteId { get; set; }

	    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
	    public int UserId { get; set; }

	    [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
	    public DateTime DateCreated { get; set; }

	    [ForeignKey("UserId")]
	    public virtual UserEntity User { get; set; }

	    [ForeignKey("NoteId")]
	    public virtual Note.Note Note { get; set; }
	}
}
