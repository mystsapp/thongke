using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThongKe.Data.Models;

namespace ThongKe.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        ThongKeDbContext Init();
    }
}
