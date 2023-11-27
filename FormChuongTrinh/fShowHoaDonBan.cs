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

namespace petStore.FormChuongTrinh
{
    public partial class fShowHoaDonBan : Form
    {
        // Khai báo biến toàn cục
        ConnectData datahoadonban = new ConnectData();
        public fShowHoaDonBan()
        {
            InitializeComponent();
        }

        private void fShowHoaDonBan_Load(object sender, EventArgs e)
        {
            // kết nối với csdl
            datahoadonban.OpenConnection();
            // Đổ dữ liệu lên datagridview
            LayDuLieu_HoaDon();
            // Việt hóa tiêu đề dgvNhanVien
            dgvHoaDonBan.Columns["MAHDBAN"].HeaderText = "MÃ HD";
            dgvHoaDonBan.Columns["TENNV"].HeaderText = "NHÂN VIÊN";
            dgvHoaDonBan.Columns["TENKH"].HeaderText = "KHÁCH HÀNG";
            dgvHoaDonBan.Columns["NGAYLAP"].HeaderText = "NGÀY LẬP";
            dgvHoaDonBan.Columns["THANHTIEN"].HeaderText = "TỔNG TIỀN";
        }
        public void LayDuLieu_HoaDon()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT hdb.MAHDBAN,kh.TENKH,nv.TENNV,hdb.NGAYLAP,hdb.THANHTIEN
                                              FROM ((HOADONBAN as hdb
                                              inner join KHACHHANG as kh on hdb.MAKH = kh.MAKH)
                                              inner join NHANVIEN as nv on hdb.MANV = nv.MANV)");
            datahoadonban.Fill(cmd);
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = datahoadonban;
            dgvHoaDonBan.DataSource = binding1;
            bindingNavigator1.BindingSource = binding1;

            dgvChiTiet.DataSource = null;
            dgvChiTiet.Rows.Clear();
            dgvChiTiet.Refresh();
        }
        /*
        public void LayDuLieu_ChiTietHoaDon()
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT TENHH FROM HOADONBAN_CHITIET ct, HANGHOA hh WHERE ct.MaHH = hh.MAHH");
            data.Fill(cmd);
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = data;
            dgvChiTiet.DataSource = binding1;
            bindingNavigator2.BindingSource = binding1;
        }*/
        public void LayDuLieu_ChiTietHoaDon(string mahdb)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            SqlCommand cmd = new SqlCommand(@"SELECT a.MaHH, b.TENHH, a.SoLuong, a.DGban, a.ThanhTien 
                                              FROM HOADONBAN_CHITIET AS a 
                                              INNER JOIN HANGHOA AS b
                                              ON MaHDban = @mahd AND a.MaHH = b.MAHH");
            cmd.Parameters.Add("@mahd", SqlDbType.VarChar).Value = mahdb;
            data.Fill(cmd);
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = data;
            dgvChiTiet.DataSource = binding1;
            bindingNavigator2.BindingSource = binding1;
        }

        private void dgvHoaDonBan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
            string mahd = dgvHoaDonBan.SelectedRows[0].Cells["MAHDBAN"].Value.ToString(); 
            LayDuLieu_ChiTietHoaDon(mahd);
            dgvChiTiet.Columns["MaHH"].HeaderText = "Mã hàng";
            dgvChiTiet.Columns["TenHH"].HeaderText = "Tên hàng";
            dgvChiTiet.Columns["SoLuong"].HeaderText = "Số lượng";
            dgvChiTiet.Columns["DGban"].HeaderText = "Đơn giá";
            dgvChiTiet.Columns["ThanhTien"].HeaderText = "Thành tiền";
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT hdb.MAHDBAN,kh.TENKH,nv.TENNV,hdb.NGAYLAP,hdb.THANHTIEN
                                              FROM ((HOADONBAN as hdb
                                              inner join KHACHHANG as kh on hdb.MAKH = kh.MAKH)
                                              inner join NHANVIEN as nv on hdb.MANV = nv.MANV)
                                              where hdb.NGAYLAP between @from and @to");
            cmd.Parameters.Add("@from", SqlDbType.DateTime).Value = dtpFrom.Value;
            cmd.Parameters.Add("@to", SqlDbType.DateTime).Value = dtpTo.Value.AddMonths(1);
            datahoadonban.Fill(cmd);
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = datahoadonban;
            dgvHoaDonBan.DataSource = binding1;
            bindingNavigator1.BindingSource = binding1;

