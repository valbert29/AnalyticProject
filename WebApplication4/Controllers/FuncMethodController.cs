using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VSZANAL.Controllers
{
    public class FuncMethodController : Controller
    {
        
        public IActionResult Index(string select)
        {
            return View();
        }
    }
}