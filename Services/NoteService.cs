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
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public NoteService(
            LoconotesDbContext dbContext
        )
        {
            _dbContext = dbContext;
        }

        public async Task<IEnumerable<NoteViewModel>> GetAll()
        {
	        var notes = await _dbContext.Notes
		        .Include(n => n.User)
		        .Where(n => !n.IsDeleted)
		        .OrderByDescending(n => n.DateCreated)
		        .Select(n => n.ToNoteViewModel(null, null))
		        .ToListAsync();

	        return notes;
        }

        public async Task<NoteViewModel> Create(ApplicationUser applicationUser, NoteCreateModel noteCreateModel)
        {
            var noteToCreate = noteCreateModel.ToNote(applicationUser);

            try
            {
                EntityEntry<Note> createdEntity = _dbContext.Notes.Add(noteToCreate);

                await _dbContext.SaveChangesAsync().ConfigureAwait(false);

				//return (await _dbContext.Notes.Include(n => n.User).FindAsync(createdEntity.Entity.Id)).ToNoteViewModel(applicationUser, null);
	            var note = (await _dbContext.Notes.Include(n => n.User).FirstAsync(n => n.Id == createdEntity.Entity.Id));
				return note.ToNoteViewModel(applicationUser, null);
			}
            catch (DbUpdateException updateException)
            {
                throw new ConflictException(updateException.Message, updateException);
            }
        }

		public async Task DeleteAll(ApplicationUser applicationUser)
		{
			foreach (var note in _dbContext.Notes.Where(note => note.UserId == applicationUser.Id))
			{
				note.IsDeleted = true;
			}
			await _dbContext.SaveChangesAsync().ConfigureAwait(false);
		}

	    public async Task DeleteNote(ApplicationUser applicationUser, int noteId)
	    {
		    var note = await _dbContext.Notes.FindAsync(noteId).ConfigureAwait(false);

		    if (note.UserId != applicationUser.Id)
		    {
			    throw new UnauthorizedAccessException();
		    }

		    note.IsDeleted = true;
		    await _dbContext.SaveChangesAsync().ConfigureAwait(false);
		}

	    public async Task<NoteViewModel> Vote(ApplicationUser applicationUser, int NoteId, VoteModel voteModel)
        {
			var note = await _dbContext.Notes.Include(n => n.User).FirstAsync(n => n.Id == NoteId).ConfigureAwait(false);

	        if (note.IsDeleted)
	        {
		        throw new NotFoundException();
	        }

	        _dbContext.Votes.Add(new Vote
	        {
		        NoteId = note.Id,
		        UserId = voteModel.UserId,
		        Value = (int)voteModel.Vote
	        });

	        note.Score += Convert.ToInt32(voteModel.Vote);

	        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
	        return note.ToNoteViewModel(applicationUser, voteModel);
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
				.Include(n => n.User)
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
