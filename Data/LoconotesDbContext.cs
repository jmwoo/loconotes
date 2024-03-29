﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using loconotes.Models.Note;
using loconotes.Models.ReportedNotes;
using loconotes.Models.User;
using Microsoft.EntityFrameworkCore;

namespace loconotes.Data
{
    public class LoconotesDbContext : DbContext
    {
        public LoconotesDbContext(DbContextOptions<LoconotesDbContext> options) : base(options)
        {
        }

        public DbSet<Note> Notes { get; set; }

        public DbSet<Models.Vote> Votes { get; set; }

		public DbSet<ReportedNote> ReportedNotes { get; set; }

        public DbSet<UserEntity> Users { get; set; }

		public DbSet<VwNote> VwNotes { get; set; }
    }
}
