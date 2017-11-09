using System;
using System.ComponentModel.DataAnnotations;

namespace auctions.Models {
    public class Bid : BaseEntity {
        public int BidId { get; set; }
        public int Amount { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int ItemId { get; set; }
        public AuctionItem AuctionItem { get; set; }
    }
}