using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petStore.Report
{
    public class LOAIHANGHOA_INFO
    {
        LOAIHANGHOA_INFO() { }
        private string maloai;
        private string tenloai;

        public string MALOAI { get => maloai; set => maloai = value; }
        public string TENLOAI { get => tenloai; set => tenloai = value; }
    }
}
