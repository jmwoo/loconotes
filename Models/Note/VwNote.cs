using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace loconotes.Models.Note
{
	[Table("vw_Notes")]
    public class VwNote // view only
    {
	    public int Id { get; set; }
	    public Guid Uid { get; set; }
	    public int Score { get; set; }
	    public DateTime DateCreated { get; set; }
	    public string Subject { get; set; }
	    public string Body { get; set; }
	    public decimal Latitude { get; set; }
	    public decimal Longitude { get; set; }
	    public int Radius { get; set; }
	    public bool IsAnonymous { get; set; }
		public string Username { get; set; }
		public int UserId { get; set; }
	    public Guid UserUid { get; set; }
	}
}
