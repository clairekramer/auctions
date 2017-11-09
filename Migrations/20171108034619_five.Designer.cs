using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using auctions.Models;

namespace auctions.Migrations
{
    [DbContext(typeof(AuctionContext))]
    [Migration("20171108034619_five")]
    partial class five
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.2");

            modelBuilder.Entity("auctions.Models.AuctionItem", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description");

                    b.Property<DateTime>("EndDate");

                    b.Property<string>("Name");

                    b.Property<int>("StartingBid");

                    b.Property<int>("UserId");

                    b.HasKey("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("AuctionItems");
                });

            modelBuilder.Entity("auctions.Models.Bid", b =>
                {
                    b.Property<int>("BidId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<int>("ItemId");

                    b.Property<int>("UserId");

                    b.HasKey("BidId");

                    b.HasIndex("ItemId");

                    b.HasIndex("UserId");

                    b.ToTable("Bids");
                });

            modelBuilder.Entity("auctions.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConfirmPW");

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password");

                    b.Property<string>("Username")
                        .HasMaxLength(20);

                    b.Property<int>("Wallet");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("auctions.Models.AuctionItem", b =>
                {
                    b.HasOne("auctions.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("auctions.Models.Bid", b =>
                {
                    b.HasOne("auctions.Models.AuctionItem", "AuctionItem")
                        .WithMany("Bids")
                        .HasForeignKey("ItemId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("auctions.Models.User", "User")
                        .WithMany("Bids")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
