using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace petStore.Report
{
    public partial class fInHoaDonBan : Form
    {
        string maHD;
        public fInHoaDonBan(string mahd)
        {
            maHD = mahd;
            InitializeComponent();
        }

        private void fInHoaDonBan_Load(object sender, EventArgs e)
        {
            ConnectData dataTable = new ConnectData();
            dataTable.OpenConnection();
            string sql = @"select hdb.MAHDBAN, kh.TENKH, kh.DIACHI, kh.SDT, nv.TENNV, hdb.NGAYLAP, hdb.THANHTIEN
                           from((HOADONBAN as hdb
                           inner join KHACHHANG as kh on hdb.MAKH = kh.MAKH)
                           inner join NHANVIEN as nv on hdb.MANV = nv.MANV)
                           where hdb.MAHDBAN = N'" + maHD + "'";
            SqlCommand cmd = new SqlCommand(sql);
            dataTable.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = dataTable;
            string MaHD = dataTable.Rows[0]["MAHDBAN"].ToString();
            string tenkh = dataTable.Rows[0]["TENKH"].ToString();
            string diachi = dataTable.Rows[0]["DIACHI"].ToString();
            string sdt = dataTable.Rows[0]["SDT"].ToString();
            string tennv = dataTable.Rows[0]["TENNV"].ToString();
            string ngay = dataTable.Rows[0]["NGAYLAP"].ToString();
            string tong = dataTable.Rows[0]["THANHTIEN"].ToString();
            ReportParameter[] p = new ReportParameter[]
            {
                new ReportParameter("pMaHD", MaHD),
                new ReportParameter("pKH", tenkh),
                new ReportParameter("pDiaChi", diachi),
                new ReportParameter("pSDT", sdt),
                new ReportParameter("pNV", tennv),
                new ReportParameter("pDate", ngay),
                new ReportParameter("pThanhTien", tong)
            };
            reportViewer1.LocalReport.SetParameters(p);
            
            IList<HOADONBAN_CHITIET_INFO> list = LayDuLieu();
            HOADONBAN_CHITIET_INFOBindingSource.DataSource = list;

            ReportDataSource reportDataSource = new ReportDataSource();
            reportDataSource.Name = "HOADONBAN_CHITIET_INFO";
            reportDataSource.Value = HOADONBAN_CHITIET_INFOBindingSource;
            reportViewer1.LocalReport.DataSources.Add(reportDataSource);
            reportViewer1.LocalReport.ReportEmbeddedResource = "petStore.Report.rpHoaDon.rdlc";

            reportViewer1.RefreshReport();
        }
        public IList<HOADONBAN_CHITIET_INFO> LayDuLieu()
        {
            ConnectData dataTable = new ConnectData();
            dataTable.OpenConnection();
            /*hh.MAHH, hh.TENHH, hdbct.MaHH, hdbct.MaHDban, hdbct.SoLuong, hdbct.DGban, hdbct.ThanhTien */
            SqlCommand cmd = new SqlCommand(@"SELECT hh.MAHH, hh.TENHH, hdbct.MaHH, hdbct.MaHDban, hdbct.SoLuong, hdbct.DGban, hdbct.ThanhTien
                                              FROM (HOADONBAN_CHITIET as hdbct
                                              inner join HANGHOA as hh on hh.MAHH = hdbct.MaHH)
                                              WHERE hdbct.MAHDBAN = N'" + maHD + "'");
            dataTable.Fill(cmd);
            IList<HOADONBAN_CHITIET_INFO> list = new List<HOADONBAN_CHITIET_INFO>();
            foreach (DataRow row in dataTable.Rows)
            {
                HANGHOA_INFO hh = new HANGHOA_INFO();
                hh.MAHH = row["MAHH"].ToString();
                hh.TENHH = row["TENHH"].ToString();

                HOADONBAN_CHITIET_INFO hd = new HOADONBAN_CHITIET_INFO();
                hd.SOLUONG = Convert.ToInt32(row["SoLuong"]);
                hd.DONGIABAN = Convert.ToInt64(row["DGban"]);
                hd.THANHTIEN = Convert.ToInt64(row["ThanhTien"]);
                hd.Hanghoa = hh;

                list.Add(hd);
            }
            return list;
        }

    }
}
