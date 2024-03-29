﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PruebaTecnica.Database;

namespace PruebaTecnica.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.3-servicing-35854")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("PruebaTecnica.Entities.Client", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("PruebaTecnica.Entities.Invoice", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<decimal>("Balance");

                    b.Property<string>("ClientId");

                    b.Property<decimal>("Discount_total");

                    b.Property<decimal>("Subtotal");

                    b.Property<decimal>("Tax_total");

                    b.Property<decimal>("Total");

                    b.HasKey("id");

                    b.HasIndex("ClientId");

                    b.ToTable("Invoices");
                });

            modelBuilder.Entity("PruebaTecnica.Entities.Line", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Currency");

                    b.Property<decimal>("Discount_rate");

                    b.Property<int?>("Invoiceid");

                    b.Property<decimal>("Price");

                    b.Property<string>("Product");

                    b.Property<int>("Quantity");

                    b.Property<decimal>("Tax_rate");

                    b.HasKey("Id");

                    b.HasIndex("Invoiceid");

                    b.ToTable("Lines");
                });

            modelBuilder.Entity("PruebaTecnica.Entities.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("Invoiceid");

                    b.Property<decimal>("Total");

                    b.HasKey("Id");

                    b.HasIndex("Invoiceid");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("PruebaTecnica.Entities.Invoice", b =>
                {
                    b.HasOne("PruebaTecnica.Entities.Client", "Client")
                        .WithMany()
                        .HasForeignKey("ClientId");
                });

            modelBuilder.Entity("PruebaTecnica.Entities.Line", b =>
                {
                    b.HasOne("PruebaTecnica.Entities.Invoice")
                        .WithMany("Lines")
                        .HasForeignKey("Invoiceid");
                });

            modelBuilder.Entity("PruebaTecnica.Entities.Payment", b =>
                {
                    b.HasOne("PruebaTecnica.Entities.Invoice")
                        .WithMany("Payments")
                        .HasForeignKey("Invoiceid");
                });
#pragma warning restore 612, 618
        }
    }
}
