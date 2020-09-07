﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SmartBuy.Administration.Infrastructure;

namespace SmartBuy.Administration.Infrastructure.Migrations
{
    [DbContext(typeof(AdministrationContext))]
    [Migration("20200806151509_administration_init")]
    partial class administration_init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("Administrator")
                .HasAnnotation("ProductVersion", "3.1.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SmartBuy.Administration.Domain.GasStation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(200)")
                        .HasMaxLength(200);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("GasStations");
                });

            modelBuilder.Entity("SmartBuy.Administration.Domain.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SmartBuy.Administration.Domain.Tank", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Bottom")
                        .HasColumnType("int");

                    b.Property<Guid>("GasStationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)")
                        .HasMaxLength(50);

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<int>("ProductId")
                        .HasColumnType("int");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<int>("Top")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GasStationId");

                    b.HasIndex("ProductId");

                    b.ToTable("Tanks");
                });

            modelBuilder.Entity("SmartBuy.Administration.Domain.TankReading", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("ReadingTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TankId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TankId");

                    b.ToTable("TankReadings");
                });

            modelBuilder.Entity("SmartBuy.Administration.Domain.TankSale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<DateTime>("SaleTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TankId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TankId");

                    b.ToTable("TankSales");
                });

            modelBuilder.Entity("SmartBuy.Administration.Domain.Tank", b =>
                {
                    b.HasOne("SmartBuy.Administration.Domain.GasStation", null)
                        .WithMany("Tanks")
                        .HasForeignKey("GasStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SmartBuy.Administration.Domain.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("SmartBuy.SharedKernel.ValueObjects.Capacity", "Capacity", b1 =>
                        {
                            b1.Property<int>("TankId")
                                .ValueGeneratedOnAdd()
                                .HasColumnType("int")
                                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                            b1.Property<int>("NetQuantity")
                                .HasColumnName("NetQuantity")
                                .HasColumnType("int");

                            b1.Property<int>("Unit")
                                .HasColumnName("Unit")
                                .HasColumnType("int");

                            b1.HasKey("TankId");

                            b1.ToTable("Tanks");

                            b1.WithOwner()
                                .HasForeignKey("TankId");
                        });
                });

            modelBuilder.Entity("SmartBuy.Administration.Domain.TankReading", b =>
                {
                    b.HasOne("SmartBuy.Administration.Domain.Tank", null)
                        .WithMany()
                        .HasForeignKey("TankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SmartBuy.Administration.Domain.TankSale", b =>
                {
                    b.HasOne("SmartBuy.Administration.Domain.Tank", null)
                        .WithMany()
                        .HasForeignKey("TankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}