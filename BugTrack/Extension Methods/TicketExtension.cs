﻿using BugTrack.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Configuration;

namespace BugTrack.Extension_Methods
{
    public static class TicketExtension
    {
        private static ApplicationDbContext db = new ApplicationDbContext();

        public static void RecordChanges(this Ticket ticket, Ticket oldTicket)
        {
            var propertyList = WebConfigurationManager.AppSettings["TrackedTicketProperties"].Split(',');
            foreach (PropertyInfo prop in ticket.GetType().GetProperties())
            {
                if (!propertyList.Contains(prop.Name))
                {
                    continue;
                }

                var value = prop.GetValue(ticket, null) ?? "";
                var oldValue = prop.GetValue(oldTicket, null) ?? "";

                if (value.ToString() != oldValue.ToString())
                {
                    var ticketHistory = new TicketHistory
                    {
                        Changed = DateTime.Now,
                        Property = prop.Name,
                        NewValue = GetValueFromKey(prop.Name, value),
                        OldValue = GetValueFromKey(prop.Name, oldValue),
                        TicketID = ticket.ID,
                        UserID = HttpContext.Current.User.Identity.GetUserId()
                    };

                    db.TicketHistories.Add(ticketHistory);
                }
            }

            db.SaveChanges();
        }

        private static string GetValueFromKey(string keyName, object key)
        {
            var returnValue = "";
            if (key.ToString() == string.Empty)
            {
                return returnValue;
            }

            switch (keyName)
            {
                case "ProjectId":
                    returnValue = db.Projects.Find(key).Name;
                    break;
                case "TicketTypeId":
                    returnValue = db.TicketTypes.Find(key).Name;
                    break;
                case "TicketPriorityId":
                    returnValue = db.TicketPriorities.Find(key).Name;
                    break;
                case "TicketStatusId":
                    returnValue = db.TicketStatus.Find(key).Name;
                    break;
                case "OwnerUserId":
                case "AssignedToUserId":
                    returnValue = db.Users.Find(key).DisplayName;
                    break;
                default:
                    returnValue = key.ToString();
                    break;
            }
            return returnValue;
        }
        
    }
}