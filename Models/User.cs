using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace auctions.Models {
    public class User : BaseEntity {
        [Key]
        public int UserId { get; set; }
        [Required(ErrorMessage = "*Required")]
        [Display(Name = "First Name:")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "*Required")]
        [Display(Name = "Last Name:")]
        public string LastName { get; set; }
        [MinLength(3, ErrorMessage = "Must be longer than 3 Characters")]
        [MaxLength(20, ErrorMessage = "Cannot be longer than 20 Characters")]
        [Display(Name = "Username:")]
        public string Username { get; set; }
        [MinLength(8)]
        [Display(Name = "Password:")]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage="Passwords do not match")]
        [Display(Name = "Confirm Password:")]
        public string ConfirmPW { get; set; }
        public int Wallet { get; set; }
        
        public List<Bid> Bids { get; set; }
        public User() {
            Bids = new List<Bid>();
        }
    }
}