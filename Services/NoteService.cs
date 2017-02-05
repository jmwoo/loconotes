using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Business.GeoLocation;
using loconotes.Data;
using loconotes.Models.Cache;
using loconotes.Models.Note;
using loconotes.Models.User;
using Microsoft.EntityFrameworkCore;

namespace loconotes.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteViewModel>> GetAll();
        Task<NoteViewModel> Create(Note note);
        Task<Note> Vote(int id, VoteModel voteModel);
        Task<IEnumerable<NoteViewModel>> Nearby(NoteSearchRequest noteSearchRequest);
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
                .Select(n => n.ToNoteViewModel(IdentityService.Users.FirstOrDefault(u => n.UserId == u.Id)));
        }

        public async Task<NoteViewModel> Create(Note note)
        {
            try
            {
                _dbContext.Notes.Add(note);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                _notesCacheProvider.Clear();
                return note.ToNoteViewModel(IdentityService.Users.FirstOrDefault(u => note.UserId == u.Id));
            }
            catch (DbUpdateException updateException)
            {
                throw new ConflictException(updateException.Message, updateException);
            }
        }

        public async Task<Note> Vote(int id, VoteModel voteModel)
        {
            try
            {
                var note = await _dbContext.Notes.FindAsync(id).ConfigureAwait(false);
                note.Score += Convert.ToInt32(voteModel.Vote);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                _notesCacheProvider.Clear();
                return note;
            }
            catch (DbUpdateException updateException)
            {
                throw new ConflictException(updateException.Message, updateException);
            }
        }

        public async Task<IEnumerable<NoteViewModel>> Nearby(NoteSearchRequest noteSearchRequest)
        {
            var allNotes = _notesCacheProvider.Get()?.AsQueryable();

            if (allNotes == null)
            {
                allNotes = _dbContext.Notes.AsQueryable();
                _notesCacheProvider.Set(allNotes.ToList());
            }

            var geoCodeRange = GeolocationHelpers.CalculateGeoCodeRange(noteSearchRequest.LatitudeD, noteSearchRequest.LongitudeD, noteSearchRequest.RangeKmD,
                GeolocationHelpers.DistanceType.Kilometers);

            var nearbyNotes = allNotes.WhereInGeoCodeRange(new GeoCodeRange
            {
                MinimumLatitude = geoCodeRange.MinimumLatitude,
                MaximumLatitude = geoCodeRange.MaximumLatitude,
                MinimumLongitude = geoCodeRange.MinimumLongitude,
                MaximumLongitude = geoCodeRange.MaximumLongitude,
            });

            var orderedNearbyNotes = nearbyNotes
                .OrderBy(n =>
                        GeolocationHelpers.CalculateDistance(n.LatitudeD, n.LongitudeD, noteSearchRequest.LatitudeD, noteSearchRequest.LongitudeD,
                            GeolocationHelpers.DistanceType.Kilometers))
                .Take(noteSearchRequest.Take)
                .Select(n => n.ToNoteViewModel(IdentityService.Users.FirstOrDefault(u => n.UserId == u.Id)))
                ;

            return orderedNearbyNotes;
        }
    }
}
