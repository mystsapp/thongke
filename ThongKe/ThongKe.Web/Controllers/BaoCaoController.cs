using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThongKe.Web.Infrastructure.Core;

namespace ThongKe.Web.Controllers
{
    public class BaoCaoController : BaseController
    {
        // GET: BaoCao
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult NgayCN()
        {

            return View();
        }
    }
}