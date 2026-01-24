using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PickleballClubManagement.Models;

namespace PickleballClubManagement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Challenge> Challenges { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        
        // Advanced Tables
        public DbSet<News> News { get; set; }
        public DbSet<Court> Courts { get; set; }
        public DbSet<TransactionCategory> TransactionCategories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Participant> Participants { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure table names
            builder.Entity<Member>().ToTable("186_Members");
            builder.Entity<Challenge>().ToTable("186_Challenges");
            builder.Entity<Match>().ToTable("186_Matches");
            builder.Entity<Booking>().ToTable("186_Bookings");
            
            // Advanced Tables Configuration
            builder.Entity<News>().ToTable("186_News");
            builder.Entity<Court>().ToTable("186_Courts");
            builder.Entity<TransactionCategory>().ToTable("186_TransactionCategories");
            builder.Entity<Transaction>().ToTable("186_Transactions");
            builder.Entity<Participant>().ToTable("186_Participants");

            // Configure Member relationships
            builder.Entity<Member>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);
            builder.Entity<Member>()
                .HasOne(m => m.User)
                .WithMany()
                .HasForeignKey(m => m.IdentityUserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Challenge relationships
            builder.Entity<Challenge>()
                .HasOne(c => c.Creator)
                .WithMany(m => m.CreatedChallenges)
                .HasForeignKey(c => c.CreatorId)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Match relationships - prevent cascade delete conflicts
            builder.Entity<Match>()
                .HasOne(m => m.Challenge)
                .WithMany(c => c.Matches)
                .HasForeignKey(m => m.ChallengeId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Entity<Match>()
                .HasOne(m => m.Winner1)
                .WithMany()
                .HasForeignKey(m => m.Winner1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(m => m.Winner2)
                .WithMany()
                .HasForeignKey(m => m.Winner2Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(m => m.Loser1)
                .WithMany()
                .HasForeignKey(m => m.Loser1Id)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Match>()
                .HasOne(m => m.Loser2)
                .WithMany()
                .HasForeignKey(m => m.Loser2Id)
                .OnDelete(DeleteBehavior.Restrict);

            // Configure Booking relationships
            builder.Entity<Booking>()
                .HasOne(b => b.Member)
                .WithMany(m => m.Bookings)
                .HasForeignKey(b => b.MemberId)
                .OnDelete(DeleteBehavior.Cascade);

            // Seed initial data
            SeedData(builder);
        }

        private void SeedData(ModelBuilder modelBuilder)
        {
            // Sample members will be created after Identity users are registered
            // This is just a placeholder for future seeding if needed
        }
    }
}
