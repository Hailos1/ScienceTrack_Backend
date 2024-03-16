﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using ScienceTrack;

#nullable disable

namespace ScienceTrack.Migrations
{
    [DbContext(typeof(ScienceTrackContext))]
    partial class ScienceTrackContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("ScienceTrack.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("Date")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("date");

                    b.Property<int?>("Stage")
                        .HasColumnType("integer")
                        .HasColumnName("stage");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("Games_pkey");

                    b.HasIndex("Stage");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("ScienceTrack.Models.GameUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdministrativeStatus")
                        .HasColumnType("integer")
                        .HasColumnName("administrativeStatus");

                    b.Property<int?>("FinanceStatus")
                        .HasColumnType("integer")
                        .HasColumnName("financeStatus");

                    b.Property<int>("Game")
                        .HasColumnType("integer")
                        .HasColumnName("game");

                    b.Property<int?>("SocialStatus")
                        .HasColumnType("integer")
                        .HasColumnName("socialStatus");

                    b.Property<int?>("User")
                        .HasColumnType("integer")
                        .HasColumnName("user");

                    b.HasKey("Id")
                        .HasName("GameUsers_pkey");

                    b.HasIndex("Game");

                    b.HasIndex("User");

                    b.ToTable("GameUsers");
                });

            modelBuilder.Entity("ScienceTrack.Models.GlobalEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdministrativeStatus")
                        .HasColumnType("integer")
                        .HasColumnName("administrativeStatus");

                    b.Property<decimal?>("Chance")
                        .HasColumnType("numeric")
                        .HasColumnName("chance");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("FinanceStatus")
                        .HasColumnType("integer")
                        .HasColumnName("financeStatus");

                    b.Property<int?>("SocialStatus")
                        .HasColumnType("integer")
                        .HasColumnName("socialStatus");

                    b.HasKey("Id")
                        .HasName("GlobalEvents_pkey");

                    b.ToTable("GlobalEvents");
                });

            modelBuilder.Entity("ScienceTrack.Models.LocalEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdministrativeStatus")
                        .HasColumnType("integer")
                        .HasColumnName("administrativeStatus");

                    b.Property<decimal?>("Chance")
                        .HasColumnType("numeric")
                        .HasColumnName("chance");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("FinanceStatus")
                        .HasColumnType("integer")
                        .HasColumnName("financeStatus");

                    b.Property<int?>("SocialStatus")
                        .HasColumnType("integer")
                        .HasColumnName("socialStatus");

                    b.HasKey("Id")
                        .HasName("LocalEvents_pkey");

                    b.ToTable("LocalEvents");
                });

            modelBuilder.Entity("ScienceTrack.Models.LocalSolution", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("AdministrativeStatus")
                        .HasColumnType("integer")
                        .HasColumnName("administrativeStatus");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<int?>("FinanceStatus")
                        .HasColumnType("integer")
                        .HasColumnName("financeStatus");

                    b.Property<int?>("SocialStatus")
                        .HasColumnType("integer")
                        .HasColumnName("socialStatus");

                    b.Property<int?>("Stage")
                        .HasColumnType("integer")
                        .HasColumnName("stage");

                    b.HasKey("Id")
                        .HasName("LocalSolutions_pkey");

                    b.HasIndex("Stage");

                    b.ToTable("LocalSolutions");
                });

            modelBuilder.Entity("ScienceTrack.Models.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("roleName");

                    b.HasKey("Id")
                        .HasName("Roles_pkey");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("ScienceTrack.Models.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("Game")
                        .HasColumnType("integer")
                        .HasColumnName("game");

                    b.Property<int>("GlobalEvent")
                        .HasColumnType("integer")
                        .HasColumnName("globalEvent");

                    b.Property<int?>("Stage")
                        .HasColumnType("integer");

                    b.Property<string>("Status")
                        .HasColumnType("text")
                        .HasColumnName("status");

                    b.HasKey("Id")
                        .HasName("Rounds_pkey");

                    b.HasIndex("Game");

                    b.HasIndex("GlobalEvent");

                    b.HasIndex("Stage");

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("ScienceTrack.Models.RoundUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("LocalEvent")
                        .HasColumnType("integer")
                        .HasColumnName("localEvent");

                    b.Property<int?>("LocalSolution")
                        .HasColumnType("integer")
                        .HasColumnName("localSolution");

                    b.Property<int>("Round")
                        .HasColumnType("integer")
                        .HasColumnName("round");

                    b.Property<int?>("User")
                        .HasColumnType("integer")
                        .HasColumnName("user");

                    b.HasKey("Id")
                        .HasName("RoundUsers_pkey");

                    b.HasIndex("LocalEvent");

                    b.HasIndex("LocalSolution");

                    b.HasIndex("Round");

                    b.HasIndex("User");

                    b.ToTable("RoundUsers");
                });

            modelBuilder.Entity("ScienceTrack.Models.Stage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Desc")
                        .HasColumnType("text")
                        .HasColumnName("desc");

                    b.Property<string>("PicturePath")
                        .HasColumnType("text")
                        .HasColumnName("picturePath");

                    b.Property<int>("RoundDuration")
                        .HasColumnType("integer");

                    b.HasKey("Id")
                        .HasName("Stages_pkey");

                    b.ToTable("Stages");
                });

            modelBuilder.Entity("ScienceTrack.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("OfficialName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("officialName");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("passwordHash");

                    b.Property<int?>("Role")
                        .HasColumnType("integer")
                        .HasColumnName("role");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("userName");

                    b.HasKey("Id")
                        .HasName("Users_pkey");

                    b.HasIndex("Role");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ScienceTrack.Models.Game", b =>
                {
                    b.HasOne("ScienceTrack.Models.Stage", "StageNavigation")
                        .WithMany("Games")
                        .HasForeignKey("Stage")
                        .HasConstraintName("Game_stage_fkey");

                    b.Navigation("StageNavigation");
                });

            modelBuilder.Entity("ScienceTrack.Models.GameUser", b =>
                {
                    b.HasOne("ScienceTrack.Models.Game", "GameNavigation")
                        .WithMany("GameUsers")
                        .HasForeignKey("Game")
                        .IsRequired()
                        .HasConstraintName("GameUsers_game_fkey");

                    b.HasOne("ScienceTrack.Models.User", "UserNavigation")
                        .WithMany("GameUsers")
                        .HasForeignKey("User")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("GameUsers_user_fkey");

                    b.Navigation("GameNavigation");

                    b.Navigation("UserNavigation");
                });

            modelBuilder.Entity("ScienceTrack.Models.LocalSolution", b =>
                {
                    b.HasOne("ScienceTrack.Models.Stage", "StageNavigation")
                        .WithMany("LocalSolutions")
                        .HasForeignKey("Stage")
                        .HasConstraintName("LocalSolution_stage_fkey");

                    b.Navigation("StageNavigation");
                });

            modelBuilder.Entity("ScienceTrack.Models.Round", b =>
                {
                    b.HasOne("ScienceTrack.Models.Game", "GameNavigation")
                        .WithMany("Rounds")
                        .HasForeignKey("Game")
                        .IsRequired()
                        .HasConstraintName("Rounds_game_fkey");

                    b.HasOne("ScienceTrack.Models.GlobalEvent", "GlobalEventNavigation")
                        .WithMany("Rounds")
                        .HasForeignKey("GlobalEvent")
                        .IsRequired()
                        .HasConstraintName("Rounds_globalEvent_fkey");

                    b.HasOne("ScienceTrack.Models.Stage", "StageNavigation")
                        .WithMany("Rounds")
                        .HasForeignKey("Stage")
                        .HasConstraintName("Rounds_stage_fkey");

                    b.Navigation("GameNavigation");

                    b.Navigation("GlobalEventNavigation");

                    b.Navigation("StageNavigation");
                });

            modelBuilder.Entity("ScienceTrack.Models.RoundUser", b =>
                {
                    b.HasOne("ScienceTrack.Models.LocalEvent", "LocalEventNavigation")
                        .WithMany("RoundUsers")
                        .HasForeignKey("LocalEvent")
                        .IsRequired()
                        .HasConstraintName("RoundUsers_localEvent_fkey");

                    b.HasOne("ScienceTrack.Models.LocalSolution", "LocalSolutionNavigation")
                        .WithMany("RoundUsers")
                        .HasForeignKey("LocalSolution")
                        .HasConstraintName("RoundUsers_localSolution_fkey");

                    b.HasOne("ScienceTrack.Models.Round", "RoundNavigation")
                        .WithMany("RoundUsers")
                        .HasForeignKey("Round")
                        .IsRequired()
                        .HasConstraintName("RoundUsers_round_fkey");

                    b.HasOne("ScienceTrack.Models.User", "UserNavigation")
                        .WithMany("RoundUsers")
                        .HasForeignKey("User")
                        .OnDelete(DeleteBehavior.SetNull)
                        .HasConstraintName("RoundUsers_user_fkey");

                    b.Navigation("LocalEventNavigation");

                    b.Navigation("LocalSolutionNavigation");

                    b.Navigation("RoundNavigation");

                    b.Navigation("UserNavigation");
                });

            modelBuilder.Entity("ScienceTrack.Models.User", b =>
                {
                    b.HasOne("ScienceTrack.Models.Role", "RoleNavigation")
                        .WithMany("Users")
                        .HasForeignKey("Role")
                        .HasConstraintName("Users_role_fkey");

                    b.Navigation("RoleNavigation");
                });

            modelBuilder.Entity("ScienceTrack.Models.Game", b =>
                {
                    b.Navigation("GameUsers");

                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("ScienceTrack.Models.GlobalEvent", b =>
                {
                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("ScienceTrack.Models.LocalEvent", b =>
                {
                    b.Navigation("RoundUsers");
                });

            modelBuilder.Entity("ScienceTrack.Models.LocalSolution", b =>
                {
                    b.Navigation("RoundUsers");
                });

            modelBuilder.Entity("ScienceTrack.Models.Role", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("ScienceTrack.Models.Round", b =>
                {
                    b.Navigation("RoundUsers");
                });

            modelBuilder.Entity("ScienceTrack.Models.Stage", b =>
                {
                    b.Navigation("Games");

                    b.Navigation("LocalSolutions");

                    b.Navigation("Rounds");
                });

            modelBuilder.Entity("ScienceTrack.Models.User", b =>
                {
                    b.Navigation("GameUsers");

                    b.Navigation("RoundUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
