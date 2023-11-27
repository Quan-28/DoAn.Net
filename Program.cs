using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace petStore
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new fDangNhap());
            //Application.Run(new fQuanLyChinh());
            //Application.Run(new FormChuongTrinh.fTaiKhoanVaNhanVien());
            //Application.Run(new FormChuongTrinh.fKhachHang());
            //Application.Run(new FormChuongTrinh.fNhaCungCap());
            //Application.Run(new FormChuongTrinh.fLoaiHangHoa());
            //Application.Run(new FormChuongTrinh.fHangHoa());
            //Application.Run(new FormChuongTrinh.fShowLoaiHH());
            //Application.Run(new FormChuongTrinh.fShowNhaCungCap());
            //Application.Run(new FormChuongTrinh.fShowNhanVien());
            //Application.Run(new FormChuongTrinh.fHoaDonBan());
        }
    }
}
