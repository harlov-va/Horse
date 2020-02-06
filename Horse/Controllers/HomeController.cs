using Horse.BLL;
using Horse.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Horse.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IManager mng) : base(mng)
        { }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetCountMoves()
        {
            string msg = "";
            var result = false;
            var countMoves = 0;
            var parameters = AjaxModel.GetAjaxParameters(HttpContext);
            result = mng.PutFigure(parameters, out msg, out countMoves);
            if (result)
            {
                return Json(new { result = result, msg = msg, countMoves = countMoves });
            }
            else
            {
                return Json(new { result = result, msg = msg });
            }
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
    }
}