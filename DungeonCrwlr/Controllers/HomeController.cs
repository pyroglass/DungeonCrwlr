using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace DungeonCrwlr.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string SaveLocations(string _p_Json)
        {
            try
            {
                var _json = JsonConvert.SerializeObject(_p_Json, Formatting.None);
                System.IO.File.WriteAllText(Server.MapPath("~/App_Data/_locations.json"), _json);
                return "Saved";
            }
            catch (Exception _ex)
            {
                return "Save Failed";
            }
        }

    }
}