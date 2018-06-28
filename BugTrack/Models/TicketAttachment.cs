using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrack.Models
{
    public class TicketAttachment
    {
        public int ID { get; set; }
        public int TicketID { get; set; }
        public string FilePath { get; set; }
        public DateTimeOffset Created { get; set; }
        public string UserID { get; set; }
        public string MediaUrl { get; set; }
        public string Description { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}