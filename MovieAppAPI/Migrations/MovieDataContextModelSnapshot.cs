﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MovieAppAPI.Data;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace MovieAppAPI.Migrations
{
    [DbContext(typeof(MovieDataContext))]
    partial class MovieDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.Property<Guid>("GenresId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MoviesId")
                        .HasColumnType("uuid");

                    b.HasKey("GenresId", "MoviesId");

                    b.HasIndex("MoviesId");

                    b.ToTable("GenreMovie");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Auth.InvalidToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("invalid_token");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Country", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CountryName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("country");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Genre", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("genre");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Movie", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<int>("AgeLimit")
                        .HasColumnType("integer");

                    b.Property<int?>("Budget")
                        .HasColumnType("integer");

                    b.Property<Guid?>("CountryId")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasColumnType("text");

                    b.Property<string>("Director")
                        .HasColumnType("text");

                    b.Property<int?>("Fees")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Poster")
                        .HasColumnType("text");

                    b.Property<string>("Tagline")
                        .HasColumnType("text");

                    b.Property<TimeSpan>("Time")
                        .HasColumnType("interval");

                    b.Property<int>("Year")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("movie");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Review", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreateDateTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsAnonymous")
                        .HasColumnType("boolean");

                    b.Property<Guid>("MovieId")
                        .HasColumnType("uuid");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<string>("ReviewText")
                        .HasColumnType("text");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid")
                        .HasColumnName("Author");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("MovieId", "UserId")
                        .IsUnique();

                    b.ToTable("review");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Users.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Gender")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserName")
                        .IsUnique();

                    b.ToTable("user");
                });

            modelBuilder.Entity("GenreMovie", b =>
                {
                    b.HasOne("MovieAppAPI.Entities.Genre", null)
                        .WithMany()
                        .HasForeignKey("GenresId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieAppAPI.Entities.Movie", null)
                        .WithMany()
                        .HasForeignKey("MoviesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Movie", b =>
                {
                    b.HasOne("MovieAppAPI.Entities.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId");

                    b.Navigation("Country");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Review", b =>
                {
                    b.HasOne("MovieAppAPI.Entities.Movie", "Movie")
                        .WithMany("Reviews")
                        .HasForeignKey("MovieId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MovieAppAPI.Entities.Users.User", "Author")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MovieAppAPI.Entities.Movie", b =>
                {
                    b.Navigation("Reviews");
                });
#pragma warning restore 612, 618
        }
    }
}
