using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using ThongKe.Data.Repositories;
using ThongKe.Service;
using ThongKe.Web.Infrastructure.Core;
using System.Web.Script.Serialization;
using ThongKe.Data.Models.EF;
using ThongKe.Web.Infrastructure.Extensions;
using ThongKe.Web.Models;
using Newtonsoft.Json;
using AutoMapper;

namespace ThongKe.Web.Controllers
{
    public class BaoCaoController : BaseController
    {
        private IThongKeService _thongkeService;
        private IaccountService _accountService;
        private ICommonService _commonService;
        public BaoCaoController(IThongKeService thongKeService)
        {
            _thongkeService = thongKeService;
           
        }
        // GET: BaoCao
        public ActionResult Index()
        {
            return View();
        }

        public ViewResult QuayTheoNgayBan()
        {

            return View();
        }//
        
        [HttpPost]
        public ViewResult QuayTheoNgayBan(string tungay, string denngay,string cn,string khoi)
        {
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            cn =  String.IsNullOrEmpty(cn)? Session["chinhanh"].ToString():cn ;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 40;// quay
            xlSheet.Column(2).Width = 10;// cn
            xlSheet.Column(3).Width = 10;// so khach
            xlSheet.Column(4).Width = 20;// doanh số
            xlSheet.Column(5).Width = 20;// doanh thu

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ QUẦY "+khoi+"  "+cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 5].Merge = true;
            setCenterAligment(2, 1, 2, 5, xlSheet);
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày: " + tungay;
            }
            else
            {
                fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 5, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "Quầy ";
            xlSheet.Cells[5, 2].Value = "Code CN ";
            xlSheet.Cells[5, 3].Value = "Số khách";
            xlSheet.Cells[5, 4].Value = "Doanh số";
            xlSheet.Cells[5, 5].Value = "Thực thu";
            xlSheet.Cells[5, 1, 5, 3].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;

            DataTable dt = _thongkeService.doanhthuQuayTheoNgayBan(tungay, denngay, cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {

                        //if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        //{
                        //    xlSheet.Cells[dong, j + 1].Value = "";
                        //}
                        //else
                        //{
                        xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        //}
                    }
                }

            }
            else
            {
                SetAlert("No sale.", "warning");
                return View();
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 2].Value = "TC";
            xlSheet.Cells[dong, 3].Formula = "SUM(C6:C" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 4, dong, 5, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 5, 12, xlSheet);
            setBorder(5, 1, 5 + dt.Rows.Count, 5, xlSheet);
            // font bold tieu de bang
            setFontBold(5, 1, 5, 5, 12, xlSheet);
            // font bold dong cuoi cùng
            setFontBold(dong, 1, dong, 5, 12, xlSheet);
            setBorder(dong, 2, dong, 5, xlSheet);

          
            // canh giưa cot chinhanh va so khach
            setCenterAligment(6, 2, 6 + dt.Rows.Count, 3, xlSheet);
            // dinh dạng number cot sokhach, doanh so, thuc thu
            NumberFormat(6, 4, 6 + dt.Rows.Count, 5, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay"+khoi+" "+cn + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }

        public ActionResult SaleTheoQuay()
        {
            return View();
        }//

        [HttpPost]
        public ViewResult SaleTheoQuay(string tungay, string denngay,string daily,string khoi)//(string tungay,string denngay, string daily)
        {
            
            string cn = Session["chinhanh"].ToString();
            khoi = String.IsNullOrEmpty(khoi)? Session["khoi"].ToString():khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 50;// sales
            xlSheet.Column(2).Width = 30;// doanh so
            xlSheet.Column(3).Width = 30;// doanh thu sale

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ QUẦY " +khoi+" " +daily;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 3].Merge = true;
            setCenterAligment(2, 1, 2, 3, xlSheet);
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày: " + tungay;
            }
            else
            {
                fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 3].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 3, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "Nhân viên ";

            xlSheet.Cells[5, 2].Value = "Doanh số";
            xlSheet.Cells[5, 3].Value = "Doanh thu";

            xlSheet.Cells[5, 1, 5, 3].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table  
            int dong = 5;
           

            DataTable dt = _thongkeService.doanhthuSaleTheoQuay(tungay, denngay, daily,cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            xlSheet.Cells[dong, j + 1].Value = "";
                        }
                        else
                        {
                            xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        }
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return View();
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 2].Formula = "SUM(B6:B" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 3].Formula = "SUM(C6:C" + (6 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 2, dong, 3, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count, 3, xlSheet);
            setFontBold(5, 1, 5, 3, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 3, 12, xlSheet);
            NumberFormat(6, 2, 6 + dt.Rows.Count, 3, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay_" + Session["daily"].ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuSale_" + khoi+" "+daily + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }

        public ViewResult SaleTheoNgayDi()
        {

            return View();
        }//

        [HttpPost]
        public ViewResult SaleTheoNgayDi(string tungay, string denngay,string daily,string khoi)
        {
            string cn = Session["chinhanh"].ToString();
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 50;// sales
            xlSheet.Column(2).Width = 30;// doanh so
            xlSheet.Column(3).Width = 30;// doanh thu sale

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU SALE " +khoi+" "+cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 3].Merge = true;
            setCenterAligment(2, 1, 2, 3, xlSheet);
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày: " + tungay;
            }
            else
            {
                fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 3].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 3, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "Nhân viên ";

            xlSheet.Cells[5, 2].Value = "Doanh số";
            xlSheet.Cells[5, 3].Value = "Doanh thu";

            xlSheet.Cells[5, 1, 5, 3].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;
           

            DataTable dt = _thongkeService.doanhthuSaleTheoNgayDi(tungay, denngay, daily,cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            xlSheet.Cells[dong, j + 1].Value = "";
                        }
                        else
                        {
                            xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        }
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return View();
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 2].Formula = "SUM(B6:B" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 3].Formula = "SUM(C6:C" + (6 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 2, dong, 3, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count, 3, xlSheet);
            setFontBold(5, 1, 5, 3, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 3, 12, xlSheet);
            NumberFormat(6, 2, 6 + dt.Rows.Count, 3, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay_" + Session["daily"].ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuSale_" + khoi + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }

        public ActionResult DoanTheoNgayDi()
        {
            return View();
        }//

        [HttpPost]
        public ViewResult DoanTheoNgayDi(string tungay, string denngay,string cn,string khoi)
        {
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi)? Session["khoi"].ToString():khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 25;// sgtcode
            xlSheet.Column(2).Width = 40;// tuyen tq
            xlSheet.Column(3).Width = 20;// bat dau 
            xlSheet.Column(4).Width = 20;// ket thu
            xlSheet.Column(5).Width = 10;// so khach
            xlSheet.Column(6).Width = 25;//doanh thu

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU THEO ĐOÀN  "+khoi+"  "+cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 6].Merge = true;
            setCenterAligment(2, 1, 2, 6, xlSheet);
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày: " + tungay;
            }
            else
            {
                fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 6].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 6, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "Sgt Code ";
            xlSheet.Cells[5, 2].Value = "Tuyến tham quan ";
            xlSheet.Cells[5, 3].Value = "Ngày đi";
            xlSheet.Cells[5, 4].Value = "Ngày về";
            xlSheet.Cells[5, 5].Value = "Số khách";
            xlSheet.Cells[5, 6].Value = "Doanh thu";
            xlSheet.Cells[5, 1, 5, 6].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;
           

            DataTable dt = _thongkeService.doanhthuDoanTheoNgay(tungay, denngay, cn, khoi);

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            xlSheet.Cells[dong, j + 1].Value = "";
                        }
                        else
                        {
                            xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        }
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return View();
            }
            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 4].Value = "TC";
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + dt.Rows.Count - 1) + ")";

            // định dạng số
            NumberFormat(dong, 5, dong, 6, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count, 6, xlSheet);
            setFontBold(5, 1, 5, 6, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 6, 12, xlSheet);

            setBorder(dong, 4,dong, 6, xlSheet);
            setFontBold(dong, 4, dong, 6, 12, xlSheet);

            // dinh dạng ngay thang cho cot ngay di , ngay ve
            DateTimeFormat(6, 3, 6 + dt.Rows.Count, 4, xlSheet);
            // canh giưa cot  ngay di, ngay ve, so khach 
            setCenterAligment(6, 3, 6 + dt.Rows.Count, 5, xlSheet);
            // dinh dạng number cot doanh so
            NumberFormat(6, 6, 6 + dt.Rows.Count, 6, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuDoan_" +khoi+ "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }

        public ActionResult TuyentqTheoNgayDi()
        {

            return View();
        }//

        [HttpPost]
        public ViewResult TuyentqTheoNgayDi(string tungay, string denngay,string cn,string khoi)
        {
            cn = String.IsNullOrEmpty(cn)? Session["chinhanh"].ToString():cn;
            khoi = String.IsNullOrEmpty(khoi)? Session["khoi"].ToString():khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;// chi nhanh
            xlSheet.Column(2).Width = 40;// tuyen tq
            xlSheet.Column(3).Width = 10;// skkl
            xlSheet.Column(4).Width = 20;// doanh thu kl
            xlSheet.Column(5).Width = 10;// sk kd
            xlSheet.Column(6).Width = 20;//doanh thu kd
            xlSheet.Column(7).Width = 15;// tong khach
            xlSheet.Column(8).Width = 20;// tong doanh thu

            xlSheet.Cells[2, 1].Value = "TUYẾN THAM QUAN THEO NGÀY ĐI TOUR "+Session["chinhanh"].ToString();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 8].Merge = true;
            setCenterAligment(2, 1, 2, 8, xlSheet);
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày: " + tungay;
            }
            else
            {
                fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 8, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "Code CN";
            xlSheet.Cells[5, 2].Value = "Tuyến tham quan ";
            xlSheet.Cells[5, 3].Value = "SK KL";
            xlSheet.Cells[5, 4].Value = "DT KL";
            xlSheet.Cells[5, 5].Value = "SK KĐ";
            xlSheet.Cells[5, 6].Value = "DT KĐ";
            xlSheet.Cells[5, 7].Value = "TỔNG SK";
            xlSheet.Cells[5, 8].Value = "TỔNG DT";

            xlSheet.Cells[5, 1, 5, 8].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;
           

            DataTable dt = _thongkeService.doanhthuTuyentqTheoNgay(tungay, denngay, cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        //{
                        //    xlSheet.Cells[dong, j + 1].Value = "";
                        //}
                        //else
                        //{
                        //    xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        //}
                        xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return View();
            }
         

            setBorder(5, 1, 5 + dt.Rows.Count, 8, xlSheet);
            setFontBold(5, 1, 5, 8, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 8, 12, xlSheet);
            // canh giưa cot  chi nhanh
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);
            // canh giưa cot  skkl
            setCenterAligment(6, 3, 6 + dt.Rows.Count, 3, xlSheet);
            // canh giưa cot  skkd
            setCenterAligment(6, 5, 6 + dt.Rows.Count, 5, xlSheet);
            // canh giưa cot  tong khach
            setCenterAligment(6, 7, 6 + dt.Rows.Count, 7, xlSheet);

            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 4].Formula = "SUM(F6:F" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 4].Formula = "SUM(H6:H" + (6 + dt.Rows.Count - 1) + ")";

            // dinh dạng number cot dt kl
            NumberFormat(6, 4, 6 + dt.Rows.Count, 4, xlSheet);
            // dinh dạng number cot dt kd
            NumberFormat(6, 6, 6 + dt.Rows.Count+1, 6, xlSheet);
            // dinh dạng number cot tong dt
            NumberFormat(6, 8, 6 + dt.Rows.Count+1, 8, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuTuyentq" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }



        public ActionResult QuayTheoNgayDi()
        {
            return View();
        }//
       
           
        [HttpPost]
        public ViewResult QuayTheoNgayDi(string tungay, string denngay,string cn,string khoi)
        {
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString():khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 40;// quay
            xlSheet.Column(2).Width = 10;// cn
            xlSheet.Column(3).Width = 10;// so khach
            xlSheet.Column(4).Width = 20;// doanh số
            xlSheet.Column(5).Width = 20;// doanh thu

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ QUẦY "+ khoi+" "+cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 5].Merge = true;
            setCenterAligment(2, 1, 2, 5, xlSheet);
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày: " + tungay;
            }
            else
            {
                fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 5].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 5, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "Quầy ";
            xlSheet.Cells[5, 2].Value = "Code CN ";
            xlSheet.Cells[5, 3].Value = "Số khách";
            xlSheet.Cells[5, 4].Value = "Doanh số";
            xlSheet.Cells[5, 5].Value = "Thực thu";
            xlSheet.Cells[5, 1, 5, 3].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;
           
            DataTable dt = _thongkeService.doanhthuQuayTheoNgayDi(tungay, denngay, cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        //if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        //{
                        //    xlSheet.Cells[dong, j + 1].Value = "";
                        //}
                        //else
                        //{
                        xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        //}
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return View();
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 2].Value = "TC";
            xlSheet.Cells[dong, 3].Formula = "SUM(C6:C" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 4, dong, 5, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 5, 12, xlSheet);
            setBorder(5, 1, 5 + dt.Rows.Count, 5, xlSheet);
            setFontBold(5, 1, 5, 5, 12, xlSheet);

            setBorder(dong, 2, dong, 5, xlSheet);
            setFontBold(dong, 1, dong, 5, 12, xlSheet);
            // canh giưa cot chinhanh va so khach
            setCenterAligment(6, 2, 6 + dt.Rows.Count, 3, xlSheet);
            // dinh dạng number cot sokhach, doanh so, thuc thu
            NumberFormat(6, 3, 6 + dt.Rows.Count, 5, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }


        public ViewResult KhachleHethong()
        {

            return View();
        }//

        [HttpPost]
        public ViewResult KhachLeHethong(string tungay, string denngay,string cn,string khoi)
        {
            cn = String.IsNullOrEmpty(cn)? Session["chinhanh"].ToString():cn;
            khoi = String.IsNullOrEmpty(khoi)? Session["khoi"].ToString():khoi;

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("lienketkhachle");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;// cn
            xlSheet.Column(2).Width = 40;// quay
            xlSheet.Column(3).Width = 10;// so khach hien tai
            xlSheet.Column(4).Width = 20;// doanh số hien tai
            xlSheet.Column(5).Width = 10;// so khach nam truoc
            xlSheet.Column(6).Width = 20; // doanh thu nam truoc
            xlSheet.Column(7).Width = 15; // ti le so khach
            xlSheet.Column(8).Width = 20;// doanh thu so sanh

            xlSheet.Cells[2, 1].Value = "LIÊN KẾT KHÁCH LẼ HỆ THỐNG "+ khoi+ "  "+cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 8].Merge = true;
            setCenterAligment(2, 1, 2, 8, xlSheet);
            // dinh dang tu ngay den ngay
            if (tungay == denngay)
            {
                fromTo = "Ngày: " + tungay;
            }
            else
            {
                fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
            }
            xlSheet.Cells[3, 1].Value = fromTo;
            xlSheet.Cells[3, 1, 3, 8].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 8, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "Code CN ";
            xlSheet.Cells[5, 1, 6, 1].Merge = true;
            xlSheet.Cells[5, 2].Value = "Quầy ";
            xlSheet.Cells[5, 2, 6, 2].Merge = true;

            xlSheet.Cells[5, 3].Value = "Thời điểm thống kê";
            xlSheet.Cells[5, 3, 5, 4].Merge = true;


            xlSheet.Cells[5, 5].Value = "So sánh cùng kỳ";
            xlSheet.Cells[5, 5, 5, 6].Merge = true;

            xlSheet.Cells[5, 7].Value = "Tỉ lệ tăng giảm ";
            xlSheet.Cells[5, 7, 5, 8].Merge = true;
            // dong thu 2
            xlSheet.Cells[6, 3].Value = "Số khách";
            xlSheet.Cells[6, 4].Value = "Doanh thu";
            xlSheet.Cells[6, 5].Value = "Số khách";
            xlSheet.Cells[6, 6].Value = "Doanh Thu";
            xlSheet.Cells[6, 7].Value = "Số khách";
            xlSheet.Cells[6, 8].Value = "Doanh thu";
            setCenterAligment(5, 1, 6, 8, xlSheet);
            xlSheet.Cells[5, 1, 6, 8].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 6;
           
            DataTable dt = _thongkeService.doanhthuKhachleHethong(tungay, denngay, cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dong++;
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        if (String.IsNullOrEmpty(dt.Rows[i][j].ToString()))
                        {
                            xlSheet.Cells[dong, j + 1].Value = 0;
                        }
                        else
                        {
                            xlSheet.Cells[dong, j + 1].Value = dt.Rows[i][j];
                        }

                        xlSheet.Cells[dong, 7].Value = (Convert.ToInt32(xlSheet.Cells[dong, 3].Value) - Convert.ToInt32(xlSheet.Cells[dong, 5].Value));
                        xlSheet.Cells[dong, 8].Value = Convert.ToDecimal(xlSheet.Cells[dong, 4].Value) - Convert.ToDecimal(xlSheet.Cells[dong, 6].Value);
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return View();
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 3].Formula = "SUM(C6:C" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 8].Formula = "SUM(H6:H" + (7 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 4, dong, 5, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count + 2, 8, xlSheet);
            setFontBold(5, 1, 5, 5, 12, xlSheet);
            setFontSize(7, 1, 6 + dt.Rows.Count + 2, 8, 12, xlSheet);
            // dinh dang giu cho so khach
            setCenterAligment(7, 3, 7 + dt.Rows.Count, 3, xlSheet);
            setCenterAligment(7, 5, 7 + dt.Rows.Count, 5, xlSheet);
            setCenterAligment(7, 7, 7 + dt.Rows.Count, 7, xlSheet);
            // dinh dạng number cot sokhach, doanh so, thuc thu
            NumberFormat(7, 4, 7 + dt.Rows.Count + 1, 4, xlSheet);
            NumberFormat(7, 6, 6 + dt.Rows.Count + 1, 6, xlSheet);
            NumberFormat(7, 8, 6 + dt.Rows.Count + 1, 8, xlSheet);

            xlSheet.View.FreezePanes(7, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "LienketKhachle" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }

      
        ///////////////////////////
        private static void NumberFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
                range.Style.Numberformat.Format = "#,#0";
            }
        }
        private static void DateFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Numberformat.Format = "dd/MM/yyyy";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
        private static void DateTimeFormat(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Numberformat.Format = "dd/MM/yyyy HH:mm";
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
        private static void setRightAligment(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
            }
        }
        private static void setCenterAligment(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
            }
        }
        private static void setFontSize(int fromRow, int fromColumn, int toRow, int toColumn, int fontSize, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Font.SetFromFont(new Font("Times New Roman", fontSize, FontStyle.Regular));
            }
        }
        private static void setFontBold(int fromRow, int fromColumn, int toRow, int toColumn, int fontSize, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Font.SetFromFont(new Font("Times New Roman", fontSize, FontStyle.Bold));
            }
        }
        private static void setBorder(int fromRow, int fromColumn, int toRow, int toColumn, ExcelWorksheet sheet)
        {
            using (var range = sheet.Cells[fromRow, fromColumn, toRow, toColumn])
            {
                range.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                range.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }
        }
    }
}