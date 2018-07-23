﻿using System;
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
    public class TransactionsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Transactions
        [Authorize]
        public ActionResult Index()
        {
            var transactions = db.Transactions.Include(t => t.Account);
            return View(transactions.ToList());
        }

        // GET: Transactions/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // GET: Transactions/Create
        [Authorize]
        public ActionResult Create(int id)
        {
            ViewBag.BudgetID = id;
            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Type");
            ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name");
            return View();
        }

        // POST: Transactions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Type,Name,description,Cost,verify,AccountID")] Transaction transaction)
        {
            var person = User.Identity.GetUserId();

            if (ModelState.IsValid)
            {
                transaction.Created = DateTime.Now;
                transaction.UserID = person;
                db.Transactions.Add(transaction);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Type", transaction.AccountID);
            //ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", transaction.HouseholdID);
            return View(transaction);
        }

        // GET: Transactions/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Type", transaction.AccountID);
            //ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", transaction.HouseholdID);
            return View(transaction);
        }

        // POST: Transactions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Type,Name,description,Cost,verify,Created,Updated,UserID,HouseholdID,AccountID")] Transaction transaction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(transaction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.AccountID = new SelectList(db.Accounts, "ID", "Type", transaction.AccountID);
            //ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", transaction.HouseholdID);
            return View(transaction);
        }

        // GET: Transactions/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Transaction transaction = db.Transactions.Find(id);
            if (transaction == null)
            {
                return HttpNotFound();
            }
            return View(transaction);
        }

        // POST: Transactions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Transaction transaction = db.Transactions.Find(id);
            db.Transactions.Remove(transaction);
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
