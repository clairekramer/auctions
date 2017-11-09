using Microsoft.EntityFrameworkCore;

namespace auctions.Models {
    public class AuctionContext : DbContext {
        public AuctionContext(DbContextOptions<AuctionContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<AuctionItem> AuctionItems { get; set; }
        public DbSet<Bid> Bids { get; set; }
    }
}