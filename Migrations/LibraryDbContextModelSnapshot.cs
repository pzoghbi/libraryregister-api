﻿// <auto-generated />
using System;
using LibraryRegister.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LibraryRegister.Migrations
{
    [DbContext(typeof(LibraryDbContext))]
    partial class LibraryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("LibraryRegister.Models.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Author");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Unknown Author"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Dale Carnegie"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Richard Koch"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Robert Kiyosaki"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Wim Hof"
                        },
                        new
                        {
                            Id = 6,
                            Name = "Julian Šribar, Boris Motik"
                        });
                });

            modelBuilder.Entity("LibraryRegister.Models.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AuthorId")
                        .HasColumnType("int");

                    b.Property<string>("Genre")
                        .HasColumnType("longtext");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("AuthorId");

                    b.ToTable("Book");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AuthorId = 2,
                            IsAvailable = false,
                            Title = "How to Win Friends and Influence People"
                        },
                        new
                        {
                            Id = 2,
                            AuthorId = 3,
                            IsAvailable = false,
                            Title = "The 80/20 Principle"
                        },
                        new
                        {
                            Id = 3,
                            AuthorId = 4,
                            IsAvailable = true,
                            Title = "Rich Dad Poor Dad"
                        },
                        new
                        {
                            Id = 4,
                            AuthorId = 2,
                            IsAvailable = true,
                            Title = "Communicate your way to success"
                        },
                        new
                        {
                            Id = 5,
                            AuthorId = 5,
                            IsAvailable = true,
                            Title = "Metoda Wim Hof"
                        },
                        new
                        {
                            Id = 6,
                            AuthorId = 6,
                            IsAvailable = true,
                            Title = "Demistificirani C++"
                        });
                });

            modelBuilder.Entity("LibraryRegister.Models.Leasing", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<DateTime>("LeaseDate")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("UserId");

                    b.ToTable("Leasing");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BookId = 1,
                            LeaseDate = new DateTime(2022, 10, 20, 11, 55, 23, 409, DateTimeKind.Utc).AddTicks(907),
                            UserId = 1001
                        },
                        new
                        {
                            Id = 2,
                            BookId = 2,
                            LeaseDate = new DateTime(2022, 10, 20, 11, 55, 23, 409, DateTimeKind.Utc).AddTicks(911),
                            UserId = 1001
                        });
                });

            modelBuilder.Entity("LibraryRegister.Models.Membership", b =>
                {
                    b.Property<Guid>("Guid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("ValidUntil")
                        .HasColumnType("datetime(6)");

                    b.HasKey("Guid");

                    b.HasIndex("UserId");

                    b.ToTable("Membership");
                });

            modelBuilder.Entity("LibraryRegister.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Id = 1001,
                            Email = "default@email.com",
                            Name = "Default User"
                        });
                });

            modelBuilder.Entity("LibraryRegister.Models.Book", b =>
                {
                    b.HasOne("LibraryRegister.Models.Author", "Author")
                        .WithMany()
                        .HasForeignKey("AuthorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Author");
                });

            modelBuilder.Entity("LibraryRegister.Models.Leasing", b =>
                {
                    b.HasOne("LibraryRegister.Models.Book", "Book")
                        .WithMany()
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("LibraryRegister.Models.User", null)
                        .WithMany("Leasings")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("LibraryRegister.Models.Membership", b =>
                {
                    b.HasOne("LibraryRegister.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("LibraryRegister.Models.User", b =>
                {
                    b.Navigation("Leasings");
                });
#pragma warning restore 612, 618
        }
    }
}
