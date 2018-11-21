using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThongKe.Data.Models;

namespace ThongKe.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
    {
        private ThongKeDbContext dbContext;

        public ThongKeDbContext Init()
        {
            return dbContext ?? (dbContext = new ThongKeDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}
