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

        public ActionResult SaleTheoQuay()
        {
            return View();
        }//

        [HttpPost]
        public ViewResult SaleTheoQuay(string tungay, string denngay, string cn, string khoi)//(string tungay,string denngay, string daily)
        {
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//stt
            xlSheet.Column(2).Width = 50;// sales
            xlSheet.Column(3).Width = 30;// doanh so
            xlSheet.Column(4).Width = 30;// doanh thu sale

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ QUẦY " + khoi + " " + cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 4].Merge = true;
            setCenterAligment(2, 1, 2, 4, xlSheet);
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
            xlSheet.Cells[3, 1, 3, 4].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 4, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhân viên ";

            xlSheet.Cells[5, 3].Value = "Doanh số";
            xlSheet.Cells[5, 4].Value = "Thực thu";

            xlSheet.Cells[5, 1, 5, 4].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table  
            int dong = 5;


            DataTable dt = _thongkeService.doanhthuSaleTheoQuay(tungay, denngay, cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

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
            xlSheet.Cells[dong, 3].Formula = "SUM(C6:C" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (6 + dt.Rows.Count - 1) + ")";


            setBorder(5, 1, 5 + dt.Rows.Count, 4, xlSheet);
            setFontBold(5, 1, 5, 3, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 4, 12, xlSheet);
            // canh giua cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);
            NumberFormat(6, 3, 6 + dt.Rows.Count, 4, xlSheet);
            // định dạng số cot tong cong
            NumberFormat(dong, 2, dong, 3, xlSheet);
            setBorder(dong, 3, dong, 4, xlSheet);
            setFontBold(dong, 3, dong, 4, 12, xlSheet);
            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay_" + Session["daily"].ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuSale_" + khoi + " " + cn + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }

        public ViewResult SaleTheoNgayDi()
        {

            return View();
        }//

        [HttpPost]
        public ViewResult SaleTheoNgayDi(string tungay, string denngay, string daily, string khoi)
        {
            string cn = Session["chinhanh"].ToString();
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//stt
            xlSheet.Column(2).Width = 50;// sales
            xlSheet.Column(3).Width = 30;// doanh so
            xlSheet.Column(4).Width = 30;// doanh thu sale

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU SALE " + khoi + " " + cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 4].Merge = true;
            setCenterAligment(2, 1, 2, 4, xlSheet);
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
            xlSheet.Cells[3, 1, 3, 4].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 4, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Nhân viên ";

            xlSheet.Cells[5, 3].Value = "Doanh số";
            xlSheet.Cells[5, 4].Value = "Thực thu";

            xlSheet.Cells[5, 1, 5, 4].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;


            DataTable dt = new DataTable();
            dt = _thongkeService.doanhthuSaleTheoNgayDi(tungay, denngay, cn, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

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
            xlSheet.Cells[dong, 3].Formula = "SUM(C6:C" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (6 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 3, dong, 4, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count, 4, xlSheet);
            setFontBold(5, 1, 5, 3, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 4, 12, xlSheet);
            // canh giua cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);

            NumberFormat(6, 3, 6 + dt.Rows.Count, 4, xlSheet);
            setBorder(dong, 3, dong, 4, xlSheet);
            setFontBold(dong, 3, dong, 4, 12, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay_" + Session["daily"].ToString() + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuSale_" + khoi + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }


        public ViewResult QuayTheoNgayBan()
        {

            return View();
        }//

        [HttpPost]
        public ViewResult QuayTheoNgayBan(string tungay, string denngay, string cn, string khoi)
        {
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//stt
            xlSheet.Column(2).Width = 40;// quay
            xlSheet.Column(3).Width = 10;// cn
            xlSheet.Column(4).Width = 10;// so khach
            xlSheet.Column(5).Width = 20;// doanh số
            xlSheet.Column(6).Width = 20;// doanh thu

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ QUẦY " + khoi + "  " + cn;
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
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Quầy ";
            xlSheet.Cells[5, 3].Value = "Code CN ";
            xlSheet.Cells[5, 4].Value = "Số khách";
            xlSheet.Cells[5, 5].Value = "Doanh số";
            xlSheet.Cells[5, 6].Value = "Thực thu";
            xlSheet.Cells[5, 1, 5, 6].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

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

            // Sum tổng tiền
            xlSheet.Cells[dong, 3].Value = "TC";
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 5, dong, 6, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 6, 12, xlSheet);
            setBorder(5, 1, 5 + dt.Rows.Count, 6, xlSheet);
            // font bold tieu de bang
            setFontBold(5, 1, 5, 6, 12, xlSheet);
            // font bold dong cuoi cùng
            setFontBold(dong, 1, dong, 6, 12, xlSheet);
            setBorder(dong, 3, dong, 6, xlSheet);
            // canh giưa cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);

            // canh giưa cot chinhanh va so khach
            setCenterAligment(6, 3, 6 + dt.Rows.Count, 4, xlSheet);
            // dinh dạng number cot sokhach, doanh so, thuc thu
            NumberFormat(6, 5, 6 + dt.Rows.Count, 6, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay" + khoi + " " + cn + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }



        public ActionResult QuayTheoNgayDi()
        {
            return View();
        }//


        [HttpPost]
        public ViewResult QuayTheoNgayDi(string tungay, string denngay, string cn, string khoi)
        {
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 40;// quay
            xlSheet.Column(3).Width = 10;// cn
            xlSheet.Column(4).Width = 10;// so khach
            xlSheet.Column(5).Width = 20;// doanh số
            xlSheet.Column(6).Width = 20;// doanh thu

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ QUẦY " + khoi + " " + cn;
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
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Quầy ";
            xlSheet.Cells[5, 3].Value = "Code CN ";
            xlSheet.Cells[5, 4].Value = "Số khách";
            xlSheet.Cells[5, 5].Value = "Doanh số";
            xlSheet.Cells[5, 6].Value = "Thực thu";
            xlSheet.Cells[5, 1, 5, 6].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

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
            xlSheet.Cells[dong, 3].Value = "TC";
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 5, dong, 6, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 6, 12, xlSheet);
            setBorder(5, 1, 5 + dt.Rows.Count, 6, xlSheet);
            setFontBold(5, 1, 5, 6, 12, xlSheet);
            // canh giưa cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);

            setBorder(dong, 3, dong, 6, xlSheet);
            setFontBold(dong, 1, dong, 6, 12, xlSheet);
            // canh giưa cot chinhanh va so khach
            setCenterAligment(6, 3, 6 + dt.Rows.Count, 4, xlSheet);
            // dinh dạng number cot sokhach, doanh so, thuc thu
            NumberFormat(6, 5, 6 + dt.Rows.Count, 6, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuay" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }

        public ActionResult DoanTheoNgayDi()
        {
            return View();
        }//

        [HttpPost]
        public ViewResult DoanTheoNgayDi(string tungay, string denngay, string cn, string khoi)
        {
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 25;// sgtcode
            xlSheet.Column(3).Width = 40;// tuyen tq
            xlSheet.Column(4).Width = 20;// bat dau 
            xlSheet.Column(5).Width = 20;// ket thu
            xlSheet.Column(6).Width = 10;// so khach
            xlSheet.Column(7).Width = 25;//doanh thu

            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU THEO ĐOÀN  " + khoi + "  " + cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 7].Merge = true;
            setCenterAligment(2, 1, 2, 7, xlSheet);
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
            xlSheet.Cells[3, 1, 3, 7].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 7, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Sgt Code ";
            xlSheet.Cells[5, 3].Value = "Tuyến tham quan ";
            xlSheet.Cells[5, 4].Value = "Ngày đi";
            xlSheet.Cells[5, 5].Value = "Ngày về";
            xlSheet.Cells[5, 6].Value = "Số khách";
            xlSheet.Cells[5, 7].Value = "Doanh thu";
            xlSheet.Cells[5, 1, 5, 7].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

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
            xlSheet.Cells[dong, 5].Value = "TC";
            xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (6 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (6 + dt.Rows.Count - 1) + ")";

            // định dạng số
            NumberFormat(dong, 6, dong, 7, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count, 7, xlSheet);
            setFontBold(5, 1, 5, 6, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 7, 12, xlSheet);
            // dinh dang giua cho cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 1, xlSheet);

            setBorder(dong, 5, dong, 7, xlSheet);
            setFontBold(dong, 5, dong, 7, 12, xlSheet);

            // dinh dạng ngay thang cho cot ngay di , ngay ve
            DateTimeFormat(6, 4, 6 + dt.Rows.Count, 5, xlSheet);
            // canh giưa cot  ngay di, ngay ve, so khach 
            setCenterAligment(6, 4, 6 + dt.Rows.Count, 6, xlSheet);
            // dinh dạng number cot doanh so
            NumberFormat(6, 7, 6 + dt.Rows.Count, 7, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuDoan_" + khoi + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }


        public ActionResult TuyentqTheoNgayDi()
        {

            return View();
        }//

        [HttpPost]
        public ViewResult TuyentqTheoNgayDi(string tungay, string denngay, string cn, string khoi)
        {
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 10;// chi nhanh
            xlSheet.Column(3).Width = 40;// tuyen tq
            xlSheet.Column(4).Width = 10;// skkl
            xlSheet.Column(5).Width = 20;// doanh thu kl
            xlSheet.Column(6).Width = 10;// sk kd
            xlSheet.Column(7).Width = 20;//doanh thu kd
            xlSheet.Column(8).Width = 15;// tong khach
            xlSheet.Column(9).Width = 20;// tong doanh thu

            xlSheet.Cells[2, 1].Value = "TUYẾN THAM QUAN THEO NGÀY ĐI TOUR " + Session["chinhanh"].ToString();
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 9].Merge = true;
            setCenterAligment(2, 1, 2, 9, xlSheet);
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
            xlSheet.Cells[3, 1, 3, 9].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 9, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Code CN";
            xlSheet.Cells[5, 3].Value = "Tuyến tham quan ";
            xlSheet.Cells[5, 4].Value = "SK KL";
            xlSheet.Cells[5, 5].Value = "DT KL";
            xlSheet.Cells[5, 6].Value = "SK KĐ";
            xlSheet.Cells[5, 7].Value = "DT KĐ";
            xlSheet.Cells[5, 8].Value = "TỔNG SK";
            xlSheet.Cells[5, 9].Value = "TỔNG DT";

            xlSheet.Cells[5, 1, 5, 9].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

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


            setBorder(5, 1, 5 + dt.Rows.Count, 9, xlSheet);
            setFontBold(5, 1, 5, 9, 12, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 9, 12, xlSheet);
            // canh giưa cot  STT VA CN
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 2, xlSheet);

            // canh giưa cot  skkl
            setCenterAligment(6, 4, 6 + dt.Rows.Count, 4, xlSheet);
            // canh giưa cot  skkd
            setCenterAligment(6, 6, 6 + dt.Rows.Count, 6, xlSheet);
            // canh giưa cot  tong khach
            setCenterAligment(6, 8, 6 + dt.Rows.Count, 8, xlSheet);

            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền

            dong++;

            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (6 + dt.Rows.Count - 1) + ")";


            xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (6 + dt.Rows.Count - 1) + ")";

            xlSheet.Cells[dong, 9].Formula = "SUM(I6:I" + (6 + dt.Rows.Count - 1) + ")";
            setBorder(dong, 5, dong, 9, xlSheet);
            setFontBold(dong, 5, dong, 9, 12, xlSheet);

            // dinh dạng number cot dt kl
            NumberFormat(6, 5, 6 + dt.Rows.Count, 5, xlSheet);
            // dinh dạng number cot dt kd
            NumberFormat(6, 7, 6 + dt.Rows.Count + 1, 7, xlSheet);
            // dinh dạng number cot tong dt
            NumberFormat(6, 9, 6 + dt.Rows.Count + 1, 9, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuTuyentq" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }


        public ViewResult KhachleHethong()
        {

            return View();
        }//

        [HttpPost]
        public ViewResult KhachLeHethong(string tungay, string denngay, string cn, string khoi)
        {
            cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;

            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("lienketkhachle");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 10;// cn
            xlSheet.Column(3).Width = 40;// quay
            xlSheet.Column(4).Width = 10;// so khach hien tai
            xlSheet.Column(5).Width = 20;// doanh số hien tai
            xlSheet.Column(6).Width = 10;// so khach nam truoc
            xlSheet.Column(7).Width = 20; // doanh thu nam truoc
            xlSheet.Column(8).Width = 15; // ti le so khach
            xlSheet.Column(9).Width = 20;// doanh thu so sanh

            xlSheet.Cells[2, 1].Value = "LIÊN KẾT KHÁCH LẼ HỆ THỐNG " + khoi + "  " + cn;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 9].Merge = true;
            setCenterAligment(2, 1, 2, 9, xlSheet);
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
            xlSheet.Cells[3, 1, 3, 9].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 9, xlSheet);

            // Tạo header

            xlSheet.Cells[5, 1].Value = "STT ";
            xlSheet.Cells[5, 1, 6, 1].Merge = true;
            xlSheet.Cells[5, 2].Value = "CN";
            xlSheet.Cells[5, 2, 6, 2].Merge = true;
            xlSheet.Cells[5, 3].Value = "Quầy ";
            xlSheet.Cells[5, 3, 6, 3].Merge = true;

            xlSheet.Cells[5, 4].Value = "Thời điểm thống kê";
            xlSheet.Cells[5, 4, 5, 5].Merge = true;


            xlSheet.Cells[5, 6].Value = "So sánh cùng kỳ";
            xlSheet.Cells[5, 6, 5, 7].Merge = true;

            xlSheet.Cells[5, 8].Value = "Tỉ lệ tăng giảm ";
            xlSheet.Cells[5, 8, 5, 9].Merge = true;
            // dong thu 2
            xlSheet.Cells[6, 4].Value = "Số khách";
            xlSheet.Cells[6, 5].Value = "Doanh thu";
            xlSheet.Cells[6, 6].Value = "Số khách";
            xlSheet.Cells[6, 7].Value = "Doanh Thu";
            xlSheet.Cells[6, 8].Value = "Số khách";
            xlSheet.Cells[6, 9].Value = "Doanh thu";
            setCenterAligment(5, 1, 6, 9, xlSheet);
            xlSheet.Cells[5, 1, 6, 9].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

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

                        xlSheet.Cells[dong, 8].Value = (Convert.ToInt32(xlSheet.Cells[dong, 4].Value) - Convert.ToInt32(xlSheet.Cells[dong, 6].Value));
                        xlSheet.Cells[dong, 9].Value = Convert.ToDecimal(xlSheet.Cells[dong, 5].Value) - Convert.ToDecimal(xlSheet.Cells[dong, 7].Value);
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
            xlSheet.Cells[dong, 4].Formula = "SUM(D6:D" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 5].Formula = "SUM(E6:E" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 6].Formula = "SUM(F6:F" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 8].Formula = "SUM(H6:H" + (7 + dt.Rows.Count - 1) + ")";
            xlSheet.Cells[dong, 9].Formula = "SUM(I6:I" + (7 + dt.Rows.Count - 1) + ")";
            // định dạng số
            NumberFormat(dong, 4, dong, 5, xlSheet);

            setBorder(5, 1, 5 + dt.Rows.Count + 2, 9, xlSheet);
            setFontBold(5, 1, 5, 5, 12, xlSheet);
            setFontSize(7, 1, 6 + dt.Rows.Count + 2, 9, 12, xlSheet);
            // dinh dang giu cho so khach
            setCenterAligment(7, 1, 7 + dt.Rows.Count, 2, xlSheet);
            setCenterAligment(7, 4, 7 + dt.Rows.Count, 4, xlSheet);
            setCenterAligment(7, 6, 7 + dt.Rows.Count, 6, xlSheet);
            setCenterAligment(7, 8, 7 + dt.Rows.Count, 8, xlSheet);
            // dinh dạng number cot sokhach, doanh so, thuc thu
            NumberFormat(7, 5, 7 + dt.Rows.Count + 1, 5, xlSheet);
            NumberFormat(7, 7, 6 + dt.Rows.Count + 1, 7, xlSheet);
            NumberFormat(7, 9, 6 + dt.Rows.Count + 1, 9, xlSheet);

            setBorder(dong, 4, dong, 9, xlSheet);
            setFontBold(dong, 4, dong, 9, 12, xlSheet);
            xlSheet.View.FreezePanes(7, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "LienketKhachle" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            return View();
        }


        //////////////////////////////////////////////////////////////////////////////////////////////
        [HttpGet]
        public JsonResult LoadDataSaleTheoQuay(string tungay, string denngay, string cn, string khoi, int page, int pageSize)
        {
            //cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            if (cn == null)
            {
                cn = "";
            }

            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;

            int totalRow = 0;

            var listAccount = _thongkeService.doanhthuSaleTheoQuayEntities(tungay, denngay, cn, khoi, page, pageSize, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<doanhthuSaleQuay>, IEnumerable<doanhthuSaleQuayViewModel>>(listAccount);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadDataSaleTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize)
        {
            int totalRow = 0;

            var listDoanhthu = _thongkeService.doanhthuSaleTheoNgayDiEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<doanhthuSaleQuay>, IEnumerable<doanhthuSaleQuayViewModel>>(listDoanhthu);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }


        public JsonResult LoadDataQuayTheoNgayBan(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize)
        {
            int totalRow = 0;

            var listDoanhthu = _thongkeService.doanhthuQuayTheoNgayBanEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<doanthuQuayNgayBan>, IEnumerable<doanthuQuayNgayBanViewModel>>(listDoanhthu);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadDataQuayTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize)
        {
            int totalRow = 0;

            var listDoanhthu = _thongkeService.doanhthuQuayTheoNgayDiEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<doanthuQuayNgayBan>, IEnumerable<doanthuQuayNgayBanViewModel>>(listDoanhthu);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadDataDoanTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize)
        {
            int totalRow = 0;

            var listDoanhthu = _thongkeService.doanhthuDoanTheoNgayDiEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<doanhthuDoanNgayDi>, IEnumerable<doanhthuDoanNgayDiViewModel>>(listDoanhthu);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult LoadDataTuyentqTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize)
        {
            int totalRow = 0;

            var listDoanhthu = _thongkeService.doanhthuTuyentqTheoNgayDiEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<tuyentqNgaydi>, IEnumerable<tuyentqNgaydiViewModel>>(listDoanhthu);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadDataKhachLeHethong(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize)
        {
            int totalRow = 0;

            var listDoanhthu = _thongkeService.doanhthuKhachLeHeThongEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            //var query = listuser.OrderBy(x => x.tenhd);
            var responseData = Mapper.Map<IEnumerable<doanhthuToanhethong>, IEnumerable<doanhthuToanhethongViewModel>>(listDoanhthu);

            return Json(new
            {
                data = responseData,
                total = totalRow,
                status = true
            }, JsonRequestBehavior.AllowGet);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////


        [HttpGet]
        public ActionResult LoadDataQuayTheoNgayDiChitietToExcel(string quay, string chinhanh, string tungay, string denngay, string khoi)
        {

            //cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 10;//STT
            xlSheet.Column(3).Width = 25;// SGTCODE
            xlSheet.Column(4).Width = 15;// serial
            xlSheet.Column(5).Width = 30;// ten khach
            xlSheet.Column(6).Width = 40;// tuyen tq
            xlSheet.Column(7).Width = 15;//  ngay di
            xlSheet.Column(8).Width = 15;//  ngay ve
            xlSheet.Column(9).Width = 15;//  gia tour
            xlSheet.Column(10).Width = 20;//  sale


            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ  QUẦY " + quay;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 10].Merge = true;
            setCenterAligment(2, 1, 2, 10, xlSheet);
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
            xlSheet.Cells[3, 1, 3, 10].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 10, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Code CN";
            xlSheet.Cells[5, 3].Value = "Sgt Code ";
            xlSheet.Cells[5, 4].Value = "Serial";
            xlSheet.Cells[5, 5].Value = "Tên khách";
            xlSheet.Cells[5, 6].Value = "Hành trình";
            xlSheet.Cells[5, 7].Value = "Ngày đi";
            xlSheet.Cells[5, 8].Value = "Ngày về";
            xlSheet.Cells[5, 9].Value = "Giá vé";
            xlSheet.Cells[5, 10].Value = "Nhân viên";
            xlSheet.Cells[5, 1, 5, 10].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;

            DataTable dt = _thongkeService.doanhthuQuayTheoNgayDiChitiet(quay, chinhanh, tungay, denngay, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

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
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return PartialView(dt);
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 8].Value = "TC";
            xlSheet.Cells[dong, 9].Formula = "SUM(I6:I" + (6 + dt.Rows.Count - 1) + ")";

            // định dạng số
            NumberFormat(dong, 8, dong, 8, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 11, 12, xlSheet);
            setBorder(5, 1, 5 + dt.Rows.Count, 10, xlSheet);
            setFontBold(5, 1, 5, 10, 12, xlSheet);
           
            // canh giưa cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 2, xlSheet);

            setBorder(dong, 8, dong, 9, xlSheet);
            setFontBold(dong, 8, dong, 9, 12, xlSheet);
            // canh giưa cot ngay di va ngày ve
            setCenterAligment(6, 7, 6 + dt.Rows.Count, 8, xlSheet);
            // dinh dạng number cot gia ve
            NumberFormat(6, 9, 6 + dt.Rows.Count, 9, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuayChitiet" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            //return RedirectToAction("LoadDataQuayTheoNgayDiChitietToExcel");
            return PartialView(dt);
        }

        public ActionResult LoadDataQuayTheoNgayBanChitietToExcel(string tungay, string denngay, string quay, string chinhanh, string khoi)
        {
            
            //cn = String.IsNullOrEmpty(cn) ? Session["chinhanh"].ToString() : cn;
            khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
            string fromTo = "";
            ExcelPackage ExcelApp = new ExcelPackage();
            ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("Report");
            // Định dạng chiều dài cho cột
            xlSheet.Column(1).Width = 10;//STT
            xlSheet.Column(2).Width = 10;//Code CN
            xlSheet.Column(3).Width = 25;// SGTCODE
            xlSheet.Column(4).Width = 15;// serial
            xlSheet.Column(5).Width = 30;// ten khach
            xlSheet.Column(6).Width = 40;// tuyen tq
            xlSheet.Column(7).Width = 15;//  ngay di
            xlSheet.Column(8).Width = 15;//  ngay ve
            xlSheet.Column(9).Width = 15;//  gia tour
            xlSheet.Column(10).Width = 20;//  sale


            xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU BÁN VÉ  QUẦY " + quay;
            xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
            xlSheet.Cells[2, 1, 2, 10].Merge = true;
            setCenterAligment(2, 1, 2, 10, xlSheet);
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
            xlSheet.Cells[3, 1, 3, 10].Merge = true;
            xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
            setCenterAligment(3, 1, 3, 10, xlSheet);

            // Tạo header
            xlSheet.Cells[5, 1].Value = "STT";
            xlSheet.Cells[5, 2].Value = "Code CN";
            xlSheet.Cells[5, 3].Value = "Sgt Code ";
            xlSheet.Cells[5, 4].Value = "Serial";
            xlSheet.Cells[5, 5].Value = "Tên khách";
            xlSheet.Cells[5, 6].Value = "Hành trình";
            xlSheet.Cells[5, 7].Value = "Ngày đi";
            xlSheet.Cells[5, 8].Value = "Ngày về";
            xlSheet.Cells[5, 9].Value = "Giá vé";
            xlSheet.Cells[5, 10].Value = "Nhân viên";
            xlSheet.Cells[5, 1, 5, 10].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

            // do du lieu tu table
            int dong = 5;

            //DataTable dt = _thongkeService.doanhthuQuayTheoNgayBanChitiet(quay, chinhanh, tungay, denngay, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());
            DataTable dt = _thongkeService.doanhthuQuayTheoNgayBanChitiet(tungay, denngay, quay, chinhanh, khoi);// Session["daily"].ToString(), Session["khoi"].ToString());

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
                    }
                }
            }
            else
            {
                SetAlert("No sale.", "warning");
                return PartialView(dt);
            }

            dong++;
            //// Merger cot 4,5 ghi tổng tiền
            //setRightAligment(dong, 3, dong, 3, xlSheet);
            //xlSheet.Cells[dong, 1, dong, 2].Merge = true;
            //xlSheet.Cells[dong, 1].Value = "Tổng tiền: ";
            // Sum tổng tiền
            xlSheet.Cells[dong, 8].Value = "TC";
            xlSheet.Cells[dong, 9].Formula = "SUM(I6:I" + (6 + dt.Rows.Count - 1) + ")";

            // định dạng số
            NumberFormat(dong, 8, dong, 8, xlSheet);
            setFontSize(6, 1, 6 + dt.Rows.Count, 11, 12, xlSheet);
            setBorder(5, 1, 5 + dt.Rows.Count, 10, xlSheet);
            setFontBold(5, 1, 5, 10, 12, xlSheet);

            // canh giưa cot stt
            setCenterAligment(6, 1, 6 + dt.Rows.Count, 2, xlSheet);

            setBorder(dong, 8, dong, 9, xlSheet);
            setFontBold(dong, 8, dong, 9, 12, xlSheet);
            // canh giưa cot ngay di va ngày ve
            setCenterAligment(6, 7, 6 + dt.Rows.Count, 8, xlSheet);
            // dinh dạng number cot gia ve
            NumberFormat(6, 9, 6 + dt.Rows.Count, 9, xlSheet);

            xlSheet.View.FreezePanes(6, 20);


            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuQuayChitiet" + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
            Response.BinaryWrite(ExcelApp.GetAsByteArray());
            Response.End();

            //return RedirectToAction("LoadDataQuayTheoNgayDiChitietToExcel");
            return PartialView(dt);
        }

        public ViewResult LoadDataSaleTheoNgayBanChitietToExcel(string tungay, string denngay, string nhanvien, string chinhanh, string khoi)
        {
            try
            {

                khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
                string fromTo = "";
                ExcelPackage ExcelApp = new ExcelPackage();
                ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("DoanhthuSale");
                // Định dạng chiều dài cho cột
                xlSheet.Column(1).Width = 10;//stt
                xlSheet.Column(2).Width = 10;// chi nhanh
                xlSheet.Column(3).Width = 25;// code
                xlSheet.Column(4).Width = 10;// vetourid
                xlSheet.Column(5).Width = 40;// ten khach
                xlSheet.Column(6).Width = 10;// so khach
                xlSheet.Column(7).Width = 20;//doanhthu
                xlSheet.Column(8).Width = 20;//thuc thu
                xlSheet.Column(9).Width = 35;//sales

                xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU SALES " + nhanvien;
                xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
                xlSheet.Cells[2, 1, 2, 9].Merge = true;
                if (tungay == denngay)
                {
                    fromTo = "Ngày: " + tungay;
                }
                else
                {
                    fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
                }
                xlSheet.Cells[3, 1].Value = fromTo;
                xlSheet.Cells[3, 1, 3, 9].Merge = true;
                xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
                setCenterAligment(2, 1, 3, 9, xlSheet);

                // Tạo header
                xlSheet.Cells[5, 1].Value = "STT";
                xlSheet.Cells[5, 2].Value = "Code CN";
                xlSheet.Cells[5, 3].Value = "Code Đoàn";
                xlSheet.Cells[5, 4].Value = "Vé tour";
                xlSheet.Cells[5, 5].Value = "Tên khách";
                xlSheet.Cells[5, 6].Value = "Số khách";
                xlSheet.Cells[5, 7].Value = "Doanh thu";
                xlSheet.Cells[5, 8].Value = "Thực thu";
                xlSheet.Cells[5, 9].Value = "Sales";

                xlSheet.Cells[5, 1, 5, 9].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

                int dong = 5;
                DataTable dt = _thongkeService.doanhthuSaleTheoNgayBanChitiet(tungay, denngay, nhanvien, chinhanh, khoi);// Session["fullName"].ToString());

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
                        }
                    }
                }
                else
                {
                    SetAlert("No sale.", "warning");
                    return View();
                }
                dong++;
                // Merger cot 4,5 ghi tổng tiền
                //setRightAligment(dong, 4, dong, 5, xlSheet);
                //xlSheet.Cells[dong, 4, dong, 5].Merge = true;
                //xlSheet.Cells[dong, 4].Value = "Tổng tiền: ";

                //// Sum tổng tiền
                xlSheet.Cells[dong, 8].Formula = "SUM(H6:H" + (6 + dt.Rows.Count - 1) + ")";
                //xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (6 + dt.Rows.Count - 1) + ")";
                //// định dạng số
                NumberFormat(dong, 6, dong, 6, xlSheet);
                setBorder(5, 1, 5 + dt.Rows.Count, 9, xlSheet);
                setFontBold(5, 1, 5, 9, 12, xlSheet);
                setFontSize(6, 1, 6 + dt.Rows.Count, 9, 12, xlSheet);
                NumberFormat(6, 7, 6 + dt.Rows.Count, 8, xlSheet);
                setCenterAligment(6, 1, 6 + dt.Rows.Count, 4, xlSheet);
                setCenterAligment(6, 6, 6 + dt.Rows.Count, 6, xlSheet);
                xlSheet.View.FreezePanes(6, 20);

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuSale_" + nhanvien + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
                Response.BinaryWrite(ExcelApp.GetAsByteArray());
                Response.End();

            }
            catch
            {
                throw;
            }
            return View();
        }

        public ViewResult LoadDataSaleTheoNgayDiChitietToExcel(string tungay, string denngay, string nhanvien, string chinhanh, string khoi)
        {
            try
            {
                khoi = String.IsNullOrEmpty(khoi) ? Session["khoi"].ToString() : khoi;
                string fromTo = "";
                ExcelPackage ExcelApp = new ExcelPackage();
                ExcelWorksheet xlSheet = ExcelApp.Workbook.Worksheets.Add("DoanhthuSale");
                // Định dạng chiều dài cho cột
                xlSheet.Column(1).Width = 10;//stt
                xlSheet.Column(2).Width = 10;// chi nhanh
                xlSheet.Column(3).Width = 25;// sgtcode
                xlSheet.Column(4).Width = 10;// vetourid
                xlSheet.Column(5).Width = 40;// ten khach
                xlSheet.Column(6).Width = 10;// so khach
                xlSheet.Column(7).Width = 20;//doanhthu
                xlSheet.Column(8).Width = 20;//thuc thu
                xlSheet.Column(9).Width = 35;//sales

                xlSheet.Cells[2, 1].Value = "BÁO CÁO DOANH THU SALES " + nhanvien;
                xlSheet.Cells[2, 1].Style.Font.SetFromFont(new Font("Times New Roman", 16, FontStyle.Bold));
                xlSheet.Cells[2, 1, 2, 8].Merge = true;
                if (tungay == denngay)
                {
                    fromTo = "Ngày: " + tungay;
                }
                else
                {
                    fromTo = "Từ ngày: " + tungay + " đến ngày: " + denngay;
                }
                xlSheet.Cells[3, 1].Value = fromTo;
                xlSheet.Cells[3, 1, 3, 9].Merge = true;
                xlSheet.Cells[3, 1].Style.Font.SetFromFont(new Font("Times New Roman", 14, FontStyle.Bold));
                setCenterAligment(2, 1, 3, 9, xlSheet);

                // Tạo header
                xlSheet.Cells[5, 1].Value = "STT";
                xlSheet.Cells[5, 2].Value = "Code CN";
                xlSheet.Cells[5, 3].Value = "Code Đoàn";
                xlSheet.Cells[5, 4].Value = "Vé tour";
                xlSheet.Cells[5, 5].Value = "Tên khách";
                xlSheet.Cells[5, 6].Value = "Số khách";
                xlSheet.Cells[5, 7].Value = "Doanh thu";
                xlSheet.Cells[5, 8].Value = "Thực thu";
                xlSheet.Cells[5, 9].Value = "Sales";

                xlSheet.Cells[5, 1, 5, 9].Style.Font.SetFromFont(new Font("Times New Roman", 12, FontStyle.Bold));

                int dong = 5;
                DataTable dt = _thongkeService.doanhthuSaleTheoNgayDiChitiet(tungay, denngay, nhanvien, chinhanh, khoi);// Session["fullName"].ToString());

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
                        }
                    }
                }
                else
                {
                    SetAlert("No sale.", "warning");
                    return View();
                }
                dong++;
                // Merger cot 4,5 ghi tổng tiền
                //setRightAligment(dong, 4, dong, 5, xlSheet);
                //xlSheet.Cells[dong, 4, dong, 5].Merge = true;
                //xlSheet.Cells[dong, 4].Value = "Tổng tiền: ";

                //// Sum tổng tiền
                xlSheet.Cells[dong, 8].Formula = "SUM(H6:H" + (6 + dt.Rows.Count - 1) + ")";
                //xlSheet.Cells[dong, 7].Formula = "SUM(G6:G" + (6 + dt.Rows.Count - 1) + ")";
                //// định dạng số
                NumberFormat(dong, 7, dong, 8, xlSheet);
                setBorder(5, 1, 5 + dt.Rows.Count, 9, xlSheet);
                setFontBold(5, 1, 5, 9, 12, xlSheet);
                setFontSize(6, 1, 6 + dt.Rows.Count, 9, 12, xlSheet);
                NumberFormat(6, 7, 6 + dt.Rows.Count, 8, xlSheet);
                setCenterAligment(6, 1, 6 + dt.Rows.Count, 3, xlSheet);
                setCenterAligment(6, 6, 6 + dt.Rows.Count, 6, xlSheet);
                xlSheet.View.FreezePanes(6, 20);

                Response.Clear();
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + "DoanhThuSale_" + nhanvien + "_" + System.DateTime.Now.ToString("dd/MM/yyyy HH:mm") + ".xlsx");
                Response.BinaryWrite(ExcelApp.GetAsByteArray());
                Response.End();

            }
            catch
            {
                throw;
            }
            return View();
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////


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