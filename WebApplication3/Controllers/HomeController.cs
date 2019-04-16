using System.Web;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using System;

namespace VSZANAL.Controllers
{
    public class HomeController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            return View();
        }

        RUNContext _context;
        IHostingEnvironment _appEnvironment;
        public HomeController(RUNContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFileCollection uploads)
        {
            var login = HttpContext.Response.HttpContext.User.Identity.Name;
            foreach (var uploadedFile in uploads)
            {
                // путь к папке Files
                string path = "/Files/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                UserFile file = new UserFile { Name = uploadedFile.FileName, Path = path, Time = DateTime.Now, UserId = user.Id };
                _context.Files.Add(file);
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> AddAvatar(IFormFileCollection uploads)
        {
            foreach (var uploadedFile in uploads)
            {
                // путь к папке Files
                string path = "/Avatar/" + uploadedFile.FileName;
                // сохраняем файл в папку Files в каталоге wwwroot
                using (var fileStream = new FileStream(_appEnvironment.WebRootPath + path, FileMode.Create))
                {
                    await uploadedFile.CopyToAsync(fileStream);
                }
                var login = HttpContext.Response.HttpContext.User.Identity.Name;
                ViewBag.login = login;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Name == login);
                // var newUs = new User { Id = user.Id, Avatar = path, Files = user.Files, Login = user.Login, Name = user.Name, Password = user.Password, RoleId = user.RoleId };
                //UserFile file = new UserFile { Name = uploadedFile.FileName, Path = path, Time = DateTime.Now, UserId = user.Id };
                //_context.Users.Remove(user);
                using (var db = _context)
                {
                    var result = db.Users.SingleOrDefault(b => b.Name == login);
                    if (result != null)
                    {
                        result.Avatar = path;
                        db.SaveChanges();
                    }
                }
            }

            return RedirectToAction("ProfilePage");
        }



        public IActionResult ProfilePage()
        {
            var login = HttpContext.Response.HttpContext.User.Identity.Name;
            ViewData["login"] = login;
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
