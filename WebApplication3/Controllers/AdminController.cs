using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VSZANAL.Models;

namespace VSZANAL.Controllers
{
    public class AdminController : Controller
    {
        private RUNContext db;
        private IHostingEnvironment env;
        public AdminController(RUNContext context, IHostingEnvironment appEnvironment)
        {
            db = context;
            env = appEnvironment;
        }

        public async Task<IActionResult> Index()
        {
            IQueryable<User> users = db.Users;
            return View(await users.AsNoTracking().ToListAsync());
        }

        public async Task<IActionResult> Files(int? Id, SortState sortOrder = SortState.NameAsc)
        {

            if (Id == null)
            {
                User user = HomeController.GetUser(db, HttpContext);
                ViewBag.login = user.Id;
                ViewBag.Owner = true;
            }
            else
            {
                ViewBag.login = Id;
                ViewBag.Owner = false;
            }

            IQueryable<UserFile> users = db.Files.Include(u => u.User);

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["TimeSort"] = sortOrder == SortState.TimeAsc ? SortState.TimeDesc : SortState.TimeAsc;

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case SortState.TimeAsc:
                    users = users.OrderBy(s => s.Time);
                    break;
                case SortState.TimeDesc:
                    users = users.OrderByDescending(s => s.Time);
                    break;
                default:
                    users = users.OrderBy(s => s.Name);
                    break;
            }
            return View(await users.AsNoTracking().ToListAsync());
        }
    }
}
