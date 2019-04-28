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
        RUNContext _context;
        IHostingEnvironment _appEnvironment;

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var login = HttpContext.User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            ViewBag.login = user.Name;
            ViewBag.Avatar = user.Avatar;
            return View();
        }
        
        public HomeController(RUNContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        [HttpPost]
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
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                UserFile file = new UserFile { Name = uploadedFile.FileName, Path = path, Time = DateTime.Now, UserId = user.Id, Previous = -1 };
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

                    using (var db = _context)
                    {
                        var login = HttpContext.User.Identity.Name;
                        var result = db.Users.SingleOrDefault(b => b.Login == login);
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
        public async Task<IActionResult> ProfilePage()
        {
            var login = HttpContext.User.Identity.Name;

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            ViewBag.Avatar =  user.Avatar;
            ViewData["login"] = user.Name;
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
