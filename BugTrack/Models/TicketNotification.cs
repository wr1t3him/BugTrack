using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrack.Models
{
    public class TicketNotification
    {
        public int ID { get; set; }
        public int TicketID { get; set; }
        public string UserID { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}