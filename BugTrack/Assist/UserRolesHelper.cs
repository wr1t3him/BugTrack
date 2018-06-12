using BugTrack.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BugTrack.Assist
{
    public class UserRolesHelper
    {
        private UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext()));
        private ApplicationDbContext db = new ApplicationDbContext();

        //Function to check if User has a role
        public bool IsUserInRole(string UserID , string roleName)
        {
            return userManager.IsInRole(UserID, roleName);
        }

        //returns a list of users that have a role
        public ICollection<string> ListUserRoles (string UserID)
        {
            return userManager.GetRoles(UserID);
        }

        //places a role on a user according to the ID
        public bool AddUserToRole (string UserID , string roleName)
        {
            var result = userManager.AddToRole(UserID, roleName);
            return result.Succeeded;
        }

        //Takes a user off a role
        public bool RemoveUserFromRole (string UserID, string roleName)
        {
            var result = userManager.RemoveFromRole(UserID, roleName);
            return result.Succeeded;
        }

        //makes a list of users that are in a role
        public ICollection<ApplicationUser> UsersInRole (string roleName)
        {
            var resultList = new List<ApplicationUser>();
            var List = userManager.Users.ToList();
            foreach(var user in List)
            {
                if(!IsUserInRole(user.Id, roleName))
                {
                    resultList.Add(user);
                }

            }

            return resultList;
        }

    }
}