﻿using System;
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
    public virtual DbSet<Stage> Stages { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var builder = new ConfigurationBuilder();
        builder.SetBasePath(Directory.GetCurrentDirectory());
        builder.AddJsonFile("appsettings.json");
        var config = builder.Build();
        string connectionString = config.GetConnectionString("connection");
        optionsBuilder.UseNpgsql(connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Games_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Status).HasColumnName("status");
            entity.Property(e => e.Stage).HasColumnName("stage");
            entity.Property(e => e.Date).HasColumnName("date");

            entity.HasOne(d => d.StageNavigation).WithMany(p => p.Games)
                .HasForeignKey(d => d.Stage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("Game_stage_fkey");
        });

        modelBuilder.Entity<Stage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Stages_pkey");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Desc).HasColumnName("desc");
            entity.Property(e => e.PicturePath).HasColumnName("picturePath");
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
            entity.Property(e => e.Stage).HasColumnName("stage");
            entity.HasOne(d => d.StageNavigation).WithMany(p => p.LocalSolutions)
                .HasForeignKey(d => d.Stage)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("LocalSolution_stage_fkey");
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

            entity.HasOne(r => r.StageNavigation)
                .WithMany(s => s.Rounds)
                .HasForeignKey(f => f.Stage)
                .HasConstraintName("Rounds_stage_fkey");
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

            entity.HasMany(u => u.RoundUsers)
                .WithOne(u => u.UserNavigation).OnDelete(DeleteBehavior.SetNull);
            
            entity.HasMany(u => u.GameUsers)
                .WithOne(u => u.UserNavigation).OnDelete(DeleteBehavior.SetNull);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
