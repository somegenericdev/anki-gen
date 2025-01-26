using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AnkiGen.Models
{
    public partial class ProjectDbContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public ProjectDbContext()
        {
        }

        public ProjectDbContext(DbContextOptions<ProjectDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Word> Words { get; set; } = null!;
        public virtual DbSet<Definition> Definitions { get; set; } = null!;
        public virtual DbSet<Form> Forms { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            OnModelCreatingPartial(modelBuilder);
            modelBuilder.Entity<Word>().Property(x => x.Pos).HasConversion(new PartOfSpeechEnumConverter());
        }

        
        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            base.ConfigureConventions(configurationBuilder);
            // configurationBuilder.Properties<PartOfSpeechEnum>().HaveConversion<string>();
        }

        
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
