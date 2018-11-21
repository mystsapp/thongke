using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThongKe.Service;

namespace ThongKe.Web.Controllers
{
    public class accountController : Controller
    {
        private IaccountService _accountService;

        public accountController(IaccountService accountService)
        {
            _accountService = accountService;
        }
        // GET: account
        public ActionResult Index(string name, string status, int page=1, int pageSize=10)

        {
            int totalRow = 0;

            var listAccout = _accountService.Search(name, page, pageSize, status, out totalRow);
            return View(listAccout);
        }
    }
}