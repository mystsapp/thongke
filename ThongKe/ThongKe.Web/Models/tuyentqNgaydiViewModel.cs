using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ThongKe.Web.Models
{
    public class tuyentqNgaydiViewModel
    {
        public long stt { get; set; }
        public string chinhanh { get; set; }
        public string tuyentq { get; set; }
        public Nullable<int> skkl { get; set; }
        public Nullable<decimal> dtkl { get; set; }
        public Nullable<int> skkd { get; set; }
        public Nullable<decimal> dtkd { get; set; }
        public Nullable<int> tongkhach { get; set; }
        public Nullable<decimal> tongdoanhthu { get; set; }
    }
}