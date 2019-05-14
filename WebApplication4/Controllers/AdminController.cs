using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Index(SortState sortOrder = SortState.TimeAsc)
        {
            IQueryable<User> users = db.Users;
            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["TimeSort"] = sortOrder == SortState.TimeAsc ? SortState.TimeDesc : SortState.TimeAsc;

            switch (sortOrder)
            {
                case SortState.NameDesc:
                    users = users.OrderByDescending(s => s.Name);
                    break;
                case SortState.TimeAsc:
                    users = users.OrderBy(s => s.Id);
                    break;
                case SortState.TimeDesc:
                    users = users.OrderByDescending(s => s.Id);
                    break;
                default:
                    users = users.OrderBy(s => s.Name);
                    break;
            }
            return View(await users.AsNoTracking().ToListAsync());
            return View(await users.AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Files(int? Id, SortState sortOrder = SortState.NameAsc)
        {

            if (Id == null)
            {
                User user = HomeController.GetUser(db, HttpContext);
                ViewBag.login = user.Id;
            }
            else
            {
                ViewBag.login = Id;
            }
            ViewBag.Admin = true;

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

        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Subscriptions(int? Id, SortState sortOrder = SortState.NameAsc)
        {
            User user = HomeController.GetUser(db, HttpContext);
            ViewData["userId"] = Id;
            //if (Id == null)
            //{
            //    ViewBag.login = user.Id;
            //}
            //else
            //{
            ViewBag.login = Id;
            //}
            ViewBag.Admin = true;

            IQueryable<Subscription> users = db.Subscriptions.Include(u => u.User);

            ViewData["NameSort"] = sortOrder == SortState.NameAsc ? SortState.NameDesc : SortState.NameAsc;
            ViewData["TimeSort"] = sortOrder == SortState.TimeAsc ? SortState.TimeDesc : SortState.TimeAsc;
            ViewData["IdSort"] = sortOrder == SortState.IdAsc ? SortState.IdDesc : SortState.IdAsc;
           
            switch (sortOrder)
            {
                case SortState.NameDesc:
                    users = users.OrderByDescending(s => s.Id);
                    break;
                case SortState.TimeAsc:
                    users = users.OrderBy(s => s.Period);
                    break;
                case SortState.TimeDesc:
                    users = users.OrderByDescending(s => s.Period);
                 break;
                //case SortState.IdAsc:
                //    users = users.OrderBy(s => s.Id);
                //    break;
                //case SortState.IdDesc:
                //    users = users.OrderByDescending(s => s.Id);
                //    break;
                default:
                    users = users.OrderBy(s => s.Id);
                    break;
            }
            return View(await users.AsNoTracking().ToListAsync());
        }

        [Authorize(Roles = "admin")]
        public IActionResult CreateSub(int Id)
        {
            ViewData["Id"] = Id;
            ViewData["UserId"] = new SelectList(db.Users, "Id", "Id");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="admin")]
        public async Task<IActionResult> CreateSub([Bind("Name,Period,UserId")] Subscription userSub)
        {
            ViewData["Id"] = userSub.UserId;
            var sub = db.Subscriptions.FirstOrDefault(s => s.Name == userSub.Name && s.UserId == userSub.UserId);
            if (sub != null)
            {
                return View(userSub);
            }
            if (ModelState.IsValid)
            {
                db.Add(userSub);//в бд
                await db.SaveChangesAsync();
                return RedirectToAction("Subscriptions", new {Id = userSub.UserId });
            }
            return View(userSub);
        }
        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFile = await db.Subscriptions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFile == null)
            {
                return NotFound();
            }

            return View(userFile);
        }

        // POST: UserFiles/Delete/5
        [Authorize(Roles = "admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userFile = await db.Subscriptions.FindAsync(id);
            db.Subscriptions.Remove(userFile);
            await db.SaveChangesAsync();
            return RedirectToAction("Subscriptions", new {id = userFile.UserId });
        }
    }
}
