using Microsoft.EntityFrameworkCore;

namespace TypeRunnerBE.Models
{
    public class TypeMarathonContext : DbContext
    {
        public TypeMarathonContext(DbContextOptions<TypeMarathonContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Users
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            modelBuilder.Entity<User>()
                .HasMany(u => u.FriendsFrom)
                .WithMany(u => u.FriendsTo)
                .UsingEntity<UserFriend>(
                    right => right
                            .HasOne(uf => uf.FromUser)
                            .WithMany()
                            .HasForeignKey(uf => uf.FromUserId),
                    left => left
                            .HasOne(uf => uf.ToUser)
                            .WithMany()
                            .HasForeignKey(uf => uf.ToUserId),
                    j => j.ToTable("UserFriends"));
            modelBuilder.Entity<User>()
                .HasOne(u => u.CurrentRace)
                .WithMany(r => r.Racers)
                .HasForeignKey(u => u.CurrentRaceId)
                .IsRequired(false);

            // User-Race
            modelBuilder.Entity<UserRaceRecord>().HasKey(ur => new { ur.UserId, ur.RaceId });

            // User-race mistakes
            modelBuilder.Entity<UserRaceMistake>().HasKey(urm => new { urm.UserId, urm.RaceId, urm.Word });
            modelBuilder.Entity<UserRaceMistake>()
                .HasOne(urm => urm.UserRace)
                .WithMany(ur => ur.Mistakes)
                .HasForeignKey(urm => new { urm.UserId, urm.RaceId });
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Race> Races { get; set; }
        public DbSet<UserRaceRecord> UserRaceRecords { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Quote> Quotes { get; set; }
    }
}
