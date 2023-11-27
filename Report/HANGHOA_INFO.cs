using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petStore.Report
{
    public class HANGHOA_INFO
    {
        public HANGHOA_INFO(){ }
        private string mahh;
        private string tenhh;
        private LOAIHANGHOA_INFO loaihanghoa;
        private int soluong;
        private double dongianhap;
        private double dongiaban;
        private NHACUNGCAP_INFO nhacungcap;
        private string mota;

        public string MAHH { get => mahh; set => mahh = value; }
        public string TENHH { get => tenhh; set => tenhh = value; }
        public LOAIHANGHOA_INFO LOAIHANGHOA { get => loaihanghoa; set => loaihanghoa = value; }
        public int SOLUONG { get => soluong; set => soluong = value; }
        public double DONGIANHAP { get => dongianhap; set => dongianhap = value; }
        public double DONGIABAN { get => dongiaban; set => dongiaban = value; }
        public NHACUNGCAP_INFO NHACUNGCAP { get => nhacungcap; set => nhacungcap = value; }
        public string MOTA { get => mota; set => mota = value; }
    }
}
