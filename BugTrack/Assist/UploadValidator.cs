using BugTrack.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace BugTrack.Assist
{
    public class UploadValidator
    {
        public static ApplicationDbContext db = new ApplicationDbContext();

        public static bool WebFriendly(HttpPostedFileBase file)
        {
            if (file == null) return false;

            try
            {
                var allowedExtensions = new List<string>
                {
                    "txt",
                    "doc",
                    "docx",
                    "pdf",
                    "xls",
                    "jpg",
                    "png"
                };

                var fileExtension = Path.GetExtension(file.FileName).Split('.')[1];
                return allowedExtensions.Contains(fileExtension);
            }
            catch
            {
                return false;
            }
        }
    }
}