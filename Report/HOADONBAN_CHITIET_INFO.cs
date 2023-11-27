using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace petStore.Report
{
    public class HOADONBAN_CHITIET_INFO
    {
        public HOADONBAN_CHITIET_INFO() { }
        private HOADONBAN_INFO hoadonban;
        private HANGHOA_INFO hanghoa = new HANGHOA_INFO();
        
        private int soluong;
        private double dongiaban;
        private double thanhtien;

        public HOADONBAN_INFO Hoadonban { get => hoadonban; set => hoadonban = value; }
        public HANGHOA_INFO Hanghoa { get => hanghoa; set => hanghoa = value; }
        public int SOLUONG { get => soluong; set => soluong = value; }
        public double DONGIABAN { get => dongiaban; set => dongiaban = value; }
        public double THANHTIEN { get => thanhtien; set => thanhtien = value; }
    }
}
