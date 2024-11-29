﻿// <auto-generated />
using System;
using DMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoDMS.Migrations
{
    [DbContext(typeof(DemoDMSContext))]
    partial class DemoDMSContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("DMS.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("AuthorName")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<int>("Department")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Extension")
                        .HasColumnType("TEXT");

                    b.Property<int>("Faculty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FilePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileType")
                        .HasColumnType("TEXT");

                    b.Property<int>("Level")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParentId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PublicationDate")
                        .HasColumnType("TEXT");

                    b.Property<double?>("Size")
                        .HasColumnType("REAL");

                    b.Property<string>("SupervisorName")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Document");
                });

            modelBuilder.Entity("DMS.Models.Folder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("DateCreated")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateModified")
                        .HasColumnType("TEXT");

                    b.Property<int>("Department")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Faculty")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<int>("ParentId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Folder");
                });
#pragma warning restore 612, 618
        }
    }
}