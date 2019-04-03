using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebApplication3.Models;

namespace VSZANAL.Controllers
{
    public class UserFilesController : Controller
    {
        private readonly RUNContext _context;
        private readonly IHostingEnvironment _appEnvironment;

        public UserFilesController(RUNContext context, IHostingEnvironment appEnvironment)
        {
            _context = context;
            _appEnvironment = appEnvironment;
        }

        // GET: UserFiles
        public async Task<IActionResult> Index()
        {
            var login = HttpContext.Response.HttpContext.User.Identity.Name;
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
            ViewBag.login = user.Id;
            var rUNContext = _context.Files.Include(u => u.User);
            return View(await rUNContext.ToListAsync());

        }

        // GET: UserFiles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFile = await _context.Files
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFile == null)
            {
                return NotFound();
            }

            return View(userFile);
        }

        // GET: UserFiles/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }



        // POST: UserFiles/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Path,Time,UserId")] UserFile userFile, string text)
        {
            if (ModelState.IsValid)
            {
                var login = HttpContext.Response.HttpContext.User.Identity.Name;
                User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                var time = DateTime.Now;
                var filename = userFile.Name + "_" + time.ToShortDateString().Replace('.', '-') + '-' + time.ToLongTimeString().Replace(':', '-') + ".txt";
                userFile.Time = time;
                userFile.Path = "/Files/" + filename;
                userFile.UserId = user.Id;
                _context.Add(userFile);//в бд
                SaveToFile(text, filename);//в папку
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userFile.UserId);
            return View(userFile);
        }

        private void SaveToFile(string text, string filename)
        {
            //var filename = name + "_" + time.ToShortDateString().Replace('.', '-') + '-' + time.ToLongTimeString().Replace(':', '-') + ".txt";

            string path = "/Files/" + filename;

            try
            {
                using (StreamWriter sw = new StreamWriter(_appEnvironment.WebRootPath + path, false, System.Text.Encoding.Default))
                {
                    sw.WriteLine(text);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // GET: UserFiles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFile = await _context.Files.FindAsync(id);
            var realname = userFile.Path.Remove(0, 7);
            GetFile(realname);
            if (userFile == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userFile.UserId);
            return View(userFile);
        }

        private string ToShortName(string fileName, int count)
        {
            return fileName.Remove(fileName.Length - count, count);
        }

        private void GetFile(string fileName)
        {
            ViewData["fileName"] = fileName;
            try
            {
                var path = "/Files/" + fileName;
                using (StreamReader sr = new StreamReader(_appEnvironment.WebRootPath + path))
                {
                    ViewData["fileTXT"] = sr.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        // POST: UserFiles/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Path,Time,UserId")] UserFile userFile,
            string text, string oldName)
        {
            if (id != userFile.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var login = HttpContext.Response.HttpContext.User.Identity.Name;
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                    var time = DateTime.Now;
                    var filename = userFile.Name + "_" + time.ToShortDateString().Replace('.', '-') + '-' + time.ToLongTimeString().Replace(':', '-') + ".txt";
                    userFile.Time = time;
                    userFile.Path = "/Files/" + filename;
                    userFile.UserId = user.Id;
                    _context.Update(userFile);
                    await _context.SaveChangesAsync();
                    SaveFileAfterEdit(filename, text, oldName);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserFileExists(userFile.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", userFile.UserId);
            return View(userFile);
        }

        private void SaveFileAfterEdit(string filename, string text, string oldName)
        {
            var path = _appEnvironment.WebRootPath + "/Files/" + oldName;
            RemoveFile(path);
            SaveToFile(text, filename);
        }

        // GET: UserFiles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userFile = await _context.Files
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userFile == null)
            {
                return NotFound();
            }

            return View(userFile);
        }

        // POST: UserFiles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userFile = await _context.Files.FindAsync(id);
            var path = _appEnvironment.WebRootPath + userFile.Path;
            RemoveFile(path);
            _context.Files.Remove(userFile);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public FileResult Download(string filename)
        {
            var path =_appEnvironment.WebRootPath + filename;
            // Объект Stream
            FileStream fs = new FileStream(path, FileMode.Open);
            string file_type = "application/txt";
            string file_name = ToShortName(filename) + ".txt";
            return File(fs, file_type, file_name);
        }

        //private async User GetUser()
        //{
        //    var login = HttpContext.Response.HttpContext.User.Identity.Name;
        //    return await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
        //}

        private string ToShortName(string fileName)
        {
            return fileName.Remove(fileName.Length - 24, 24).Remove(0, 7);
        }

        private bool UserFileExists(int id)
        {
            return _context.Files.Any(e => e.Id == id);
        }

        private void RemoveFile(string path) => System.IO.File.Delete(path);
    }
}
