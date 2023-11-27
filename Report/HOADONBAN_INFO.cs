using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petStore.Report
{
    public class HOADONBAN_INFO
    {
        HOADONBAN_INFO(){}
        private string mahdban;
        private NHANVIEN_INFO nhanvien;
        private KHACHHANG_INFO khachhang;
        private DateTime ngaylap;
        private double tongtien;

        public string MAHDBAN { get => mahdban; set => mahdban = value; }
        public NHANVIEN_INFO Nhanvien { get => nhanvien; set => nhanvien = value; }
        public KHACHHANG_INFO Khachhag { get => khachhang; set => khachhang = value; }
        public DateTime NGAYLAP { get => ngaylap; set => ngaylap = value; }
        public double TONGTIEN { get => tongtien; set => tongtien = value; }
    }
}
