using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petStore.Report
{
    public class NHANVIEN_INFO
    {
        NHANVIEN_INFO() { }
        private string manv;
        private string tennv;
        private string cccd;
        private bool gioitinh;
        private DateTime ngaysinh;
        private string sdt;

        public string MANV { get => manv; set => manv = value; }
        public string TENNV { get => tennv; set => tennv = value; }
        public string CCCD { get => cccd; set => cccd = value; }
        public bool GIOITINH { get => gioitinh; set => gioitinh = value; }
        public DateTime NGAYSINH { get => ngaysinh; set => ngaysinh = value; }
        public string SDT { get => sdt; set => sdt = value; }
    }
}
