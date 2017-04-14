using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;

namespace Vidly.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            using (OurDbContext db = new OurDbContext())
            {
                return View(db.UserAccount.ToList());
            }
        }

        // GET: Account/ReservationList
        public ActionResult ReservationList()
        {
            using (OurDbContext db = new OurDbContext())
            {
                return View(db.Requests.ToList());
            }
        }

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost] // POST: account registration to DB list
        public ActionResult Register(UserAccount account)
        {
            if(ModelState.IsValid)
            {
                using (OurDbContext db = new OurDbContext())
                {
                    db.UserAccount.Add(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " Successfully registered";
            }
            return View();
        }

        // GET: Account/Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(UserAccount account)
        {
            if (ModelState.IsValid)
            {
                using (OurDbContext db = new OurDbContext())
                {
                    db.UserAccount.Remove(account);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = account.FirstName + " " + account.LastName + " Successfully deleted";
            }
            return View();
        }

        [HttpPost] // POST: Logs user in using DB List Session info
        public ActionResult Login(UserAccount user)
        {
            using (OurDbContext db = new OurDbContext())
            {
                var usr = db.UserAccount.Where(u => u.Username == user.Username && u.Password == user.Password).FirstOrDefault();
                if (usr != null)
                {
                    Session["UserID"] = usr.UserID.ToString();
                    Session["Username"] = usr.Username.ToString();
                    return RedirectToAction("LoggedIn");
                }
                else
                {
                    ModelState.AddModelError("", "Username or Password is wrong.");
                }
            }
            return View();
        }

        // If userid is not null and correct return LoggedIn views Account/LoggedIn
        // Else return user to Account/Login
        public ActionResult LoggedIn()
        {
            if(Session["UserID"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        public ActionResult Reservation()
        {
            return View();
        }

        //Allows a user to create a reservation -- Passes a request argument which stores the input values--
        //-- into a Method 
        [HttpPost]
        public ActionResult Reservation(Request request)
        {
            if(ModelState.IsValid)
            {
                using (OurDbContext db = new OurDbContext())
                {
                    db.Requests.Add(request);
                    db.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = "Reservation for " + request.FirstName + " for a party of " + request.Size + " on" + request.Date;
            }
            return View();
        }
        public ActionResult Reserved()
        {
            if (Session["Id"] != null)
            {
                return RedirectToAction("ReservationList");
            }
            else
            {
                return View();
            }
        }

        // GET: Account/Reservation

    }

}