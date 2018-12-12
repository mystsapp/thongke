using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThongKe.Common.ViewModel;
using ThongKe.Data.Models.EF;
using ThongKe.Service;
using ThongKe.Web.Infrastructure.Core;
using ThongKe.Web.Models;

namespace ThongKe.Web.Controllers
{
    public class HomeController : BaseController
    {
        private IThongKeService _thongkeService;

        public HomeController(IThongKeService thongKeService)
        {
            _thongkeService = thongKeService;
        }
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult LoadDataKhachLeHethong(string tungay, string denngay, string chinhanh, string khoi)
        {
            tungay = "01/12/2018";
            denngay = "11/12/2018";
            chinhanh = "STS";
            khoi = "OB";

            var listDoanhthu = _thongkeService.doanhthuKhachLeHeThongEntities(tungay, denngay, chinhanh, khoi);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<doanhthuToanhethong>, IEnumerable<doanhthuToanhethongViewModel>>(listDoanhthu);

            return Json(new
            {
                data = responseData,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }
    }
}