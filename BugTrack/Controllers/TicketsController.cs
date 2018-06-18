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
using Microsoft.AspNet.Identity;

namespace BugTrack.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ProjectsHelper projhelp = new ProjectsHelper();
        
        
        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            var userTickets = new List<Ticket>();

            if (User.IsInRole("Admin"))
            {
               userTickets = db.Tickets.ToList();                
            }
            else if(User.IsInRole("Submitter"))
            {
                userTickets = db.Tickets.Where(t => t.OwnerUserID == userID).ToList();                
            }
            else if(User.IsInRole("Developer"))
            {
                userTickets = db.Tickets.Where(t => t.AssignedToUserID == userID).ToList();                
            }
            else
            {                
                var myprojects = projhelp.ListUserProjects(userID);
                foreach(var project in myprojects)
                {
                    var projId = project.ID;
                    userTickets.AddRange(db.Tickets.Where(t => t.ProjectID == projId).ToList());
                }
                
                               
            }
            //var tickets = db.Tickets.Include(t => t.AssignedToUser).Include(t => t.OwnerUser).Include(t => t.Project).Include(t => t.TicketPriority).Include(t => t.TicketStatus).Include(t => t.TicketType);
            return View(userTickets);
        }

        // GET: Tickets/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // GET: Tickets/Create
        [Authorize(Roles = "Submitter")]
        public ActionResult Create()
        {
            var userID = User.Identity.GetUserId();

            ViewBag.OwnerUserID = userID;
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name");
            ViewBag.TicketPriorityID = new SelectList(db.TicketPriorities, "ID", "Name");
            ViewBag.TicketStatusID = new SelectList(db.TicketStatus, "ID", "Name");
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "ID", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Created,Updated,ProjectID,TicketTypeID,TicketPriorityID,TicketStatusID,OwnerUserID,AssignedToUserID")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            var userID = User.Identity.GetUserId();
            ticket.Created = DateTimeOffset.Now;
            ViewBag.OwnerUserID = userID;
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityID = new SelectList(db.TicketPriorities, "ID", "Name", ticket.TicketPriorityID);
            ViewBag.TicketStatusID = new SelectList(db.TicketStatus, "ID", "Name", ticket.TicketStatusID);
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "ID", "Name", ticket.TicketTypeID);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserID = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserID);
            ViewBag.OwnerUserID = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityID = new SelectList(db.TicketPriorities, "ID", "Name", ticket.TicketPriorityID);
            ViewBag.TicketStatusID = new SelectList(db.TicketStatus, "ID", "Name", ticket.TicketStatusID);
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "ID", "Name", ticket.TicketTypeID);
            return View(ticket);
        }

        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Title,Created,Updated,ProjectID,TicketTypeID,TicketPriorityID,TicketStatusID,OwnerUserID,AssignedToUserID")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ticket.Updated = DateTimeOffset.Now;
            ViewBag.AssignedToUserID = new SelectList(db.Users, "Id", "FirstName", ticket.AssignedToUserID);
            ViewBag.OwnerUserID = new SelectList(db.Users, "Id", "FirstName", ticket.OwnerUserID);
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityID = new SelectList(db.TicketPriorities, "ID", "Name", ticket.TicketPriorityID);
            ViewBag.TicketStatusID = new SelectList(db.TicketStatus, "ID", "Name", ticket.TicketStatusID);
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "ID", "Name", ticket.TicketTypeID);
            return View(ticket);
        }

        // GET: Tickets/Delete/5
        [Authorize(Roles = "Admin, ProjectManager")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            if (ticket == null)
            {
                return HttpNotFound();
            }
            return View(ticket);
        }

        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ticket ticket = db.Tickets.Find(id);
            db.Tickets.Remove(ticket);
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
