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
        FuncMethodController()
        {
            var service = LoggingDecorator<IMathFunc>.Create(new MathFunctions());
        }
        public IActionResult Index(string select)
        {
            return View();
        }
    }
}