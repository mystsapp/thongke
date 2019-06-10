using System;
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
        DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string cn, string khoi);

        IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string cn, string khoi, int page, int pageSize, out int totalRow);
        IEnumerable<thongkeweb> KinhDoanhOnlineEntities(string tungay, string denngay, string khoi, int page, int pageSize, out int totalRow);
        IEnumerable<thongkeweb> KinhDoanhOnlineEntities_ngaydi(string tungay, string denngay, string khoi, int page, int pageSize, out int totalRow);
        DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi);

        IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoNgayDiEntities(string tungay, string denngay, string cn, string khoi, int page, int pageSize, out int totalRow);

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

        DataTable doanhthuQuayTheoNgayDiChitiet(string quay,string chinhanh, string tungay, string denngay, string khoi);

        DataTable doanhthuQuayTheoNgayBanChitiet(string tungay, string denngay, string quay, string chinhanh, string khoi);

        DataTable doanhthuSaleTheoNgayBanChitiet(string tungay, string denngay, string nhanvien, string chinhanh, string khoi);

        DataTable doanhthuSaleTheoNgayDiChitiet(string tungay, string denngay, string nhanvien, string chinhanh, string khoi);
        DataTable ThongkeWebchitiet(string tungay, string denngay, string chinhanh, string khoi);
        DataTable doanhthuDoanTheoNgayDiChitiet(string sgtcode, string khoi);
        DataTable getTourbySgtcode(string sgtcode, string khoi);
        DataTable ThongkeWebchitiet_ngaydi(string tungay, string denngay, string chinhanh, string khoi);
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

        public DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string cn, string khoi)
        {
            try
            {
                TKDbContext db = new TKDbContext();
                DataTable dt = new DataTable();
                //DateTime tn = Convert.ToDateTime("2018 - 11 - 01");
                //DateTime dn = Convert.ToDateTime("2018-11-10");

                var result = DbContext.spBaocaoDoanhThuSaleTheoQuay(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), cn, khoi).ToList();
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

        public IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string cn, string khoi, int page, int pageSize, out int totalRow)
        {
            // pageSize = 10;
            try
            {
                //DateTime tn = Convert.ToDateTime("2018 - 11 - 01");
                //DateTime dn = Convert.ToDateTime("2018-11-10");

                var result = DbContext.spBaocaoDoanhThuSaleTheoQuay(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), cn, khoi).ToList();
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

        public DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spBaocaoDoanhThuSaleTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            try
            {
                var result = DbContext.spBaocaoDoanhThuSaleTheoNgayDi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
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
        public DataTable getTourbySgtcode(string sgtcode,string khoi)
        {
            try
            {

                //chitiettour a = DbContext.spGetTourByCode(sgtcode, khoi).ToList();
                //return a;
                DataTable dt = new DataTable();
                var result = DbContext.spGetTourByCode(sgtcode, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
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

        public DataTable doanhthuQuayTheoNgayDiChitiet(string quay, string chinhanh, string tungay, string denngay, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhSoQuayChitietNgaydi(quay, chinhanh, Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuQuayTheoNgayBanChitiet(string tungay, string denngay, string quay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhSoQuayChitietNgayBan(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), quay, chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuSaleTheoNgayBanChitiet(string tungay, string denngay, string nhanvien, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhThuSaleChitietNgayban(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), nhanvien, chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public DataTable doanhthuSaleTheoNgayDiChitiet(string tungay, string denngay, string nhanvien, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhThuSaleChitietNgaydi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), nhanvien, chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }

        public IEnumerable<thongkeweb> KinhDoanhOnlineEntities(string tungay, string denngay, string khoi, int page, int pageSize, out int totalRow)
        {
            // pageSize = 10;
            try
            {
                //DateTime tn = Convert.ToDateTime("2018 - 11 - 01");
                //DateTime dn = Convert.ToDateTime("2018-11-10");

                var result = DbContext.spThongkeWeb(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.chinhanh).Skip((page - 1) * pageSize).Take(pageSize).ToList();
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

        public DataTable ThongkeWebchitiet(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spThongkeWebchitiet(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }
        public DataTable doanhthuDoanTheoNgayDiChitiet(string sgtcode, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spDoanhthuDoanChitiet(sgtcode, khoi).ToList();
                var count = result.Count();

                dt = EntityToTable.ToDataTable(result);
                return dt;
            }
            catch
            {
                throw;
            }
        }
        public IEnumerable<thongkeweb> KinhDoanhOnlineEntities_ngaydi(string tungay, string denngay, string khoi, int page, int pageSize, out int totalRow)
        {
            // pageSize = 10;
            try
            {
                //DateTime tn = Convert.ToDateTime("2018 - 11 - 01");
                //DateTime dn = Convert.ToDateTime("2018-11-10");

                var result = DbContext.spThongkeWeb_ngaydi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), khoi).ToList();
                totalRow = result.Count();

                result = result.OrderBy(x => x.chinhanh).Skip((page - 1) * pageSize).Take(pageSize).ToList();
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
        public DataTable ThongkeWebchitiet_ngaydi(string tungay, string denngay, string chinhanh, string khoi)
        {
            try
            {
                DataTable dt = new DataTable();
                var result = DbContext.spThongkeWebchitiet_ngaydi(Convert.ToDateTime(tungay), Convert.ToDateTime(denngay), chinhanh, khoi).ToList();
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