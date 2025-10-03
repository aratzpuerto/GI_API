using GI_API.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace GI_API.Database
{
    public class GIDbContext: DbContext
    {
        public GIDbContext(DbContextOptions<GIDbContext> options) : base(options) { }

        public DbSet<TaskType> TaskTypes { get; set; }
        public DbSet<Models.Task> Tasks { get; set; }
        public DbSet<WeaponType> WeaponTypes { get; set; }
        public DbSet<Stat> Stats { get; set; }
        
        //public DbSet<Weapon> Weapons { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Element> Elements { get; set; }
        //public DbSet<Character> Characters { get; set; }
        //public DbSet<Constellation> Constellations { get; set; }
        public DbSet<AscensionMaterialType> AscensionMaterialTypes { get; set; }

        //public DbSet<AscensionMaterial> AscensionMaterials { get; set; }
        //public DbSet<Talent> Talents { get; set; }
        public DbSet<Day> Days { get; set; }
        public DbSet<DomainType> DomainTypes { get; set; }

        //public DbSet<Domain> Domains { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Task -> TaskType relationship
            modelBuilder.Entity<Models.Task>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd();
                entity.Property(e => e.Description)
                      .HasColumnType("varchar(max)");

                entity.Property(e => e.ShowOrder)
                      .HasDefaultValue(1);

                entity.Property(e => e.Show)
                      .HasDefaultValue(true);

                entity.Property(e => e.Completed)
                      .HasDefaultValue(false);

                entity.Property(e => e.CompletionDate)
                      .HasColumnType("date");

                entity.HasOne(t => t.TaskType)
                      .WithMany()
                      .HasForeignKey(t => t.TypeId)
                      .OnDelete(DeleteBehavior.Cascade)
                      .HasConstraintName("FK__Tasks__Type");
            });


          
        }

    }
}
