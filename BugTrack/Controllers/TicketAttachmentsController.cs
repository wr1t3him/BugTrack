using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using BugTrack.Models;
using Microsoft.AspNet.Identity;
using BugTrack.Assist;

namespace BugTrack.Controllers
{
    public class TicketAttachmentsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TicketAttachments
        [Authorize]
        public ActionResult Index()
        {
           return View((db.TicketAttachments.ToList()));
        }

        public ActionResult Specific (int ticketID)
        {
            ViewBag.Header = "Ticket Attachments";
            var ticketAttachments = db.TicketAttachments.Where(t => t.TicketID == ticketID).ToList();
            return View("Index", ticketAttachments);
        }

        // GET: TicketAttachments/Details/5
        [Authorize]
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Create
        [Authorize]
        public ActionResult Create()
        {
            var userID = User.Identity.GetUserId();
            Mytickets Tickethelp = new Mytickets();
            var userTickets = Tickethelp.ListOfUserTickets(userID);

            ViewBag.TicketID = new SelectList(userTickets, "ID", "Title");
            return View();
        }

        // POST: TicketAttachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "TicketID")] TicketAttachment ticketAttachment, HttpPostedFileBase file)
        {
            if (ModelState.IsValid)
            {
                ticketAttachment.MediaUrl = "/Uploads/default.png";
                if(file != null)
                {
                    var filename = Path.GetFileName(file.FileName);
                    file.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), filename));
                    ticketAttachment.MediaUrl = "/Uploads" + filename;
                }

                ticketAttachment.Created = DateTimeOffset.Now;
                ticketAttachment.UserID = User.Identity.GetUserId();

                db.TicketAttachments.Add(ticketAttachment);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TicketID = new SelectList(db.Tickets, "ID", "Title", ticketAttachment.TicketID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserID);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            ViewBag.TicketID = new SelectList(db.Tickets, "ID", "Title", ticketAttachment.TicketID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserID);
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,TicketID,FilePath,Created,UserID")] TicketAttachment ticketAttachment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticketAttachment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.TicketID = new SelectList(db.Tickets, "ID", "Title", ticketAttachment.TicketID);
            ViewBag.UserID = new SelectList(db.Users, "Id", "FirstName", ticketAttachment.UserID);
            return View(ticketAttachment);
        }

        // GET: TicketAttachments/Delete/5
        [Authorize]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            if (ticketAttachment == null)
            {
                return HttpNotFound();
            }
            return View(ticketAttachment);
        }

        // POST: TicketAttachments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TicketAttachment ticketAttachment = db.TicketAttachments.Find(id);
            db.TicketAttachments.Remove(ticketAttachment);
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
