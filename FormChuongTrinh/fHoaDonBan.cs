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
    public partial class fHoaDonBan : Form
    {
        public fHoaDonBan()
        {
            InitializeComponent();
        }

        private void fHoaDonBan_Load(object sender, EventArgs e)
        {
            label16.Text = "";
            // lấy dữ liệu đổ vào combobox TENHH
            txtNhanVien.Text = fQuanLyChinh.HovaTen;
            LayDuLieu_HoaDon(cboTenHH, "SELECT * FROM HANGHOA", "MAHH", "MAHH");

            txtMaHD.Clear();
            txtTongTien.Text = "0";
            cboMaKH.Text = "";
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();

            cboTenHH.Text = "";
            txtTenHH.Clear();
            numSoLuong.Value = 0;
            txtThanhTien.Text = "0";
            txtDGB.Text = "0";
            //Làm mờ 1 vài trường dữ liệu
            txtTenHH.Enabled = false;
            cboTenHH.Enabled = false;
            numSoLuong.Enabled = false;
            dtpHoaDon.Enabled = false;
            cboMaKH.Enabled = false;
            txtMaHD.Enabled = false;
            txtTenKH.Enabled = false;
            txtNhanVien.Enabled = false;
            txtTongTien.Enabled = false;
            txtDGB.Enabled = false;
            txtThanhTien.Enabled = false;
            txtDiaChi.Enabled = false;
            txtSDT.Enabled = false;
            // làm sáng button
            btnTaoHoaDon.Enabled = true;
            //làm mờ button
            btnHuyThem.Enabled = false;
            btnThem.Enabled = false;
            btnXoa.Enabled = false;

            btnKetThuc.Enabled = false;
            btnThanhToan.Enabled = false;
        }
        //Hàm tạo khóa có dạng: TientoNgaythangnam_giophutgiay
        public static string CreateKey(string tiento)
        {
            string key = tiento;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
            //Ví dụ 07/08/2009
            string d = String.Format("{0}{1}{2}", partsDay[1], partsDay[0], partsDay[2]);
            key = key + d;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            //Ví dụ 7:08:03 PM hoặc 7:08:03 AM
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTimeTo24(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            //Xóa ký tự trắng và PM hoặc AM
            partsTime[2] = partsTime[2].Remove(2, 3);
            string t;
            t = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            key = key + t;
            return key;
        }
        //Chuyển đổi từ PM sang dạng 24h
        public static string ConvertTimeTo24(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }


        #region các Sự kiện click của các nút
        //Button Thêm
        private void btnThem_Click(object sender, EventArgs e)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            string sql;
            double sl, SLcon, tong, Tongmoi;
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM NHANVIEN WHERE TENNV = @tennv");
            cmd.Parameters.Add("@tennv", SqlDbType.NVarChar).Value = txtNhanVien.Text;
            data.Fill(cmd);
            string manv = data.Rows[0]["MANV"].ToString();
            sql = @"SELECT MAHDBAN FROM HOADONBAN WHERE MAHDBAN = N'" + txtMaHD.Text + "'";
            if (!CheckKey(sql))
            {
                // Mã hóa đơn chưa có, tiến hành lưu các thông tin chung
                // Mã HDBan được sinh tự động do đó không có trường hợp trùng khóa

                if (cboMaKH.Text.Length == 0)
                {
                    MessageBox.Show("Bạn phải nhập khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cboMaKH.Focus();
                    return;
                }
                sql = @"INSERT INTO HOADONBAN(MAHDBAN, MANV, MAKH, NGAYLAP, THANHTIEN)
                        VALUES (@mahd, @manv, @makh, @ngay, @thanhtien)";
                SqlCommand cmd1 = new SqlCommand(sql);
                cmd1.Parameters.Add("@mahd", SqlDbType.VarChar).Value = txtMaHD.Text;
                cmd1.Parameters.Add("@manv", SqlDbType.VarChar).Value = manv;
                cmd1.Parameters.Add("@makh", SqlDbType.VarChar).Value = cboMaKH.SelectedValue.ToString();
                cmd1.Parameters.Add("@ngay", SqlDbType.DateTime).Value = dtpHoaDon.Value;
                cmd1.Parameters.Add("@thanhtien", SqlDbType.Money).Value = txtTongTien.Text;
                data.Update(cmd1);
            }
            // Lưu thông tin của các mặt hàng
            if (cboTenHH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Bạn phải nhập mã hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTenHH.Focus();
                return;
            }
            if ((numSoLuong.Text.Trim().Length == 0) || (numSoLuong.Value == 0))
            {
                MessageBox.Show("Bạn phải nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                numSoLuong.Value = 0;
                numSoLuong.Focus();
                return;
            }
            sql = @"SELECT MaHH FROM HOADONBAN_CHITIET 
                    WHERE MaHH = N'" + cboTenHH.SelectedValue + "' AND MaHDBan = N'" + txtMaHD.Text.Trim() + "'";
            if (CheckKey(sql))
            {
                MessageBox.Show("Mã hàng này đã có, bạn phải chọn mã khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                cboTenHH.Text = "";
                numSoLuong.Value = 0;
                txtThanhTien.Text = "0";
                cboTenHH.Focus();
                return;
            }
            // Kiểm tra xem số lượng hàng trong kho còn đủ để cung cấp không?
            sl = Convert.ToDouble(LayMienDuLieu("SELECT SOLUONG FROM HANGHOA WHERE MAHH = N'" + cboTenHH.SelectedValue + "'"));
            if (Convert.ToDouble(numSoLuong.Value) > sl)
            {
                MessageBox.Show("Số lượng mặt hàng này chỉ còn " + sl, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                numSoLuong.Value = 0;
                numSoLuong.Focus();
                return;
            }
            sql = @"INSERT INTO HOADONBAN_CHITIET(MaHDban, MaHH, SoLuong, DGban, ThanhTien)
                    VALUES(@mahd, @mahh, @sl, @dgb, @tt)";
            SqlCommand cmd2 = new SqlCommand(sql);
            cmd2.Parameters.Add("@mahd", SqlDbType.VarChar).Value = txtMaHD.Text;
            cmd2.Parameters.Add("@mahh", SqlDbType.VarChar).Value = cboTenHH.SelectedValue.ToString();
            cmd2.Parameters.Add("@sl", SqlDbType.TinyInt).Value = (double)numSoLuong.Value;
            cmd2.Parameters.Add("@dgb", SqlDbType.Money).Value = txtDGB.Text;
            cmd2.Parameters.Add("@tt", SqlDbType.Money).Value = txtThanhTien.Text;
            data.Update(cmd2);
            Load_DuLieu_LenDGV();
            // Cập nhật lại số lượng của mặt hàng vào bảng tblHang
            SLcon = sl - Convert.ToDouble(numSoLuong.Value);
            sql = @"UPDATE HANGHOA 
                    SET SOLUONG = @sl
                    WHERE MAHH = @mahh";
            SqlCommand cmd3 = new SqlCommand(sql);
            cmd3.Parameters.Add("@sl", SqlDbType.TinyInt).Value = SLcon;
            cmd3.Parameters.Add("@mahh", SqlDbType.VarChar).Value = cboTenHH.SelectedValue.ToString();
            data.Update(cmd3);
            // Cập nhật lại tổng tiền cho hóa đơn bán
            tong = Convert.ToDouble(LayMienDuLieu("SELECT THANHTIEN FROM HOADONBAN WHERE MAHDBAN = N'" + txtMaHD.Text + "'"));
            Tongmoi = tong + Convert.ToDouble(txtThanhTien.Text);
            sql = "UPDATE HOADONBAN SET THANHTIEN =" + Tongmoi + " WHERE MaHDBan = N'" + txtMaHD.Text + "'";
            SqlCommand cmd4 = new SqlCommand(sql);
            data.Update(cmd4);
            txtTongTien.Text = Tongmoi.ToString();

            cboTenHH.Text = "";
            numSoLuong.Value = 0;
            txtThanhTien.Text = "0";
            txtTenHH.Text = "";
            txtDGB.Text = "";
            btnXoa.Enabled = true;
            btnKetThuc.Enabled = true;
        }
        //Button Kết Thúc
        private void btnKetThuc_Click(object sender, EventArgs e)
        {
            fHoaDonBan_Load(sender, e);
        }
        //Button xóa 
        private void btnXoa_Click(object sender, EventArgs e)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            string MaHangxoa, sql;
            Double ThanhTienxoa, SoLuongxoa, sl, slcon, tong, tongmoi;
            if ((MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {
                MaHangxoa = dgvChiTietHD.CurrentRow.Cells[0].Value.ToString();
                SoLuongxoa = Convert.ToDouble(dgvChiTietHD.CurrentRow.Cells[2].Value.ToString());
                ThanhTienxoa = Convert.ToDouble(dgvChiTietHD.CurrentRow.Cells[4].Value.ToString());
                sql = "DELETE HOADONBAN_CHITIET WHERE MaHDBan=N'" + txtMaHD.Text + "' AND MaHH = N'" + MaHangxoa + "'";
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
                cmd2.Parameters.Add("@mahh", SqlDbType.VarChar).Value = cboTenHH.SelectedValue.ToString();
                data.Update(cmd2);
                // Cập nhật lại tổng tiền cho hóa đơn bán
                tong = Convert.ToDouble(LayMienDuLieu("SELECT THANHTIEN FROM HOADONBAN WHERE MAHDBAN = N'" + txtMaHD.Text + "'"));
                tongmoi = tong - ThanhTienxoa;
                sql = "UPDATE HOADONBAN SET THANHTIEN =" + tongmoi + " WHERE MaHDBan = N'" + txtMaHD.Text + "'";
                SqlCommand cmd3 = new SqlCommand(sql);
                data.Update(cmd3);
                txtTongTien.Text = tongmoi.ToString();
            }
            Load_DuLieu_LenDGV();
        }
        //Button Tạo Hóa Đơn Mới
        private void btnTaoHoaDon_Click(object sender, EventArgs e)
        {   // lấy dữ liệu đổ vào combobox MaKH
            LayDuLieu_HoaDon(cboMaKH, "SELECT * FROM KHACHHANG", "MAKH", "MAKH");
            
            dtpHoaDon.Value = DateTime.Now;
            txtMaHD.Text = CreateKey("HDB");

            txtNhanVien.Text = fQuanLyChinh.HovaTen;
            txtTongTien.Text = "0";
            cboMaKH.Text = "";
            txtTenKH.Clear();
            txtDiaChi.Clear();
            txtSDT.Clear();

            cboTenHH.Text = "";
            numSoLuong.Value = 0;
            txtThanhTien.Text = "0";
            txtDGB.Text = "0";

            // làm mờ các button
            btnTaoHoaDon.Enabled = false;
            // Enable các trường nhập dữ liệu
            dtpHoaDon.Enabled = true;
            cboMaKH.Enabled = true;
            cboTenHH.Enabled = true;
            numSoLuong.Enabled = true;
            // làm sáng lại các button
            btnHuyThem.Enabled = true;
            btnThem.Enabled = true;
            btnXoa.Enabled = true;

            btnKetThuc.Enabled = true;
            btnThanhToan.Enabled = true;
        }
        //Button Hủy Hóa Đơn
        private void btnHuyThem_Click(object sender, EventArgs e)
        {
            ConnectData data = new ConnectData();
            data.OpenConnection();
            double sl, slcon, slxoa;
            if (MessageBox.Show("Bạn có chắc chắn muốn hủy Hóa đơn không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string sql = "SELECT MAHH,SOLUONG FROM HOADONBAN_CHITIET WHERE MAHDBAN = N'" + txtMaHD.Text + "'";
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
                sql = "DELETE HOADONBAN_CHITIET WHERE MAHDBAN=N'" + txtMaHD.Text + "'";
                SqlCommand cmd1 = new SqlCommand(sql);
                data.Update(cmd1);

                //Xóa hóa đơn
                sql = "DELETE HOADONBAN WHERE MAHDBAN=N'" + txtMaHD.Text + "'";
                SqlCommand cmd2 = new SqlCommand(sql);
                data.Update(cmd2);
                fHoaDonBan_Load(sender, e);
            }
            Load_DuLieu_LenDGV();

        }
        #endregion
        #region Lấy dữ liệu
        // Lấy dữ liệu vào các ComboBox:
        public void LayDuLieu_HoaDon(ComboBox comboBox, string data, string display, string value)
        {
            ConnectData table = new ConnectData();
            table.OpenConnection();
            string sql = data;
            SqlCommand command = new SqlCommand(sql);
            table.Fill(command);
            comboBox.DataSource = table;
            comboBox.DisplayMember = display;
            comboBox.ValueMember = value;
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
        private void Load_DuLieu_LenDGV()
        {
            string sql;
            sql = @"SELECT a.MaHH, b.TENHH, a.SoLuong, b.DONGIABAN, a.ThanhTien 
                    FROM HOADONBAN_CHITIET AS a, HANGHOA AS b 
                    WHERE a.MaHDban = N'" + txtMaHD.Text + "' AND a.MaHH = b.MAHH";
            DataTable tblCTHDB = GetDataToTable(sql);
            dgvChiTietHD.DataSource = tblCTHDB;
            dgvChiTietHD.Columns[0].HeaderText = "Mã hàng";
            dgvChiTietHD.Columns[1].HeaderText = "Tên hàng";
            dgvChiTietHD.Columns[2].HeaderText = "Số lượng";
            dgvChiTietHD.Columns[3].HeaderText = "Đơn giá";
            dgvChiTietHD.Columns[4].HeaderText = "Thành tiền";
        }
        #endregion
        #region Các sự kiện
        private void cboMaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboMaKH.Text == "")
            {
                txtTenKH.Text = "";
                txtDiaChi.Text = "";
                txtSDT.Text = "";
            }
            //Khi chọn Mã khách hàng thì các thông tin của khách hàng sẽ hiện ra
            str = "Select TENKH from KHACHHANG where MAKH = N'" + cboMaKH.SelectedValue + "'";
            txtTenKH.Text = LayMienDuLieu(str);
            str = "Select DIACHI from KHACHHANG where MAKH = N'" + cboMaKH.SelectedValue + "'";
            txtDiaChi.Text = LayMienDuLieu(str);
            str = "Select SDT from KHACHHANG where MAKH = N'" + cboMaKH.SelectedValue + "'";
            txtSDT.Text = LayMienDuLieu(str);
        }
        private void cboTenHH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string str;
            if (cboTenHH.Text == "")
            {
                txtTenHH.Text = "";
                txtDGB.Text = "";
            }
            // Khi chọn mã hàng thì các thông tin về hàng hiện ra
            str = "SELECT TENHH FROM HANGHOA WHERE MAHH =N'" + cboTenHH.SelectedValue + "'";
            txtTenHH.Text = LayMienDuLieu(str);
            str = "SELECT DonGiaBan FROM HANGHOA WHERE MAHH =N'" + cboTenHH.SelectedValue + "'";
            txtDGB.Text = LayMienDuLieu(str);
        }
        //Khi thay đổi số lượng thì thực hiện tính lại thành tiền
        private void numSoLuong_ValueChanged(object sender, EventArgs e)
        {
            
            double tt, sl, dg;
            if (numSoLuong.Value == 0)
                sl = 0;
            else
                sl = (Double)numSoLuong.Value;
            if (txtDGB.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDGB.Text);
            tt = sl * dg;
            txtThanhTien.Text = tt.ToString();
        }
        //Khi thay đổi Đơn giá bán thì thực hiện tính lại thành tiền
        private void txtDGB_TextChanged(object sender, EventArgs e)
        {

            double tt, sl, dg;
            if (numSoLuong.Value == 0)
                sl = 0;
            else
                sl = (Double)numSoLuong.Value;
            if (txtDGB.Text == "")
                dg = 0;
            else
                dg = Convert.ToDouble(txtDGB.Text);
            tt = sl * dg;
            txtThanhTien.Text = tt.ToString();
        }
        //Kiểm tra và thông báo nếu nhập số lượng không phải là số
        private void numSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
                label16.Text = "";
            }
            else
            {
                label16.Text = "Số lượng chỉ được nhập giá trị là số!";
                e.Handled = true;
            }
        }
        
        #endregion


        //Hàm kiểm tra tồn tại khóa
        public static bool CheckKey(string sql)
        {
            ConnectData dataTable = new ConnectData();
            dataTable.OpenConnection();
            SqlCommand cmd = new SqlCommand(sql);
            dataTable.Fill(cmd);
            if (dataTable.Rows.Count > 0)
                return true;
            else return false;
        }

        private void btnThanhToan_Click(object sender, EventArgs e)
        {
            try
            {
                string hd = txtMaHD.Text;
                using (Report.fInHoaDonBan form = new Report.fInHoaDonBan(hd))
                {
                    form.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                // Xử lý ngoại lệ ở đây, ví dụ: hiển thị thông báo lỗi
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
