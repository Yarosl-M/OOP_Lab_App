﻿// <auto-generated />
using System;
using AT_Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AT_Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20231117091134_AddAdminAndSubscribedFlags")]
    partial class AddAdminAndSubscribedFlags
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.24");

            modelBuilder.Entity("AT_Domain.Models.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsAdmin")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsSubscribed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("TEXT");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("8d76de5e-8e04-44a9-ae0b-c7f0014bf5f7"),
                            CreatedAt = new DateTime(2023, 11, 17, 9, 11, 34, 330, DateTimeKind.Utc).AddTicks(9595),
                            IsAdmin = false,
                            IsSubscribed = false,
                            Password = "$2a$11$ZkcgTqTH4uHiLNtSLikmTehXCioYUaVBiDFMEmPra9iSBaE73iwHy",
                            Username = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
