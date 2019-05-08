using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AnaliticsFunctions;

namespace VSZANAL.Controllers
{
    public class FuncMethodController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}