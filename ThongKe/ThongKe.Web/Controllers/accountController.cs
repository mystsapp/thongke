using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ThongKe.Data.Models;
using ThongKe.Service;
using ThongKe.Web.Models;

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
        public ActionResult Index()

        {
            //int totalRow = 0;

            //var listAccout = _accountService.Search(name, page, pageSize, status, out totalRow);
            return View();
        }

        [HttpGet]
        public JsonResult LoadData(string name, string status, int page, int pageSize)
        {
            int totalRow = 0;

            var listAccount = _accountService.Search(name, page, pageSize, status, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<account>, IEnumerable<accountViewModel>>(listAccount);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDetail(string id)
        {
            bool statusUser = false;
            var user = _accountService.GetById(id);
            if (user != null)
            {
                statusUser = true;

            }
            return Json(new
            {
                data = user,
                status = statusUser
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SaveData(string strUser, string Hidid)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            var user = serializer.Deserialize<account>(strUser);
            bool status = true;
            string message = string.Empty;
            if (Hidid == "0")//dang them
            {
                user.password = _accountService.EncodeSHA1(user.password);
                user.ngaytao = DateTime.Now;
                user.ngaycapnhat = DateTime.Now;
                try
                {
                    _accountService.Add(user);
                    _accountService.Save();
                }
                catch (Exception ex)
                {
                    status = false;
                    //message = ex.Message;
                    throw ex;
                }
            }
            else if (Hidid != "0")
            {
                var oldUser = _accountService.GetById(user.username);
                if (user.password != oldUser.password) //password field is required
                {
                    user.password = _accountService.EncodeSHA1(user.password);
                }

                user.password = _accountService.EncodeSHA1(user.password);
                try
                {
                    _accountService.Update(user);
                    _accountService.Save();
                }
                catch (Exception ex)
                {
                    message = ex.Message;
                    status = false;
                    throw ex;
                }
            }
            return Json(new
            {
                status = status,
                message = message
            });
        }

        [HttpPost]
        public JsonResult Delete(string id)
        {
            bool status = true;
            string message = string.Empty;
            try
            {
                _accountService.Delete(id);
                _accountService.Save();
            }
            catch (Exception ex)
            {
                status = false;
                message = ex.Message;
            }

            return Json(new
            {
                status = status,
                message = message
            });
        }
    }
}