            dgvChiTiet.DataSource = null;
            dgvChiTiet.Rows.Clear();
            dgvChiTiet.Refresh();
        }

        private void tsbtnLoadHD_Click(object sender, EventArgs e)
        {
            LayDuLieu_HoaDon();
        }

        private void tsbtnXoa_Click(object sender, EventArgs e)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            double sl, slcon, slxoa;
            string mahd;
            mahd = dgvHoaDonBan.CurrentRow.Cells[0].Value.ToString();
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa Hóa đơn "+mahd+" không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MAHH,SOLUONG FROM HOADONBAN_CHITIET WHERE MAHDBAN = N'" + mahd + "'";
                DataTable tblHang = GetDataToTable(sql);
                for (int hang = 0; hang <= tblHang.Rows.Count - 1; hang++)
                {
                    // Cập nhật lại số lượng cho các mặt hàng
                    sl = Convert.ToDouble(LayMienDuLieu("SELECT SOLUONG FROM HANGHOA WHERE MAHH = N'" + tblHang.Rows[hang][0].ToString() + "'"));
                    slxoa = Convert.ToDouble(tblHang.Rows[hang][1].ToString());
                    slcon = sl + slxoa;
                    sql = "UPDATE HANGHOA SET SOLUONG =" + slcon + " WHERE MAHH= N'" + tblHang.Rows[hang][0].ToString() + "'";
                    SqlCommand cmd = new SqlCommand(sql);
                    data.Update(cmd);
                }

                //Xóa chi tiết hóa đơn
                sql = "DELETE HOADONBAN_CHITIET WHERE MAHDBAN=N'" + mahd+ "'";
                SqlCommand cmd1 = new SqlCommand(sql);
                data.Update(cmd1);

                //Xóa hóa đơn
                sql = "DELETE HOADONBAN WHERE MAHDBAN=N'" + mahd + "'";
                SqlCommand cmd2 = new SqlCommand(sql);
                data.Update(cmd2);
                fShowHoaDonBan_Load(sender, e);
            }
        }
        private void tsbtnXoaChiTiet_Click(object sender, EventArgs e)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            string mahd, mahh;
            mahd = dgvHoaDonBan.CurrentRow.Cells[0].Value.ToString();
            mahh = dgvChiTiet.CurrentRow.Cells[0].Value.ToString();
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa hàng hóa có mã: "+mahh+" không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                MaHangxoa = dgvChiTiet.CurrentRow.Cells[0].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvChiTiet.CurrentRow.Cells[2].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvChiTiet.CurrentRow.Cells[4].Value.ToString());
                sql = "DELETE HOADONBAN_CHITIET WHERE MaHDBan=N'" + mahd + "' AND MaHH = N'" + MaHangxoa + "'";
                SqlCommand cmd1 = new SqlCommand(sql);
                data.Update(cmd1);
                // Cập nhật lại số lượng cho các mặt hàng
                sl = Convert.ToDouble(LayMienDuLieu("SELECT SOLUONG FROM HANGHOA WHERE MAHH = N'" + MaHangxoa + "'"));
                slcon = sl + SoLuongxoa;
                sql = @"UPDATE HANGHOA 
                    SET SOLUONG = @sl
                    WHERE MAHH = @mahh";
                SqlCommand cmd2 = new SqlCommand(sql);
                cmd2.Parameters.Add("@sl", SqlDbType.TinyInt).Value = slcon;
                cmd2.Parameters.Add("@mahh", SqlDbType.VarChar).Value = mahh;
                data.Update(cmd2);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(LayMienDuLieu("SELECT THANHTIEN FROM HOADONBAN WHERE MAHDBAN = N'" + mahd + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE HOADONBAN SET THANHTIEN =" + tongmoi + " WHERE MaHDBan = N'" + mahd + "'";
                SqlCommand cmd3 = new SqlCommand(sql);
                data.Update(cmd3);
            }
            LayDuLieu_ChiTietHoaDon(mahd);
        }
        //Lấy dữ liệu vào bảng
        public static DataTable GetDataToTable(string sql)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            SqlDataAdapter dap = new SqlDataAdapter(sql, data.connection); //Định nghĩa đối tượng thuộc lớp SqlDataAdapter
            //Khai báo đối tượng table thuộc lớp DataTable
            DataTable table = new DataTable();
            dap.Fill(table); //Đổ kết quả từ câu lệnh sql vào table
            return table;
        }
        // Lấy dữ liệu vào các TextBox:
        public static string LayMienDuLieu(string sql)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            string ma = "";
            SqlCommand cmd = new SqlCommand(sql, data.connection);
            data.Fill(cmd);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                ma = reader.GetValue(0).ToString();
            reader.Close();
            return ma;
        }

        private void tsbtnPrint_Click(object sender, EventArgs e)
        {
            string hd = dgvHoaDonBan.CurrentRow.Cells[0].Value.ToString();
            using (Report.fInHoaDonBan form = new Report.fInHoaDonBan(hd))
            {
                form.ShowDialog();
            }
        }
    }
}
