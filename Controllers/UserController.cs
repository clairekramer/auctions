using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using auctions.Models;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace auctions.Controllers
{
    public class UserController : Controller
    {
        private AuctionContext _context;
        public UserController(AuctionContext context) {
            _context = context;
        }
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult Register(User model) {
            User CheckUsername = _context.Users.SingleOrDefault(user => user.Username == model.Username);
            if(CheckUsername != null) {
                ViewBag.errors = "Username already registered to an account";
                return View("Index");
            }
            if(ModelState.IsValid) {
                _context.Add(model);
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                model.Wallet = 1000;
                model.Password = Hasher.HashPassword(model, model.Password);
                model.ConfirmPW = Hasher.HashPassword(model, model.ConfirmPW);
                _context.SaveChanges();
                ViewBag.errors = "Successfully Registered! You may now login!";
                return View("Index");
            }
            else {
                return View("Index");
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string Username, string Password) {
            User CheckUsername = _context.Users.SingleOrDefault(user => user.Username == Username);
            if(CheckUsername != null) {
                var Hasher = new PasswordHasher<User>();
                if(0 != Hasher.VerifyHashedPassword(CheckUsername, CheckUsername.Password, Password)) {
                    HttpContext.Session.SetInt32("UserId", CheckUsername.UserId);
                    return RedirectToAction("Dashboard", "Auction");
                }
                else {
                    ViewBag.errors = "Incorrect Password";
                    return View("Index");
                }
            }
            else {
                ViewBag.errors = "Invalid Username";
                return View("Index");
            }
        }

        [HttpGet]
        [Route("Logout")]
        public IActionResult Logout() {
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }
    }
}