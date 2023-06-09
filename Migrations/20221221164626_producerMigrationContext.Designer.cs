﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Producer.Migrations
{
    [DbContext(typeof(ProducerContext))]
    [Migration("20221221164626_producerMigrationContext")]
    partial class producerMigrationContext
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GiftItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("InStock")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("GiftItems");
                });

            modelBuilder.Entity("GiftItemDetail", b =>
                {
                    b.Property<int>("GiftItemDetailID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("GiftItemDetailID"));

                    b.Property<int>("GiftItemId")
                        .HasColumnType("int");

                    b.Property<string>("Manufacturer")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("GiftItemDetailID");

                    b.HasIndex("GiftItemId")
                        .IsUnique();

                    b.ToTable("GiftItemDetails");
                });

            modelBuilder.Entity("GiftItemDetail", b =>
                {
                    b.HasOne("GiftItem", null)
                        .WithOne("giftItemDetails")
                        .HasForeignKey("GiftItemDetail", "GiftItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GiftItem", b =>
                {
                    b.Navigation("giftItemDetails");
                });
#pragma warning restore 612, 618
        }
    }
}
