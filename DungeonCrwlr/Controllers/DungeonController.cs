using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DungeonCrwlr.Controllers
{
    public class DungeonController : Controller
    {
        //
        // GET: /Dungeon/
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NewSession()
        {
            var SessionGuid = Guid.NewGuid().ToString();
            System.IO.File.WriteAllText(Server.MapPath($@"~/App_Data/{SessionGuid}.json"), "");

            return View("NewSession", (object)SessionGuid);
        }

        public ActionResult JoinSessionWithId(string sessionId)
        {
            return RedirectToAction("RunSession", new { sessionId = sessionId });
        }

        public ActionResult JoinSession()
        {
            return View();
        }

        public ActionResult RunSession(string sessionId)
        {
            return View("RunSession", null, sessionId );
        }

        public string GetLayout(string sessionId)
        {
            var _result = string.Empty;

            try
            {
                using (StreamReader r = new StreamReader(Server.MapPath($@"~/App_Data/{sessionId}.json")))
                {
                    _result = r.ReadToEnd();
                }
            }
            catch (Exception _ex)
            {
                _result = _ex.Message;
            }

            return _result;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public string SaveLocations(string json)
        {
            var _sessionId = Session["SessionId"].ToString();

            if (!String.IsNullOrEmpty(_sessionId))
            {
                try
                {
                    var _json = JsonConvert.SerializeObject(json, Formatting.None);
                    System.IO.File.WriteAllText(Server.MapPath($@"~/App_Data/{_sessionId}.json"), _json);
                    return "Saved";
                }
                catch (Exception _ex)
                {
                    return "Save Failed";
                }
            }

            return "Session Ended.  Please Rejoin your game";
        }
    }
}