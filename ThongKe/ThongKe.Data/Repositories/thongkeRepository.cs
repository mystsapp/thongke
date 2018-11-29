﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThongKe.Data.Infrastructure;
using ThongKe.Data.Models.EF;
using System.Data;
using ThongKe.Common;
namespace ThongKe.Data.Repositories
{
    public interface IthongkeRepository : IRepository<doanthuQuayNgayBan>
    {
        DataTable doanhthuQuayTheoNgayBan(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string khoi);

        DataTable doanhthuDoanTheoNgay(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuTuyentqTheoNgay(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuQuayTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuKhachleHethong(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string khoi);
    }
    public class thongkeRepository : RepositoryBase<doanthuQuayNgayBan>, IthongkeRepository
    {
        public thongkeRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }

        public DataTable doanhthuDoanTheoNgay(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spBaocaoDoanhThuDoanTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable doanhthuQuayTheoNgayBan(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spBaocaoDoanhThuQuayTheoNgayBan(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string khoi)
        {
            try
            {
                TKDbContext db = new TKDbContext();
                DataTable dt = new DataTable();
                //DateTime tn = Convert.ToDateTime("2018 - 11 - 01");
                //DateTime dn = Convert.ToDateTime("2018-11-10");

                var result = DbContext.spBaocaoDoanhThuSaleTheoQuay(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), daily, khoi).ToList();
                var count = result.Count();
                //var result = db.spBaocaoDoanhThuSaleTheoQuay(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), daily, khoi);
                //var count = result.Count();
                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuTuyentqTheoNgay(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spThongkeTuyentqTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable doanhthuQuayTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spBaocaoDoanhThuQuayTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable doanhthuKhachleHethong(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spThongkeKhachToanHeThong(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                if (dt.Rows.Count > 0)
                    return dt;
                return null;
            }
            catch
            {
                return null;
            }
        }

        public DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spBaocaoDoanhThuSaleTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), daily, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                return null;
            }
        }
    }


}
