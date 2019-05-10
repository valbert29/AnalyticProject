using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using codemode_youtube.Models;
using Newtonsoft.Json;

namespace codemode_youtube.Controllers
{
    public class VisualizeDataController : Controller
    {
    
        public ActionResult ColumnChart()
        {
            return View();
        }
     
        public ActionResult PieChart()
        {
            return View();
        }
 
        public ActionResult LineChart()
        {
            return View();
        }
         
        public ActionResult VisualizeData()
        { 
            return Json(Result(), JsonRequestBehavior.AllowGet);
        }
         
        public List<Data> Result()
        {
            string[] names;
            double[] values;
            try
            {
                names = Functions
                    .Parser.GetNames(Functions
                        .Parser.Reader(Functions
                        .Parser.FindLastFile(@"~\AnalyticProject\WebApplication3\wwwroot\Files"))).ToArray();
                values = Functions
                    .Parser.GetValues(Functions
                    .Parser.Reader(Functions
                    .Parser.FindLastFile(@"~\AnalyticProject\WebApplication3\wwwroot\Files"))).ToArray();
            }
            catch
            {
                names = new string[] { "error" };
                values = new double[] { 0 };

            }
            List<Data> data = new List<Data>();
            for (int i = 0; i < names.Length; i++)
            {
                data.Add(new Data()
                {
                    Names = names[i],
                    Values = values[i]
                });
            }
            
            return data;
        }
    }
} 