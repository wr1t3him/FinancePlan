using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTrack.Assist;
using FinancePlan.Models;
using Microsoft.AspNet.Identity;
using FinancePlan.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FinancePlan.Controllers
{
    public class InvitationsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UserRolesHelper roleHelper = new UserRolesHelper();

        // GET: Invitations
        [Authorize]
        public ActionResult Index()
        {
            var invitations = db.Invitations.Include(i => i.Household);
            return View(invitations.ToList());
        }

        // GET: Invitations/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // GET: Invitations/Create
        [Authorize]
        public ActionResult Create(int houseID)
        {
            ViewBag.HouseholdID = houseID;
            return View();
        }

        // POST: Invitations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Created,Email,Body,Lifespan,HouseholdID")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                invitation.Created = DateTime.Now;
                invitation.code = Guid.NewGuid().ToString();

                db.Invitations.Add(invitation);
                db.SaveChanges();

                var callbackurl = Url.Action("Join", "Invitations", new { email = invitation.Email, houseCode = invitation.code }, protocol: Request.Url.Scheme);
                var houseName = db.Households.Find(invitation.HouseholdID).Name;
                var email = new IdentityMessage()
                {
                    Subject = string.Format("Invitation to the " + houseName + " {0}", houseName),
                    Body = invitation.Body + invitation.code + "<br /> Please accept this invitation by clicking <a href=\"" + callbackurl + "\">here</a>",
                    Destination = invitation.Email
                };

                var svc = new EmailService();
                await svc.SendAsync(email);

                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.HouseholdID = invitation.HouseholdID;
                var message = string.Join(" | ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));

                ModelState.AddModelError("", message);
                return View(invitation);
            }
        }

        [Authorize]
        public ActionResult Join(string email, string houseCode)
        {
            var acceptInviteVM = new AcceptInviteViewModel
            {
                Email = email,
                Code = houseCode,
                HouseholdID = db.Invitations.FirstOrDefault(i => i.code == houseCode).HouseholdID
            };
            return View(acceptInviteVM);
        }

        [HttpPost]
        public ActionResult Join(AcceptInviteViewModel acceptInviteVM)
        {
            var invite = db.Invitations.FirstOrDefault(i => i.Email == acceptInviteVM.Email && i.code == acceptInviteVM.Code);
            //var userId = User.Identity.GetUserId();
            var userManager = new UserManager<ApplicationUser>(
            new UserStore<ApplicationUser>(db));
            var userID = userManager.FindByEmail(acceptInviteVM.Email).Id;

            if (userID == null && invite != null)
            {
                userManager.Create(new ApplicationUser
                {
                    UserName = acceptInviteVM.Email,
                    Email = acceptInviteVM.Email,
                    HouseholdID = acceptInviteVM.HouseholdID
                }, "Abc&123");


                userManager.AddToRole(userID, "Adult");
                db.SaveChanges();

                TempData["sweetMsg"] = "Thank you for accepting my invitation, you are now a Household Member!";
            }
            else if (invite != null)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.Find(userId);
                user.HouseholdID = invite.HouseholdID;
                roleHelper.AddUserToRole(userId, "Adult");

                db.SaveChanges();

                TempData["sweetMsg"] = "Thank you for accepting my invitation, you are now a Household Member!";
            }
            return RedirectToAction("Index", "Home");
        
        }

        // GET: Invitations/Edit/5
        [Authorize]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", invitation.HouseholdID);
            return View(invitation);
        }

        // POST: Invitations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Created,Email,Body,Lifespan,HouseholdID")] Invitation invitation)
        {
            if (ModelState.IsValid)
            {
                db.Entry(invitation).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.HouseholdID = new SelectList(db.Households, "ID", "Name", invitation.HouseholdID);
            return View(invitation);
        }

        // GET: Invitations/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Invitation invitation = db.Invitations.Find(id);
            if (invitation == null)
            {
                return HttpNotFound();
            }
            return View(invitation);
        }

        // POST: Invitations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Invitation invitation = db.Invitations.Find(id);
            db.Invitations.Remove(invitation);
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
