using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThongKe.Data.Infrastructure;
using ThongKe.Data.Models.EF;
using ThongKe.Data.Repositories;

namespace ThongKe.Service
{
    public interface ICommonService
    {
        IEnumerable<string> GetAllChiNhanh();
        IEnumerable<dmdaily> GetDmdailyByChiNhanh(string chinhanh);
        IEnumerable<dmdaily> GetAllDmDaiLy();
    }
    public class CommonService : ICommonService
    {
        private IchinhanhRepository _chinhanhRepository;
        private IdmdailyRepository _dmdailyRepository;
        private IUnitOfWork _unitOfWork;

        public CommonService(IchinhanhRepository chinhanhRepository, IdmdailyRepository dmdailyRepository, IUnitOfWork unitOfWork)
        {
            _chinhanhRepository = chinhanhRepository;
            _dmdailyRepository = dmdailyRepository;
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<string> GetAllChiNhanh()
        {
            var result=_chinhanhRepository.GetAll().Select(x=>x.chinhanh1).Distinct();
            var count=result.Count();
            return result;
        }

        public IEnumerable<dmdaily> GetAllDmDaiLy()
        {
            return _dmdailyRepository.GetAll();
        }

        public IEnumerable<dmdaily> GetDmdailyByChiNhanh(string chinhanh)
        {
            var listDaily = new List<dmdaily>();
            if (chinhanh != "")
                listDaily = _dmdailyRepository.GetMulti(x => x.chinhanh == chinhanh && x.trangthai).ToList();
            else
                listDaily = _dmdailyRepository.GetAll().ToList();
            return listDaily;
        }
    }
}
