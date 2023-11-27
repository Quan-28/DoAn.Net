using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petStore.Report
{
    public class NHACUNGCAP_INFO
    {
        NHACUNGCAP_INFO() { }
        private string mancc;
        private string tenncc;
        private string sdt;
        private string diachi;

        public string MANCC { get => mancc; set => mancc = value; }
        public string TENNCC { get => tenncc; set => tenncc = value; }
        public string SDT { get => sdt; set => sdt = value; }
        public string DIACHI { get => diachi; set => diachi = value; }
    }
}
