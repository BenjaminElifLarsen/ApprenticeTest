﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Catering.Shared.IPL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Catering.Shared.Migrations
{
    [DbContext(typeof(CateringContext))]
    [Migration("20240411122049_menu_item_price")]
    partial class menu_item_price
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("CateringDataProcessingPlatform.DL.Models.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.ComplexProperty<Dictionary<string, object>>("Location", "CateringDataProcessingPlatform.DL.Models.Customer.Location#CustomerLocation", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("City")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<string>("Street")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");
                        });

                    b.HasKey("Id");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("CateringDataProcessingPlatform.DL.Models.Dish", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Dishes");
                });

            modelBuilder.Entity("CateringDataProcessingPlatform.DL.Models.Menu", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Menus");
                });

            modelBuilder.Entity("CateringDataProcessingPlatform.DL.Models.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.ComplexProperty<Dictionary<string, object>>("Customer", "CateringDataProcessingPlatform.DL.Models.Order.Customer#ReferenceId", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Menu", "CateringDataProcessingPlatform.DL.Models.Order.Menu#ReferenceId", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<Guid>("Id")
                                .HasColumnType("uniqueidentifier");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Time", "CateringDataProcessingPlatform.DL.Models.Order.Time#OrderTime", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<DateTime>("Time")
                                .HasColumnType("datetime2");
                        });

                    b.HasKey("Id");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("CateringDataProcessingPlatform.DL.Models.Customer", b =>
                {
                    b.OwnsMany("Shared.DDD.ReferenceId", "Orders", b1 =>
                        {
                            b1.Property<Guid>("CustomerId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("CustomerId", "Id");

                            b1.ToTable("Customers_Orders");

                            b1.WithOwner()
                                .HasForeignKey("CustomerId");
                        });

                    b.Navigation("Orders");
                });

            modelBuilder.Entity("CateringDataProcessingPlatform.DL.Models.Dish", b =>
                {
                    b.OwnsMany("Shared.DDD.ReferenceId", "Menues", b1 =>
                        {
                            b1.Property<Guid>("DishId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<Guid>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("uniqueidentifier");

                            b1.HasKey("DishId", "Id");

                            b1.ToTable("Dishes_Menues");

                            b1.WithOwner()
                                .HasForeignKey("DishId");
                        });

                    b.Navigation("Menues");
                });

            modelBuilder.Entity("CateringDataProcessingPlatform.DL.Models.Menu", b =>
                {
                    b.OwnsMany("CateringDataProcessingPlatform.DL.Models.MenuPart", "Parts", b1 =>
                        {
                            b1.Property<Guid>("MenuId")
                                .HasColumnType("uniqueidentifier");

                            b1.Property<int>("Id")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int");

                            SqlServerPropertyBuilderExtensions.UseIdentityColumn(b1.Property<int>("Id"));

                            b1.Property<long>("Amount")
                                .HasColumnType("bigint");

                            b1.Property<string>("Name")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)");

                            b1.Property<float>("Price")
                                .HasColumnType("real");

                            b1.HasKey("MenuId", "Id");

                            b1.ToTable("MenuPart");

                            b1.WithOwner()
                                .HasForeignKey("MenuId");

                            b1.OwnsOne("Shared.DDD.ReferenceId", "Dish", b2 =>
                                {
                                    b2.Property<Guid>("MenuPartMenuId")
                                        .HasColumnType("uniqueidentifier");

                                    b2.Property<int>("MenuPartId")
                                        .HasColumnType("int");

                                    b2.Property<Guid>("Id")
                                        .HasColumnType("uniqueidentifier");

                                    b2.HasKey("MenuPartMenuId", "MenuPartId");

                                    b2.ToTable("MenuPart");

                                    b2.WithOwner()
                                        .HasForeignKey("MenuPartMenuId", "MenuPartId");
                                });

                            b1.Navigation("Dish")
                                .IsRequired();
                        });

                    b.Navigation("Parts");
                });
#pragma warning restore 612, 618
        }
    }
}