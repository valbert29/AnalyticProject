using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;
using VSZANAL.Models;

namespace VSZANAL.Controllers
{
    public class HomeController : Controller
    {
        RUNContext db;
        IHostingEnvironment _appEnvironment;

        [Authorize]
        public IActionResult Index()
        {
            User user = GetUser(db, HttpContext);
            if (user != null)
            {
                ViewBag.login = user.Name;
                ViewBag.Avatar = user.Avatar;
            }            
            return View();
        }
        
        public HomeController(RUNContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddFile(IFormFileCollection uploads)
        {
            var login = HttpContext.User.Identity.Name;
            foreach (var uploadedFile in uploads)
            {
                var valid = uploadedFile.FileName.Split(".");
                if (valid[valid.Length - 1] != "txt")
                {
                    break;
                }
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                User user = GetUser(db, HttpContext);
                UserFile file = new UserFile { Name = uploadedFile.FileName, Path = path, Time = DateTime.Now, UserId = user.Id, Previous = -1 };
                db.Files.Add(file);
            }
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddAvatar(IFormFileCollection uploads)
        {
            foreach (var uploadedFile in uploads)
            {
                var valid = uploadedFile.FileName.Split(".");//JPG, GIF или PNG.
                var last = valid[valid.Length - 1].ToLower();
                if (last == "jpg"|| last == "gif" || last == "png" || last == "jpeg")
                {
                    // путь к папке Files
                    string path = "/Avatar/" + uploadedFile.FileName;
                    // сохраняем файл в папку Files в каталоге wwwroot
                    using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                    {
                        await uploadedFile.CopyToAsync(fileStream);
                    }

                    using (var db = this.db)
                    {
                        var result = GetUser(db, HttpContext);
                        if (result != null)
                        {
                            result.Avatar = path;
                            db.SaveChanges();
                        }
                    }
                }
                
            }
            return RedirectToAction("ProfilePage");
        }
        
        [Authorize]
        public IActionResult ProfilePage()
        {
            User user = GetUser(db, HttpContext);
            Role role = db.Roles.FirstOrDefault(u => u.Id == user.RoleId);
            ViewBag.Avatar =  user.Avatar;
            ViewData["login"] = user.Name;
            ViewBag.Role = role.Name;
            return View();
        }

        [Authorize]
        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        [Authorize]
        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public static User GetUser(RUNContext db, HttpContext httpContext )
        {
            var login = httpContext.User.Identity.Name;
            return db.Users.FirstOrDefault(u => u.Login == login);
        }
    }
}
