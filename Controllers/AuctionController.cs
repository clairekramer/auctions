using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using auctions.Models;
using System.Linq;

namespace auctions.Controllers {
    public class AuctionController : Controller {
        public AuctionContext _context;
        public AuctionController(AuctionContext context) {
            _context = context;
        }

        [HttpGet]
        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = _context.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            List<AuctionItem> AllItems = _context.AuctionItems
                                        .Include(i => i.Bids)
                                            .ThenInclude(b => b.User)
                                        .Include(i => i.User)
                                        .OrderBy(i => i.EndDate)
                                        .ToList();
            foreach (var item in AllItems){
                if(item.EndDate == DateTime.Now) {
                    foreach(var bid in item.Bids) {
                        Bid HighestBid = _context.Bids
                                .Include(b => b.User)
                                    .Include(b => b.AuctionItem).OrderByDescending(b => b.Amount)
                                .SingleOrDefault(b => b.ItemId == item.ItemId);
                        HighestBid.User.Wallet-=HighestBid.Amount;
                        _context.SaveChanges();
                    }
                }
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            ViewBag.AllItems = AllItems.Where(i => i.EndDate > DateTime.Now);
            ViewBag.CurrentUser = CurrentUser;
            ViewBag.CurrentDate = DateTime.Now.Date;
            return View();
        }

        [HttpGet]
        [Route("NewAuction")]
        public IActionResult NewAuction() {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View();
        }


        [HttpPost]
        [Route("AddItem")]
        public IActionResult AddItem(AuctionItem model) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = _context.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            if(model.EndDate < DateTime.Now) {
                ModelState.AddModelError("EndDate", "Must be in the future");
            }
            if(model.StartingBid <= 0) {
                ModelState.AddModelError("StartingBid", "Starting Bid must be greater than 0");
            }
            if(ModelState.IsValid) {
                model.User = CurrentUser;
                _context.Add(model);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View("NewAuction");
        }

        [HttpGet]
        [Route("Auction/{ItemId}")]
        public IActionResult Auction(int ItemId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            AuctionItem CurrentItem = _context.AuctionItems
                                        .Include(i => i.Bids)
                                            .ThenInclude(b => b.User)
                                        .SingleOrDefault(i => i.ItemId == ItemId);
            Bid HighestBid = _context.Bids
                                .Include(b => b.User)
                                    .Include(b => b.AuctionItem).OrderByDescending(b => b.Amount)
                                .SingleOrDefault(b => b.ItemId == ItemId);
            ViewBag.CurrentItem = CurrentItem;
            ViewBag.HighestBid = HighestBid;
            ViewBag.CurrentDate = DateTime.Now.Date;
            ViewBag.UserId = HttpContext.Session.GetInt32("UserId");
            return View("Auction");
        }

        [HttpPost]
        [Route("/AddBid")]
        public IActionResult AddBid(int UserId, int ItemId, int Amount) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            User CurrentUser = _context.Users.SingleOrDefault(user => user.UserId == HttpContext.Session.GetInt32("UserId"));
            AuctionItem CurrentItem = _context.AuctionItems
                                        .Include(i => i.Bids)
                                            .ThenInclude(b => b.User)
                                        .Include(i => i.User)
                                        .SingleOrDefault(i => i.ItemId == ItemId);
            if(Amount < CurrentItem.StartingBid) {
                ViewBag.error = "Cannot accept a bid lower than Starting Bid";
                Bid HighestBid = _context.Bids
                                .Include(b => b.User)
                                    .Include(b => b.AuctionItem).OrderByDescending(b => b.Amount)
                                .SingleOrDefault(b => b.ItemId == ItemId);
                ViewBag.CurrentItem = CurrentItem;
                ViewBag.CurrentDate = DateTime.Now;
                ViewBag.HighestBid = HighestBid;
                return View("Auction", ItemId);
            }
            else {
                Bid NewBid = new Bid {
                        Amount = Amount,
                        UserId = UserId,
                        User = CurrentUser,
                        ItemId = ItemId,
                        AuctionItem = CurrentItem
                };
                _context.Add(NewBid);
                _context.SaveChanges();
                return RedirectToAction("Dashboard");
            }
        }

        [HttpGet]
        [Route("Delete/{ItemId}")]
        public IActionResult Delete(int ItemId) {
            if(HttpContext.Session.GetInt32("UserId") == null) {
                return RedirectToAction("Index", "User");
            }
            AuctionItem CurrentItem = _context.AuctionItems
                                            .SingleOrDefault(i => i.ItemId == ItemId);
            _context.AuctionItems.Remove(CurrentItem);
            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

    }
}