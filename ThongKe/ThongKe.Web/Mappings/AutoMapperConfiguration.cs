using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ThongKe.Data.Models.EF;
using ThongKe.Web.Models;

namespace ThongKe.Web.Mappings
{
    public class AutoMapperConfiguration
    {
        public static void configure()
        {
            Mapper.CreateMap<account, accountViewModel>();
            Mapper.CreateMap<chinhanh, chinhanhViewModel>();
            Mapper.CreateMap<dmdaily, dmdailyViewModel>();
        }
    }
}