﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using BugTrack.ActionFilter;
using BugTrack.Assist;
using BugTrack.Extension_Methods;
using BugTrack.Models;
using Microsoft.AspNet.Identity;

namespace BugTrack.Controllers
{
    public class TicketsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ProjectsHelper projhelp = new ProjectsHelper();
        public UserRolesHelper helprole = new UserRolesHelper();
        
        
        // GET: Tickets
        [Authorize]
        public ActionResult Index()
        {
            var userID = User.Identity.GetUserId();
            Mytickets Tickethelp = new Mytickets();
            var userTickets = Tickethelp.ListOfUserTickets(userID);
            
            return View(userTickets);
        }

        // GET: Tickets/Details/5
        [TicketAuthorization]
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
            var person = User.Identity.GetUserId();
            var projHelp = new ProjectsHelper();
            var projects = projHelp.ListUserProjects(person);

            ViewBag.OwnerUserID = User.Identity.GetUserId();
            ViewBag.ProjectID = new SelectList(projects, "ID", "Name");
            ViewBag.TicketPriorityID = new SelectList(db.TicketPriorities, "ID", "Name");
            
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "ID", "Name");
            return View();
        }

        // POST: Tickets/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Title,Updated,ProjectID,TicketTypeID,TicketPriorityID,TicketStatusID,OwnerUserID,AssignedToUserID")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                ticket.TicketStatusID = db.TicketStatus.FirstOrDefault(t => t.Name == "UnAssigned").ID;
                ticket.OwnerUserID = User.Identity.GetUserId();
                ticket.Created = DateTimeOffset.Now;
                db.Tickets.Add(ticket);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            
            
            
            ViewBag.ProjectID = new SelectList(db.Projects, "ID", "Name", ticket.ProjectID);
            ViewBag.TicketPriorityID = new SelectList(db.TicketPriorities, "ID", "Name", ticket.TicketPriorityID);
            
            ViewBag.TicketTypeID = new SelectList(db.TicketTypes, "ID", "Name", ticket.TicketTypeID);
            return View(ticket);
        }

        // GET: Tickets/Edit/5
        [TicketAuthorization]
        public ActionResult Edit(int? id)
        {
            
           var developers = new List<ApplicationUser>();

            //Get the project for this Ticket
            foreach(var user in projhelp.UsersOnProject(db.Tickets.Find(id).ProjectID).ToList())
            {
                if (helprole.IsUserInRole(user.Id, "Developer"))
                    developers.Add(user);
            }
            
            //Get all Developers on the Project


            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ticket ticket = db.Tickets.Find(id);
            var project = ticket.ProjectID;
           
            if (ticket == null)
            {
                return HttpNotFound();
            }
            ViewBag.AssignedToUserID = new SelectList(developers, "ID", "FirstName", ticket.AssignedToUserID);
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
        public async Task<ActionResult> Edit([Bind(Include = "ID,Title,Created,Updated,ProjectID,TicketTypeID,TicketPriorityID,TicketStatusID,OwnerUserID,AssignedToUserID")] Ticket ticket)
        {
            var oldTicket = db.Tickets.AsNoTracking().FirstOrDefault(t => t.ID == ticket.ID);

            if (ModelState.IsValid)
            {
                ticket.Updated = DateTimeOffset.Now;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                ticket.RecordChanges(oldTicket);
                await ticket.TriggerNotifications(oldTicket);
                return RedirectToAction("Index");
            }
            
            ViewBag.AssignedToUserID = new SelectList(db.Users, "ID", "FirstName", ticket.AssignedToUserID);
            ViewBag.OwnerUserID = new SelectList(db.Users, "ID", "FirstName", ticket.OwnerUserID);
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
