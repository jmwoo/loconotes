using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Models.Note;
using loconotes.Models.User;

namespace loconotes.Services
{
	public interface INoteConverter
	{
		NoteViewModel ToNoteViewModel(ApplicationUser applicationUser, Note note, VoteModel voteModel);
	}

    public class NoteConverter : INoteConverter
	{
		public NoteViewModel ToNoteViewModel(ApplicationUser applicationUser, Note note, VoteModel voteModel)
		{
			AuthorView authorView;

			if (note.IsAnonymous)
			{
				authorView = null;
			}
			else if (note.User == null) // viewer is self
				authorView = new AuthorView { Uid = applicationUser.Uid, Username = applicationUser.Username };
			else
				authorView = new AuthorView { Uid = note.User.Uid.Value, Username = note.User.Username };

			return new NoteViewModel
			{
				Id = note.Id,
				Uid = note.Uid.Value,
				Score = note.Score,
				DateCreated = note.DateCreated.Value,
				Subject = note.Subject,
				Body = note.Body,
				Latitude = note.Latitude,
				Longitude = note.Longitude,
				Radius = note.Radius,
				User = authorView,
				MyVote = voteModel?.Vote ?? VoteEnum.None
			};
		}
	}
}
