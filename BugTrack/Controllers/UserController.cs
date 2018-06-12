using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrack.Assist;
using BugTrack.Models;

namespace BugTrack.Controllers
{
    public class UserController : Controller
    {
        private UserRolesHelper roleHelp = new UserRolesHelper();
        private ProjectsHelper proj = new ProjectsHelper();
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Index()
        {
            return View(db.Users.ToList());
        }

        // GET: User/Details/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // GET: User/Create
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Create()
        {
            return View();
        }

        // POST: User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,FirstName,LastName,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: User/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }

            var currentRole = roleHelp.ListUserRoles(id).FirstOrDefault();
            ViewBag.Roles = new SelectList(db.Roles, "Name", "Name", currentRole);

            var projIDs = new List<int>();
            var projs = proj.ListUserProjects(id);
            foreach(var project in projs)
            {
                projIDs.Add(project.ID);
            }
            ViewBag.Projects = new MultiSelectList(db.Projects, "Id", "Name", projIDs);

            return View(applicationUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,FirstName,LastName,Email,PhoneNumber")] ApplicationUser applicationUser, string Roles, List<int> Projects)
        {
            if (ModelState.IsValid)
            {
                applicationUser.UserName = applicationUser.Email;
                foreach(var role in roleHelp.ListUserRoles(applicationUser.Id))
                {
                    roleHelp.RemoveUserFromRole(applicationUser.Id, role);
                }

                if(Roles != null)
                {
                    roleHelp.AddUserToRole(applicationUser.Id, Roles);
                }


                foreach(var project in proj.ListUserProjects(applicationUser.Id))
                {
                    proj.RemoveUserFromProject(applicationUser.Id, project.ID);
                }

                if(Projects != null)
                {
                    foreach(var projectID in Projects)
                    {
                        proj.AddUserToProject(applicationUser.Id, projectID);
                    }
                }

                db.Users.Attach(applicationUser);
                db.Entry(applicationUser).Property(a => a.FirstName).IsModified = true;
                db.Entry(applicationUser).Property(a => a.LastName).IsModified = true;
                db.Entry(applicationUser).Property(a => a.PhoneNumber).IsModified = true;
                db.Entry(applicationUser).Property(a => a.Email).IsModified = true;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: User/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
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
