using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinancePlan.Models;
using Microsoft.AspNet.Identity;

namespace FinancePlan.Controllers
{
    public class AccountsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Accounts
        [Authorize]
        public ActionResult Index()
        {
            var accounts = db.Accounts.Include(a => a.Bank).Include(a => a.Household);
            return View(accounts.ToList());
        }

        // GET: Accounts/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // GET: Accounts/Create
        [Authorize]
        public ActionResult Create()
        {
            var person = User.Identity.GetUserId();
            var customuser = db.Users.Find(person).FirstName;
            //var house = db.Users.Find(person).HouseholdID;
            //var myhouse = db.Households.Find(house);
            //var banklist = db.Banks.Find(person).ID;

            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name");
            ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name");
            ViewBag.UserID = customuser;
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Balance,Type,HouseholdID,BankID")] Account account)
        {
            var person = User.Identity.GetUserId();
            var personal = db.Users.Find(person).FirstName;

            if (ModelState.IsValid)
            {
                account.UserID = person;
                db.Accounts.Add(account);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name", account.BankID);
            ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", account.HouseholdID);
            return View(account);
        }

        // GET: Accounts/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name", account.BankID);
            ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", account.HouseholdID);
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Balance,Type,UserID,HouseholdID,BankID")] Account account)
        {
            if (ModelState.IsValid)
            {
                db.Entry(account).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BankID = new SelectList(db.Banks, "ID", "Name", account.BankID);
            ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", account.HouseholdID);
            return View(account);
        }

        // GET: Accounts/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Account account = db.Accounts.Find(id);
            if (account == null)
            {
                return HttpNotFound();
            }
            return View(account);
        }

        // POST: Accounts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Account account = db.Accounts.Find(id);
            db.Accounts.Remove(account);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
