﻿// <auto-generated />
using Excuses.Persistence.EFCore.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Excuses.Persistence.EFCore.Migrations
{
    [DbContext(typeof(ApiDbContext))]
    partial class ApiDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("Excuses.Persistence.Shared.Models.Excuse", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Category")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("TEXT");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Excuses");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Category = "work",
                            Text = "My computer exploded."
                        },
                        new
                        {
                            Id = 2,
                            Category = "work",
                            Text = "I got stuck in an endless email loop."
                        },
                        new
                        {
                            Id = 3,
                            Category = "work",
                            Text = "My pet python is shedding and needs supervision."
                        },
                        new
                        {
                            Id = 4,
                            Category = "school",
                            Text = "Aliens abducted my homework."
                        },
                        new
                        {
                            Id = 5,
                            Category = "school",
                            Text = "The dog ate my laptop."
                        },
                        new
                        {
                            Id = 6,
                            Category = "school",
                            Text = "I got lost in the metaverse and couldn't escape."
                        },
                        new
                        {
                            Id = 7,
                            Category = "social",
                            Text = "I accidentally set my alarm for PM instead of AM."
                        },
                        new
                        {
                            Id = 8,
                            Category = "social",
                            Text = "I was practicing social distancing... from everyone."
                        },
                        new
                        {
                            Id = 9,
                            Category = "social",
                            Text = "My grandma challenged me to a gaming tournament."
                        },
                        new
                        {
                            Id = 10,
                            Category = "technology",
                            Text = "My Wi-Fi ran out of data."
                        },
                        new
                        {
                            Id = 11,
                            Category = "technology",
                            Text = "The internet was too slow to function."
                        },
                        new
                        {
                            Id = 12,
                            Category = "technology",
                            Text = "I mistakenly set my phone to 'Airplane Mode' and it flew away."
                        },
                        new
                        {
                            Id = 13,
                            Category = "pets",
                            Text = "My cat hid my car keys."
                        },
                        new
                        {
                            Id = 14,
                            Category = "pets",
                            Text = "My dog locked me out."
                        },
                        new
                        {
                            Id = 15,
                            Category = "pets",
                            Text = "My parrot changed my password and won’t tell me what it is."
                        },
                        new
                        {
                            Id = 16,
                            Category = "general",
                            Text = "Gravity stopped working for me temporarily."
                        },
                        new
                        {
                            Id = 17,
                            Category = "general",
                            Text = "I got stuck in an existential crisis."
                        },
                        new
                        {
                            Id = 18,
                            Category = "general",
                            Text = "I was time-traveling and lost track of reality."
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
