using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Business.GeoLocation;
using loconotes.Data;
using loconotes.Models;
using loconotes.Models.Cache;
using loconotes.Models.Note;
using loconotes.Models.User;
using Microsoft.AspNetCore.Server.Kestrel;
using Microsoft.EntityFrameworkCore;

namespace loconotes.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteViewModel>> GetAll();
        Task<NoteViewModel> Create(ApplicationUser applicationUser, NoteCreateModel noteCreateModel);
        Task<NoteViewModel> Vote(ApplicationUser applicationUser, int NoteId, VoteModel voteModel);
        Task<IEnumerable<NoteViewModel>> Nearby(ApplicationUser applicationUser, NoteSearchRequest noteSearchRequest);
		Task DeleteAll(ApplicationUser applicationUser);
	    Task DeleteNote(ApplicationUser applicationUser, int noteId);

    }

    public class NoteService : INoteService
    {
        private readonly LoconotesDbContext _dbContext;
        private readonly INotesCacheProvider _notesCacheProvider;

        public NoteService(
            LoconotesDbContext dbContext,
            INotesCacheProvider notesCacheProvider
        )
        {
            _dbContext = dbContext;
            _notesCacheProvider = notesCacheProvider;
        }

        public async Task<IEnumerable<NoteViewModel>> GetAll()
        {
            IEnumerable<Note> allNotes = _notesCacheProvider.Get();

            if (allNotes == null)
            {
                allNotes = _dbContext.Notes;
                _notesCacheProvider.Set(allNotes.ToList());
            }

            return allNotes
                .OrderByDescending(n => n.DateCreated)
				.Where(n => !n.IsDeleted)
                .Select(n => n.ToNoteViewModel(null));
        }

        public async Task<NoteViewModel> Create(ApplicationUser applicationUser, NoteCreateModel noteCreateModel)
        {
            var noteToCreate = noteCreateModel.ToNote(applicationUser);

            try
            {
                _dbContext.Notes.Add(noteToCreate);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                _notesCacheProvider.Clear();
                return noteToCreate.ToNoteViewModel(applicationUser);
            }
            catch (DbUpdateException updateException)
            {
                throw new ConflictException(updateException.Message, updateException);
            }
        }

		public async Task DeleteAll(ApplicationUser applicationUser)
		{
			try
			{
				foreach (var note in _dbContext.Notes.Where(note => note.UserId == applicationUser.Id))
				{
					note.IsDeleted = true;
				}
				await _dbContext.SaveChangesAsync().ConfigureAwait(false);
				_notesCacheProvider.Clear();
			}
			catch (DbUpdateException updateException)
			{
				throw new ConflictException(updateException.Message, updateException);
			}
		}

	    public async Task DeleteNote(ApplicationUser applicationUser, int noteId)
	    {
			try
			{
				var note = await _dbContext.Notes.FindAsync(noteId).ConfigureAwait(false);
				note.IsDeleted = true;
				await _dbContext.SaveChangesAsync().ConfigureAwait(false);
				_notesCacheProvider.Clear();
			}
			catch (DbUpdateException updateException)
			{
				throw new ConflictException(updateException.Message, updateException);
			}
		}

	    public async Task<NoteViewModel> Vote(ApplicationUser applicationUser, int NoteId, VoteModel voteModel)
        {
            try
            {
                var note = await _dbContext.Notes.FindAsync(NoteId).ConfigureAwait(false);

	            if (note.IsDeleted)
	            {
					throw new NotFoundException();
				}

                _dbContext.Votes.Add(new Vote
                {
                    NoteId = note.Id,
                    UserId = voteModel.UserId,
                    Value = (int) voteModel.Vote
                });

                note.Score += Convert.ToInt32(voteModel.Vote);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                _notesCacheProvider.Clear();
                return note.ToNoteViewModel(applicationUser, voteModel);
            }
            catch (DbUpdateException updateException)
            {
                throw new ConflictException(updateException.Message, updateException);
            }
        }

        public async Task<IEnumerable<NoteViewModel>> Nearby(ApplicationUser applicationUser, NoteSearchRequest noteSearchRequest)
        {
            var geoCodeRange = GeolocationHelpers.CalculateGeoCodeRange(noteSearchRequest.LatitudeD, noteSearchRequest.LongitudeD, noteSearchRequest.RangeKmD,
                GeolocationHelpers.DistanceType.Kilometers);

            var userVotes = _dbContext.Votes.Where(v => v.UserId == applicationUser.Id);

            var nearbyNotes = _dbContext.Notes.AsQueryable().WhereInGeoCodeRange(new GeoCodeRange
            {
                MinimumLatitude = geoCodeRange.MinimumLatitude,
                MaximumLatitude = geoCodeRange.MaximumLatitude,
                MinimumLongitude = geoCodeRange.MinimumLongitude,
                MaximumLongitude = geoCodeRange.MaximumLongitude,
            });

            // TODO: holy shit this is complicated
            var orderedNearbyNotes = nearbyNotes
                .OrderBy(n =>
                        GeolocationHelpers.CalculateDistance(n.LatitudeD, n.LongitudeD, noteSearchRequest.LatitudeD, noteSearchRequest.LongitudeD,
                            GeolocationHelpers.DistanceType.Kilometers))
				.Where(n => !n.IsDeleted)
                .Take(noteSearchRequest.Take)
                .Select(n => n.ToNoteViewModel(applicationUser, 
                    userVotes.Select(v => v.NoteId).Contains(n.Id) ? new VoteModel { UserId = applicationUser.Id, Vote = (VoteEnum)userVotes.FirstOrDefault(v => v.NoteId == n.Id).Value } : null)
                )
                ;

            return orderedNearbyNotes;
        }
    }
}
