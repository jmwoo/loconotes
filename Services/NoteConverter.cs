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
		IEnumerable<NoteViewModel> ToNoteViewModel(ApplicationUser viewingUser, IEnumerable<Note> notes, VoteModel viewingUsersVote);
	}

    public class NoteConverter : INoteConverter
	{
		public IEnumerable<NoteViewModel> ToNoteViewModel(ApplicationUser viewingUser, IEnumerable<Note> notes, VoteModel viewingUsersVote)
		{
			return notes.Select(note =>
			{
				var userNoteViewModel = note.IsAnonymous || viewingUser == null ? null : new AuthorView
				{
					Uid = viewingUser.Uid,
					Username = viewingUser.Username
				};

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
					User = userNoteViewModel,
					MyVote = viewingUsersVote?.Vote ?? VoteEnum.None
				};
			});
		}
	}
}
