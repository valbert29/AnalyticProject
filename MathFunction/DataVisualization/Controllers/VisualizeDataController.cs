using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using codemode_youtube.Models;
using Functions;
using Newtonsoft.Json;

namespace DataVisualization.Controllers
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
         
        public ActionResult VisualizeData()
        { 
            return Json(Result(), JsonRequestBehavior.AllowGet);
        }
         
        public List<Data> Result()
        {
            List<Data> data = new List<Data>();
            try
            {
                string[] arrNames = Functions
                    .Parser.GetNames(Functions
                    .Parser.Reader(Functions
                    .Parser.FindLastFile(@"~\wwwroot\Files"))).ToArray();
                double[] arrValues = Functions
                    .Parser.GetValues(Functions
                    .Parser.Reader(Functions
                    .Parser.FindLastFile(@"~\wwwroot\Files"))).ToArray();
            }
            catch
            {
                string[] arrNames = new string[]{ "неверный формат данных"};
                double[] arrValues = new double[] { 0.0 };
            }

            data.Add(new Data()
            {
                Names = "Atir",
                Values = 88
            });

            return data;
        }
    }
}