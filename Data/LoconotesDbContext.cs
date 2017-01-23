using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Models.Note;
using Microsoft.EntityFrameworkCore;

namespace loconotes.Data
{
    public class LoconotesDbContext : DbContext
    {
        public LoconotesDbContext(DbContextOptions<LoconotesDbContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }
    }
}
