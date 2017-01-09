using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Business.Exceptions;
using loconotes.Business.GeoLocation;
using loconotes.Data;
using loconotes.Models;
using Microsoft.EntityFrameworkCore;

namespace loconotes.Services
{
    public interface INoteService
    {
        Task<IEnumerable<NoteViewModel>> GetAll();
        Task<NoteViewModel> Create(Note note);
        Task<Note> Vote(int id, Vote vote);
        Task<IEnumerable<NoteViewModel>> Nearby(NoteSearchRequest noteSearchRequest);
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
            var allNotes = _dbContext.Notes;

            return allNotes
                .OrderByDescending(n => n.DateCreated)
                .Select(n => n.ToNoteViewModel());
        }

        public async Task<NoteViewModel> Create(Note note)
        {
            try
            {
                _dbContext.Notes.Add(note);
                await _dbContext.SaveChangesAsync().ConfigureAwait(false);
                return note.ToNoteViewModel();
            }
            catch (DbUpdateException updateException)
            {
                throw new ConflictException(updateException.Message);
            }
        }

        public Task<Note> Vote(int id, Vote vote)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<NoteViewModel>> Nearby(NoteSearchRequest noteSearchRequest)
        {
            var geoCodeRange = GeolocationHelpers.CalculateGeoCodeRange(noteSearchRequest.LatitudeD, noteSearchRequest.LongitudeD, noteSearchRequest.RangeKmD,
                GeolocationHelpers.DistanceType.Kilometers);

            var nearbyNotes = _dbContext.Notes.AsQueryable().WhereInGeoCodeRange(new GeoCodeRange
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
                .Select(n => n.ToNoteViewModel())
                ;

            return orderedNearbyNotes;
        }
    }
}
