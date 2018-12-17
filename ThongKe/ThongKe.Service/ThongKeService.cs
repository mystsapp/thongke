using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThongKe.Data.Infrastructure;
using ThongKe.Data.Models.EF;
using ThongKe.Data.Repositories;

namespace ThongKe.Service
{
    public interface IThongKeService
    {
        DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string cn, string khoi);

        IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string chinhanh, string khoi);

        IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoNgayDiEntities(string tungay, string denngay, string daily, string chinhanh, string khoi, int page, int pageSize, out int totalRow);

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

        //IEnumerable<doanhthuToanhethong> doanhthuKhachLeHeThongEntities(string tungay, string denngay, string chinhanh, string khoi);

    }
    public class ThongKeService : IThongKeService
    {
        private IthongkeRepository _thongkeRepository;
        private IUnitOfWork _unitOfWork;

        public ThongKeService(IthongkeRepository thongkeRepository, IUnitOfWork unitOfWork)
        {
            _thongkeRepository = thongkeRepository;
            _unitOfWork = unitOfWork;
        }

        public DataTable doanhthuDoanTheoNgay(string tungay, string denngay, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuDoanTheoNgay(tungay, denngay, chinhanh, khoi);
            return result;
        }

        public DataTable doanhthuKhachleHethong(string tungay, string denngay, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuKhachleHethong(tungay, denngay, chinhanh, khoi);
            return result;
        }

        public DataTable doanhthuQuayTheoNgayBan(string tungay, string denngay, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuQuayTheoNgayBan(tungay, denngay, chinhanh, khoi);
            return result;
        }

        public DataTable doanhthuQuayTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuQuayTheoNgayDi(tungay, denngay, chinhanh, khoi);
            return result;
        }

        public IEnumerable<doanthuQuayNgayBan> doanhthuQuayTheoNgayBanEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            var listDanhThu = _thongkeRepository.doanhthuQuayTheoNgayBanEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            return listDanhThu;
        }

        public DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuSaleTheoNgayDi(tungay, denngay, daily, chinhanh, khoi);
            return result;
        }

        public IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoNgayDiEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow)
        {
            var listDanhThu = _thongkeRepository.doanhthuSaleTheoNgayDiEntities(tungay, denngay, daily, cn, khoi, page, pageSize, out totalRow);
            return listDanhThu;
        }

        public DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string cn, string khoi)
        {
            var result = _thongkeRepository.doanhthuSaleTheoQuay(tungay, denngay, daily, cn, khoi);
            return result;
        }

        public IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow)
        {
            var listDanhThu = _thongkeRepository.doanhthuSaleTheoQuayEntities(tungay, denngay, daily, cn, khoi, page, pageSize, out totalRow);
            return listDanhThu;
        }

        public DataTable doanhthuTuyentqTheoNgay(string tungay, string denngay, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuTuyentqTheoNgay(tungay, denngay, chinhanh, khoi);
            return result;
        }

        public IEnumerable<doanthuQuayNgayBan> doanhthuQuayTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            var listDanhThu = _thongkeRepository.doanhthuQuayTheoNgayDiEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            return listDanhThu;
        }

        public IEnumerable<doanhthuDoanNgayDi> doanhthuDoanTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            var listDanhThu = _thongkeRepository.doanhthuDoanTheoNgayDiEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            return listDanhThu;
        }

        public IEnumerable<tuyentqNgaydi> doanhthuTuyentqTheoNgayDiEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            var listDanhThu = _thongkeRepository.doanhthuTuyentqTheoNgayDiEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            return listDanhThu;
        }

        public IEnumerable<doanhthuToanhethong> doanhthuKhachLeHeThongEntities(string tungay, string denngay, string chinhanh, string khoi, int page, int pageSize, out int totalRow)
        {
            var listDanhThu = _thongkeRepository.doanhthuKhachLeHeThongEntities(tungay, denngay, chinhanh, khoi, page, pageSize, out totalRow);
            return listDanhThu;
        }

        //public IEnumerable<doanhthuToanhethong> doanhthuKhachLeHeThongEntities(string tungay, string denngay, string chinhanh, string khoi)
        //{
        //    var listDanhThu = _thongkeRepository.doanhthuKhachLeHeThongEntities(tungay, denngay, chinhanh, khoi);
        //    return listDanhThu;
        //}
    }
}
