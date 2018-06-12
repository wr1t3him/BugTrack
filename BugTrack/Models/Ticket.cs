using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrack.Models
{
    public class Ticket
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset Updated { get; set; }
        public int ProjectID { get; set; }
        public int TicketTypeID { get; set; }
        public int TicketPriorityID { get; set; }
        public int TicketStatusID { get; set; }
        public string OwnerUserID { get; set; }
        public string AssignedToUserID { get; set; }

        public virtual ICollection<TicketAttachment> TicketAttachments { get; set; }
        public virtual ICollection<TicketComment> TicketComments { get; set; }
        public virtual ICollection<TicketHistory> TicketHistories { get; set; }
        public virtual ICollection<TicketNotification> TicketNotifications { get; set; }

        public Ticket()
        {
            this.TicketAttachments = new HashSet<TicketAttachment>();
            this.TicketComments = new HashSet<TicketComment>();
            this.TicketHistories = new HashSet<TicketHistory>();
            this.TicketNotifications = new HashSet<TicketNotification>();

        }

        public virtual TicketStatus TicketStatus { get; set; }
        public virtual TicketPriority TicketPriority { get; set; }
        public virtual TicketType TicketType { get; set; }
        public virtual Project Project { get; set; }
        public virtual ApplicationUser OwnerUser { get; set; }
        public virtual ApplicationUser AssignedToUser { get; set; }

    }
}