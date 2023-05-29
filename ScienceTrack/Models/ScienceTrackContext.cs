using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using ScienceTrack.Models;

namespace ScienceTrack;

public partial class ScienceTrackContext : DbContext
{
    public ScienceTrackContext()
    {
    }

    public ScienceTrackContext(DbContextOptions<ScienceTrackContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameUser> GameUsers { get; set; }

    public virtual DbSet<GlobalEvent> GlobalEvents { get; set; }

    public virtual DbSet<LocalEvent> LocalEvents { get; set; }

    public virtual DbSet<LocalSolution> LocalSolutions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Round> Rounds { get; set; }

    public virtual DbSet<RoundUser> RoundUsers { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=science_track;Username=postgres;Password=563596");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Games_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status).HasColumnName("status");
        });

        modelBuilder.Entity<GameUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GameUsers_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdministrativeStatus).HasColumnName("administrativeStatus");
            entity.Property(e => e.FinanceStatus).HasColumnName("financeStatus");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.SocialStatus).HasColumnName("socialStatus");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.GameUsers)
                .HasForeignKey(d => d.Game)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GameUsers_game_fkey");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.GameUsers)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("GameUsers_user_fkey");
        });

        modelBuilder.Entity<GlobalEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("GlobalEvents_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdministrativeStatus).HasColumnName("administrativeStatus");
            entity.Property(e => e.Chance).HasColumnName("chance");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FinanceStatus).HasColumnName("financeStatus");
            entity.Property(e => e.SocialStatus).HasColumnName("socialStatus");
        });

        modelBuilder.Entity<LocalEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LocalEvents_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdministrativeStatus).HasColumnName("administrativeStatus");
            entity.Property(e => e.Chance).HasColumnName("chance");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FinanceStatus).HasColumnName("financeStatus");
            entity.Property(e => e.SocialStatus).HasColumnName("socialStatus");
        });

        modelBuilder.Entity<LocalSolution>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("LocalSolutions_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AdministrativeStatus).HasColumnName("administrativeStatus");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.FinanceStatus).HasColumnName("financeStatus");
            entity.Property(e => e.SocialStatus).HasColumnName("socialStatus");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Roles_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.RoleName).HasColumnName("roleName");
        });

        modelBuilder.Entity<Round>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Rounds_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Game).HasColumnName("game");
            entity.Property(e => e.GlobalEvent).HasColumnName("globalEvent");
            entity.Property(e => e.Status).HasColumnName("status");

            entity.HasOne(d => d.GameNavigation).WithMany(p => p.Rounds)
                .HasForeignKey(d => d.Game)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rounds_game_fkey");

            entity.HasOne(d => d.GlobalEventNavigation).WithMany(p => p.Rounds)
                .HasForeignKey(d => d.GlobalEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Rounds_globalEvent_fkey");
        });

        modelBuilder.Entity<RoundUser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("RoundUsers_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.LocalEvent).HasColumnName("localEvent");
            entity.Property(e => e.LocalSolution).HasColumnName("localSolution");
            entity.Property(e => e.Round).HasColumnName("round");
            entity.Property(e => e.User).HasColumnName("user");

            entity.HasOne(d => d.LocalEventNavigation).WithMany(p => p.RoundUsers)
                .HasForeignKey(d => d.LocalEvent)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoundUsers_localEvent_fkey");

            entity.HasOne(d => d.LocalSolutionNavigation).WithMany(p => p.RoundUsers)
                .HasForeignKey(d => d.LocalSolution)
                .HasConstraintName("RoundUsers_localSolution_fkey");

            entity.HasOne(d => d.RoundNavigation).WithMany(p => p.RoundUsers)
                .HasForeignKey(d => d.Round)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoundUsers_round_fkey");

            entity.HasOne(d => d.UserNavigation).WithMany(p => p.RoundUsers)
                .HasForeignKey(d => d.User)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("RoundUsers_user_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Users_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.PasswordHash).HasColumnName("passwordHash");
            entity.Property(e => e.Role).HasColumnName("role");
            entity.Property(e => e.UserName).HasColumnName("userName");
            entity.Property(e => e.OfficialName).HasColumnName("officialName");   

            entity.HasOne(d => d.RoleNavigation).WithMany(p => p.Users)
                .HasForeignKey(d => d.Role)
                .HasConstraintName("Users_role_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
