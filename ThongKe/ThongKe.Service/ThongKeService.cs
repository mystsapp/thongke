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
        DataTable doanhthuQuayTheoNgayBan(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string cn, string khoi);

        IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow);

        DataTable doanhthuDoanTheoNgay(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuTuyentqTheoNgay(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuQuayTheoNgayDi(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuKhachleHethong(string tungay, string denngay, string chinhanh, string khoi);

        DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string chinhanh, string khoi);
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

        public DataTable doanhthuSaleTheoNgayDi(string tungay, string denngay, string daily, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuSaleTheoNgayDi(tungay, denngay, daily, chinhanh, khoi);
            return result;
        }

        public DataTable doanhthuSaleTheoQuay(string tungay, string denngay, string daily, string cn, string khoi)
        {
            var result = _thongkeRepository.doanhthuSaleTheoQuay(tungay, denngay, daily, cn, khoi);
            return result;
        }

        public IEnumerable<doanhthuSaleQuay> doanhthuSaleTheoQuayEntities(string tungay, string denngay, string daily, string cn, string khoi, int page, int pageSize, out int totalRow)
        {
            return _thongkeRepository.doanhthuSaleTheoQuayEntities(tungay, denngay, daily, cn, khoi, page, page, out totalRow);
        }

        public DataTable doanhthuTuyentqTheoNgay(string tungay, string denngay, string chinhanh, string khoi)
        {
            var result = _thongkeRepository.doanhthuTuyentqTheoNgay(tungay, denngay, chinhanh, khoi);
            return result;
        }

        //public IEnumerable<doanhthuSaleQuay> Search(int page, int pageSize, out int totalRow)
        //{
        //    var query = _thongkeRepository.GetAll();
        //    if (!string.IsNullOrEmpty(keyword))
        //    {
        //        query = _accountRepository.GetMulti(x => x.hoten.Contains(keyword) || x.username.Contains(keyword));
        //    }

        //    if (!string.IsNullOrEmpty(status))
        //    {
        //        var statusBool = bool.Parse(status);
        //        query = query.Where(x => x.trangthai == statusBool);
        //    }

        //    totalRow = query.Count();
        //    query = query.OrderByDescending(x => x.ngaytao).Skip((page - 1) * pageSize).Take(pageSize);

        //    return query;
        //}
    }
}
