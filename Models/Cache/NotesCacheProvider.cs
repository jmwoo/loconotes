using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Services;
using Microsoft.Extensions.Caching.Memory;
using Note = loconotes.Models.Note.Note;

namespace loconotes.Models.Cache
{
    public interface INotesCacheProvider : ICacheProvider<List<Note.Note>>
    {
    }

    public class NotesCacheProvider : CacheProvider<List<Note.Note>>, INotesCacheProvider
    {
        public NotesCacheProvider(
            IMemoryCache memoryCache
            ) : base(
                memoryCache, 
                CacheKeys.NotesCacheKey, 
                TimeSpan.FromHours(1))
        {
        }
    }
}
