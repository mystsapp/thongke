﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using ThongKe.Common;
using ThongKe.Common.ViewModel;
using ThongKe.Data.Infrastructure;
using ThongKe.Data.Models.EF;

namespace ThongKe.Data.Repositories
{
    public interface IthongkeRepository : IRepository<doanthuQuayNgayBan>
    {
        DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string cn, string khoi);

        IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string chinhanh, string khoi);

        IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoNgayDiEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuQuayTheoNgayBan(string tungay, string denngay, string chinhanh, string khoi);

        IEnumerable<doanthuQuayNgayBan> doanhthuQuayTheoNgayBanEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuQuayTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi);

        IEnumerable<doanthuQuayNgayBan> doanhthuQuayTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuDoanTheoNgay(string tungay, string denngay, string chinhanh, string khoi);

        IEnumerable<doanhthuDoanNgayDi> doanhthuDoanTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuTuyentqTheoNgay(string tungay, string denngay, string chinhanh, string khoi);

        IEnumerable<tuyentqNgaydi> doanhthuTuyentqTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuKhachleHethong(string tungay, string denngay, string chinhanh, string khoi);

        IEnumerable<doanhthuToanhethong> doanhthuKhachLeHeThongEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow);

        IEnumerable<ThongKeKhachViewModel> ThongKeSoKhachOB(string khoi);

        IEnumerable<ThongKeDoanhThuViewModel> ThongKeDoanhThuOB(string khoi);

        DataTable doanhthuQuayTheoNgayDiChitiet(string quay,string tungay, string denngay, string khoi);

        DataTable doanhthuQuayTheoNgayBanChitiet(string tungay, string denngay, string quay, string khoi);

        DataTable doanhthuSaleTheoNgayBanChitiet(string tungay, string denngay, string nhanvien, string khoi);

        DataTable doanhthuSaleTheoNgayDiChitiet(string tungay, string denngay, string nhanvien, string khoi);
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

        public DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string cn, string khoi)
        {
            try
            {
                TKDbContext db = new TKDbContext();
                DataTable dt = new DataTable();
                //DateTime tn = Convert.ToDateTime("2018 - 11 - 01");
                //DateTime dn = Convert.ToDateTime("2018-11-10");

                var result = DbContext.spBaocaoDoanhThuSaleTheoQuay(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), daily, cn, khoi).ToList();
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

        public IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow)
        {
            // pageSize = 10;
            try
            {
                //DateTime tn = Convert.ToDateTime("2018 - 11 - 01");
                //DateTime dn = Convert.ToDateTime("2018-11-10");

                var result = DbContext.spBaocaoDoanhThuSaleTheoQuay(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), daily, cn, khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.stt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                // dt = EntityToTable.ToDataTable(result);
                if (result.Count > 0)
                    return result;
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

        public DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spBaocaoDoanhThuSaleTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), daily, chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoNgayDiEntities(string tungay, string denngay, string daily, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            try
            {
                var result = DbContext.spBaocaoDoanhThuSaleTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), daily, chinhanh, khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.stt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                // dt = EntityToTable.ToDataTable(result);
                if (result.Count > 0)
                    return result;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<doanthuQuayNgayBan> doanhthuQuayTheoNgayBanEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            try
            {
                var result = DbContext.spBaocaoDoanhThuQuayTheoNgayBan(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.stt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                // dt = EntityToTable.ToDataTable(result);
                if (result.Count > 0)
                    return result;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<doanthuQuayNgayBan> doanhthuQuayTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            try
            {
                var result = DbContext.spBaocaoDoanhThuQuayTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.stt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                // dt = EntityToTable.ToDataTable(result);
                if (result.Count > 0)
                    return result;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<doanhthuDoanNgayDi> doanhthuDoanTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            try
            {
                var result = DbContext.spBaocaoDoanhThuDoanTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.stt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                // dt = EntityToTable.ToDataTable(result);
                if (result.Count > 0)
                    return result;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<tuyentqNgaydi> doanhthuTuyentqTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            try
            {
                var result = DbContext.spThongkeTuyentqTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.stt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                // dt = EntityToTable.ToDataTable(result);
                if (result.Count > 0)
                    return result;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<doanhthuToanhethong> doanhthuKhachLeHeThongEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            try
            {
                var result = DbContext.spThongkeKhachToanHeThong(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.stt).Skip((page - 1) * pageSize).Take(pageSize).ToList();
                // dt = EntityToTable.ToDataTable(result);
                if (result.Count > 0)
                    return result;
                return null;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<ThongKeKhachViewModel> ThongKeSoKhachOB(string khoi)
        {
            var parammeter = new SqlParameter[]
            {
                new SqlParameter("@khoi",khoi)
            };
            var result = DbContext.Database.SqlQuery<ThongKeKhachViewModel>("spThongkeKhach @khoi", parammeter);
            return result;
            //try
            //{
            //    var parammeter = new SqlParameter[]
            //{
            //    new SqlParameter("@khoi",khoi)
            //};
            //    IEnumerable<ThongKeKhachViewModel> result = DbContext.Database.SqlQuery<ThongKeKhachViewModel>("spThongkeKhach @khoi", parammeter);

            //    // dt = EntityToTable.ToDataTable(result);
            //    if (result.Count() > 0)
            //        return result;
            //    return null;
            //}
            //catch
            //{
            //    throw;
            //}
        }

        public IEnumerable<ThongKeDoanhThuViewModel> ThongKeDoanhThuOB(string khoi)
        {
            var parammeter = new SqlParameter[]
            {
                new SqlParameter("@khoi",khoi)
            };
            var result = DbContext.Database.SqlQuery<ThongKeDoanhThuViewModel>("spThongKeDoanhthu @khoi", parammeter);
            return result;
        }

        public DataTable doanhthuQuayTheoNgayDiChitiet(string quay, string tungay, string denngay, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhSoQuayChitietNgaydi(quay, Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuQuayTheoNgayBanChitiet(string tungay, string denngay, string quay, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhSoQuayChitietNgayBan(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), quay, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuSaleTheoNgayBanChitiet(string tungay, string denngay, string nhanvien, string khoi)
        {
            try
            {
                DataTable dt = new DataTable(); 
                 var result = DbContext.spDoanhThuSaleChitietNgayban(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), nhanvien, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuSaleTheoNgayDiChitiet(string tungay, string denngay, string nhanvien, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhThuSaleChitietNgaydi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), nhanvien, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }
    }
}