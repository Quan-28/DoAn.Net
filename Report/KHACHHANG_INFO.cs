using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petStore.Report
{
    public class KHACHHANG_INFO
    {
        KHACHHANG_INFO() { }
        private string makh;
        private string tenkh;
        private bool gioitinh;
        private string sdt;
        private string diachi;

        public string MAKH { get => makh; set => makh = value; }
        public string TENKH { get => tenkh; set => tenkh = value; }
        public bool GIOITINH { get => gioitinh; set => gioitinh = value; }
        public string SDT { get => sdt; set => sdt = value; }
        public string DIACHI { get => diachi; set => diachi = value; }
    }
}
