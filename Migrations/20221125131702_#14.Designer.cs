﻿// <auto-generated />
using System;
using DMS.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DemoDMS.Migrations
{
    [DbContext(typeof(DemoDMSContext))]
    [Migration("20221125131702_#14")]
    partial class _14
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.0");

            modelBuilder.Entity("DemoDMS.Models.Document", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Category")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Extension")
                        .HasColumnType("TEXT");

                    b.Property<string>("FilePath")
                        .HasColumnType("TEXT");

                    b.Property<string>("FileType")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("ModifiedDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<double?>("Size")
                        .HasColumnType("REAL");

                    b.Property<DateTime>("UploadDate")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Document");
                });
#pragma warning restore 612, 618
        }
    }
}
