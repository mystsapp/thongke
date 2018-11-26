using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThongKe.Common.ViewModel;
using ThongKe.Web.Infrastructure.Core;

namespace ThongKe.Web.Controllers
{
    public class HomeController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult Statistic(string fromDate, string toDate)
        {
            return Json(new
            {
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}