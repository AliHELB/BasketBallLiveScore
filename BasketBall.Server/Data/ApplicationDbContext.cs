using Microsoft.EntityFrameworkCore;
using BasketBall.Server.Models;

namespace BasketBall.Server.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<StartingFive> StartingFives { get; set; }
        public DbSet<TimerMatch> TimerMatches { get; set; }
        public DbSet<BasketEvent> BasketEvents { get; set; }
        public DbSet<FaultEvent> FaultEvents { get; set; }
        public DbSet<PlayerSubstitutionEvent> PlayerSubstitutionEvents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configuration des relations et des clés étrangères
            modelBuilder.Entity<Player>()
                .HasOne(p => p.Team)
                .WithMany()
                .HasForeignKey(p => p.TeamId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.CreatedByUser)
                .WithMany()
                .HasForeignKey(m => m.CreatedByUserId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.HomeTeam)
                .WithMany()
                .HasForeignKey(m => m.HomeTeamId);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.AwayTeam)
                .WithMany()
                .HasForeignKey(m => m.AwayTeamId);

            modelBuilder.Entity<StartingFive>()
                .HasOne(sf => sf.Match)
                .WithMany()
                .HasForeignKey(sf => sf.MatchId);

            modelBuilder.Entity<StartingFive>()
                .HasOne(sf => sf.Team)
                .WithMany()
                .HasForeignKey(sf => sf.TeamId);

            modelBuilder.Entity<StartingFive>()
                .HasOne(sf => sf.Player)
                .WithMany()
                .HasForeignKey(sf => sf.PlayerId);

            modelBuilder.Entity<TimerMatch>()
                .HasKey(t => t.TimerMatchId);

            modelBuilder.Entity<TimerMatch>()
                .HasOne<Match>()
                .WithMany()
                .HasForeignKey(t => t.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerSubstitutionEvent>()
                .HasOne<Match>()
                .WithMany()
                .HasForeignKey(e => e.MatchId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerSubstitutionEvent>()
                .HasOne<Player>()
                .WithMany()
                .HasForeignKey(e => e.PlayerOutId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerSubstitutionEvent>()
                .HasOne<Player>()
                .WithMany()
                .HasForeignKey(e => e.PlayerInId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }
    }
}
