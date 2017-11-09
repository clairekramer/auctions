using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using auctions.Models;

namespace auctions.Models {
    public class AuctionItem : BaseEntity {
        [Key]
        public int ItemId { get; set; }
        [MinLength(3)]
        [Display(Name = "Product Name:")]
        public string Name { get; set; }
        [MinLength(10)]
        [Display(Name = "Description:")]
        public string Description { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Starting Bid:")]
        public int StartingBid { get; set; }
        [Display(Name = "End Date:")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public List<Bid> Bids { get; set; }
        public AuctionItem() {
            Bids = new List<Bid>();
        }
    }
}