using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using BugTrack.Assist;
using FinancePlan.Models;
using FinancePlan.ViewModels;
using Microsoft.AspNet.Identity;

namespace FinancePlan.Controllers
{
    public class HouseholdsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper rolehelper = new UserRolesHelper();

        // GET: Households
        [Authorize]
        public ActionResult Index()
        {
            return View(db.Households.Where(i => i.deleted != true).ToList());
        }

        // GET: Households/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Household household = db.Households.Find(id);
            
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        [Authorize]
        public ActionResult HouseMembers (int? id)
        {

             return View(db.Users.Where(u => u.HouseholdID == id).ToList());
        }


        // GET: Households/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Households/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] Household household)
        {
            if (ModelState.IsValid)
            {
                var userID = User.Identity.GetUserId();
                rolehelper.AddUserToRole(userID, "Owner");

                db.Households.Add(household);
                db.SaveChanges();

                var user = db.Users.Find(userID);
                user.HouseholdID = household.ID;
                db.SaveChanges();

                return RedirectToAction("Index");
            }

            return View(household);
        }

        // GET: Households/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name, deleted")] Household household)
        {
            if (ModelState.IsValid)
            {
                db.Entry(household).State = EntityState.Modified;            
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(household);
        }

        public ActionResult HouseDashboard()
        {
            return View();
        }
        
        [Authorize]
        public ActionResult LeaveHouse (int id)
        {
            ViewBag.HouseholdID = id;
            return View();
        }

        [HttpPost]
        public ActionResult LeaveHouse(bool Leave, int householdID)
        {
            if(User.Identity.IsAuthenticated && Leave)
            {
                var user = db.Users.Find(User.Identity.GetUserId());
                user.HouseholdID = null;
                user.Household.Name = null;
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View();
        }

        // GET: Households/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Household household = db.Households.Find(id);
            if (household == null)
            {
                return HttpNotFound();
            }
            return View(household);
        }

        // POST: Households/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Household household = db.Households.Find(id);
            db.Households.Remove(household);
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
