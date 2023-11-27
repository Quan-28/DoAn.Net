using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Data.SqlClient;


namespace petStore.FormChuongTrinh
{
    public partial class fTaiKhoanVaNhanVien : Form
    {
        // Khai báo biến toàn cục
        ConnectData dataTable1 = new ConnectData();
        ConnectData dataTable2 = new ConnectData();
        bool capNhat1 = false;
        bool capNhat2 = false;
        string maNV = "";
        string user = "";
        public fTaiKhoanVaNhanVien()
        {
            InitializeComponent();
        }
        // Form load
        private void fTaiKhoanVaNhanVien_Load(object sender, EventArgs e)
        {
            label12.Text = "";
            label13.Text = "";
            // Kết nối với csdl
            dataTable1.OpenConnection();
            dataTable2.OpenConnection();
            // đổ dữ liệu lên datagridview
            LayDuLieu_NhanVien();
            LayDuLieu_TaiKhoan();
            LayDuLieu_TaiKhoan(cboMaNV, "SELECT * FROM NHANVIEN", "MANV", "MANV");
            LayDuLieu_TaiKhoan(cboQuyen, "SELECT * FROM TAIKHOAN", "QUYENHAN", "QUYENHAN");
            // format lại datagridview khi chưa selection
            dgvNhanVien.ClearSelection();
            dgvTaiKhoan.ClearSelection();
            // Việt hóa tiêu đề dgvNhanVien
            dgvNhanVien.Columns["MANV"].HeaderText = "MÃ NHÂN VIÊN";
            dgvNhanVien.Columns["TENNV"].HeaderText = "TÊN NHÂN VIÊN";
            dgvNhanVien.Columns["CCCD"].HeaderText = "CĂN CƯỚC CÔNG DÂN";
            dgvNhanVien.Columns["GIOITINH"].HeaderText = "GIỚI TÍNH";
            dgvNhanVien.Columns["NGAYSINH"].HeaderText = "NGÀY SINH";
            dgvNhanVien.Columns["SDT"].HeaderText = "SỐ ĐIỆN THOẠI";
            dgvNhanVien.Columns["ANH"].HeaderText = "ẢNH";

            // Việt hóa tiêu đề dgvTaiKhoan
            dgvTaiKhoan.Columns["MANV"].HeaderText = "MÃ NV";
            dgvTaiKhoan.Columns["TENDANGNHAP"].HeaderText = "TÊN ĐĂNG NHẬP";
            dgvTaiKhoan.Columns["MATKHAU"].HeaderText = "MẬT KHẨU";
            dgvTaiKhoan.Columns["QUYENHAN"].HeaderText = "QUYỀN HẠN";
            dgvTaiKhoan.Columns["GHICHU"].HeaderText = "GHI CHÚ";

            // Làm sáng nút Thêm mới, Sửa và Xóa
            btnThem1.Enabled = true;
            btnSua1.Enabled = true;
            btnXoa1.Enabled = true;
            btnThem2.Enabled = true;
            btnSua2.Enabled = true;
            btnXoa2.Enabled = true;
            
            //Làm mờ nút lưu và bỏ qua
            btnLuu1.Enabled = false;
            btnHuyBo1.Enabled = false;
            btnLuu2.Enabled = false;
            btnHuyBo2.Enabled = false;
            btnUpAnh.Enabled = false;
            btnXoaAnh.Enabled = false;

            // làm mờ các trường nhập dữ liệu
            //NHANVIEN
            txtMaNV.Enabled = false;
            txtTenNV.Enabled = false;
            txtCCCD.Enabled = false;
            rdNam.Enabled = false;
            rdNu.Enabled = false;
            dtpNgaySinh.Enabled = false;
            txtSDT.Enabled = false;
            //TAIKHOAN
            cboMaNV.Enabled = false;
            txtUser.Enabled = false;
            txtPass.Enabled = false;
            cboQuyen.Enabled = false;
            txtGhiChu.Enabled = false;
            // điều chỉnh lại cột ảnh trong datagridview
            DataGridViewImageColumn pic = new DataGridViewImageColumn();
            pic = (DataGridViewImageColumn)dgvNhanVien.Columns["ANH"];
            pic.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        #region Lấy dữ liệu
        // lấy dữ liệu nhân viên
        public void LayDuLieu_NhanVien()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT MANV, TENNV, CCCD, (CASE WHEN GIOITINH = '0' THEN N'Nam' ELSE N'Nữ' END) AS GIOITINH, NGAYSINH, SDT, ANH
                                                FROM NHANVIEN");
            dataTable1.Fill(cmd);
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = dataTable1;
            dgvNhanVien.DataSource = binding1;
        }
        public void LayDuLieu_TaiKhoan()
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * FROM TAIKHOAN");
            dataTable2.Fill(cmd);
            BindingSource binding2 = new BindingSource();
            binding2.DataSource = dataTable2;
            dgvTaiKhoan.DataSource = binding2;
        }
        // Lấy dữ liệu vào các ComboBox:
        public void LayDuLieu_TaiKhoan(ComboBox comboBox, string data, string display, string value)
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
        #endregion
        #region button của dgvNhanVien
        // Sự kiện Click của nút Thêm:
        private void btnThem1_Click(object sender, EventArgs e)
        {
            capNhat1 = false;
            LayDuLieu_TaiKhoan(cboMaNV, "SELECT * FROM NHANVIEN", "MANV", "MANV");
            LayDuLieu_TaiKhoan(cboQuyen, "SELECT * FROM TAIKHOAN", "QUYENHAN", "QUYENHAN");
            // làm trống các trường nhập dữ liệu
            XoaTrangTruongDuLieu();
            txtMaNV.Focus();
            // làm mờ nút Thêm, Sửa, Xóa
            btnThem1.Enabled = false;
            btnSua1.Enabled = false;
            btnXoa1.Enabled = false;
            // làm sáng các trường nhập dữ liệu
            txtMaNV.Enabled = true;
            txtTenNV.Enabled = true;
            txtCCCD.Enabled = true;
            rdNam.Enabled = true;
            rdNu.Enabled = true;
            dtpNgaySinh.Enabled = true;
            txtSDT.Enabled = true;
            //làm sáng nút Lưu và Bỏ qua
            btnLuu1.Enabled = true;
            btnHuyBo1.Enabled = true;
            btnUpAnh.Enabled = true;
            btnXoaAnh.Enabled = true;
        }
        // Sự kiện Click của nút Xóa:
        private void btnSua1_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count > 0)
            {
                // Đánh dấu là Cập nhật
                capNhat1 = true;
                maNV = txtMaNV.Text;
                LayDuLieu_TaiKhoan(cboMaNV, "SELECT * FROM NHANVIEN", "MANV", "MANV");
                LayDuLieu_TaiKhoan(cboQuyen, "SELECT * FROM TAIKHOAN", "QUYENHAN", "QUYENHAN");
                // Làm mờ nút Thêm mới, Sửa và Xóa
                btnThem1.Enabled = false;
                btnSua1.Enabled = false;
                btnXoa1.Enabled = false;

                // Làm sáng nút Lưu và Bỏ qua
                btnLuu1.Enabled = true;
                btnHuyBo1.Enabled = true;
                btnUpAnh.Enabled = true;
                btnXoaAnh.Enabled = true;

                // làm sáng các trường nhập dữ liệu
                txtMaNV.Enabled = true;
                txtTenNV.Enabled = true;
                txtCCCD.Enabled = true;
                rdNam.Enabled = true;
                rdNu.Enabled = true;
                dtpNgaySinh.Enabled = true;
                txtSDT.Enabled = true;
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 nhân viên để sửa thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Sự kiện Click của nút Xóa:
        private void btnXoa1_Click(object sender, EventArgs e)
        {
            if (dgvNhanVien.SelectedRows.Count > 0)
            {
                DialogResult kq;
                kq = MessageBox.Show("Bạn có muốn xóa Mã nhân viên " + txtMaNV.Text + " không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (kq == DialogResult.Yes)
                {
                    string sql = @"DELETE FROM NHANVIEN WHERE MANV = @manv";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@manv", SqlDbType.NVarChar, 5).Value = txtMaNV.Text;
                    dataTable1.Update(cmd);

                }

                // Tải lại form
                fTaiKhoanVaNhanVien_Load(sender, e);
                XoaTrangTruongDuLieu();
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 nhân viên để Xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Sự kiện Click của nút Lưu dữ liệu:
        private void btnLuu1_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu (Các trường không được rỗng, mã sách không trùng)
            if (txtMaNV.Text.Trim() == "")
                MessageBox.Show("Mã nhân viên không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtTenNV.Text.Trim() == "")
                MessageBox.Show("Tên nhân viên không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                bool error = false;
                DataGridViewRow currentRow = dgvNhanVien.CurrentRow;
                foreach (DataGridViewRow row in dgvNhanVien.Rows)
                {
                    if (capNhat1 && row == currentRow) continue;
                    if (row.Cells["MANV"].Value.ToString() == txtMaNV.Text)
                        error = true;
                }

                if (error)
                    MessageBox.Show("Mã nhân viên " + txtMaNV.Text + " đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // Lưu các thông tin trên các control vào CSDL
                    try
                    {
                        if (capNhat1)
                        {
                            string sql1 = @"UPDATE   NHANVIEN
                                               SET      MANV = @manv,
                                                        TENNV = @tennv,
                                                        CCCD = @cccd,
                                                        GIOITINH = @gioitinh,
                                                        NGAYSINH = @ngaysinh,
                                                        SDT = @sdt,
                                                        ANH = @anh
                                               WHERE    MANV = @manvcu";
                            string sql2 = @"UPDATE   NHANVIEN
                                               SET      MANV = @manv,
                                                        TENNV = @tennv,
                                                        CCCD = @cccd,
                                                        GIOITINH = @gioitinh,
                                                        NGAYSINH = @ngaysinh,
                                                        SDT = @sdt,
                                                        ANH = NULL
                                               WHERE    MANV = @manvcu";
                            SqlCommand cmd;
                            if (pictureBox1.Image != null)
                            {
                                cmd = new SqlCommand(sql1);
                                cmd.Parameters.Add("@manv", SqlDbType.VarChar).Value = txtMaNV.Text;
                                cmd.Parameters.Add("@tennv", SqlDbType.NVarChar).Value = txtTenNV.Text;
                                cmd.Parameters.Add("@cccd", SqlDbType.VarChar).Value = txtCCCD.Text;
                                cmd.Parameters.Add("@gioitinh", SqlDbType.Bit).Value = rdNu.Checked ? 1 : 0;
                                cmd.Parameters.Add("@ngaysinh", SqlDbType.Date).Value = dtpNgaySinh.Value;
                                cmd.Parameters.Add("@sdt", SqlDbType.NVarChar).Value = txtSDT.Text;
                                cmd.Parameters.AddWithValue("@anh", chuyenAnhthanhByte(pictureBox1));
                                cmd.Parameters.Add("@manvcu", SqlDbType.VarChar).Value = maNV;
                            }
                            else
                            {
                                cmd = new SqlCommand(sql2);
                                cmd.Parameters.Add("@manv", SqlDbType.VarChar).Value = txtMaNV.Text;
                                cmd.Parameters.Add("@tennv", SqlDbType.NVarChar).Value = txtTenNV.Text;
                                cmd.Parameters.Add("@cccd", SqlDbType.VarChar).Value = txtCCCD.Text;
                                cmd.Parameters.Add("@gioitinh", SqlDbType.Bit).Value = rdNu.Checked ? 1 : 0;
                                cmd.Parameters.Add("@ngaysinh", SqlDbType.Date).Value = dtpNgaySinh.Value;
                                cmd.Parameters.Add("@sdt", SqlDbType.NVarChar).Value = txtSDT.Text;
                                cmd.Parameters.Add("@manvcu", SqlDbType.VarChar).Value = maNV;
                            }
                            dataTable1.Update(cmd);
                        }
                        else
                        {
                            string sql1 = @"INSERT INTO NHANVIEN (MANV, TENNV, CCCD, GIOITINH, NGAYSINH, SDT, ANH)
                                           VALUES(@manv, @tennv, @cccd, @gioitinh, @ngaysinh, @sdt, @anh)";
                            string sql2 = @"INSERT INTO NHANVIEN (MANV, TENNV, CCCD, GIOITINH, NGAYSINH, SDT)
                                           VALUES(@manv, @tennv, @cccd, @gioitinh, @ngaysinh, @sdt)";
                            SqlCommand cmd;
                            if (pictureBox1.Image != null)
                            {
                                cmd = new SqlCommand(sql1);
                                cmd.Parameters.Add("@manv", SqlDbType.VarChar).Value = txtMaNV.Text;
                                cmd.Parameters.Add("@tennv", SqlDbType.NVarChar).Value = txtTenNV.Text;
                                cmd.Parameters.Add("@cccd", SqlDbType.VarChar).Value = txtCCCD.Text;
                                cmd.Parameters.Add("@gioitinh", SqlDbType.Bit).Value = rdNu.Checked ? 1 : 0;
                                cmd.Parameters.Add("@ngaysinh", SqlDbType.Date).Value = dtpNgaySinh.Value;
                                cmd.Parameters.Add("@sdt", SqlDbType.NVarChar).Value = txtSDT.Text;
                                cmd.Parameters.AddWithValue("@anh", chuyenAnhthanhByte(pictureBox1));
                            }
                            else
                            {
                                cmd = new SqlCommand(sql2);
                                cmd.Parameters.Add("@manv", SqlDbType.VarChar).Value = txtMaNV.Text;
                                cmd.Parameters.Add("@tennv", SqlDbType.NVarChar).Value = txtTenNV.Text;
                                cmd.Parameters.Add("@cccd", SqlDbType.VarChar).Value = txtCCCD.Text;
                                cmd.Parameters.Add("@gioitinh", SqlDbType.Bit).Value = rdNu.Checked ? 1 : 0;
                                cmd.Parameters.Add("@ngaysinh", SqlDbType.Date).Value = dtpNgaySinh.Value;
                                cmd.Parameters.Add("@sdt", SqlDbType.NVarChar).Value = txtSDT.Text;
                            }
                            dataTable1.Update(cmd);
                            
                        }

                        // Tải lại form
                        fTaiKhoanVaNhanVien_Load(sender, e);
                        XoaTrangTruongDuLieu();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnHuyBo1_Click(object sender, EventArgs e)
        {
            fTaiKhoanVaNhanVien_Load(sender, e);
            XoaTrangTruongDuLieu();
        }
        #endregion
        #region button của dgvTaiKhoan
        private void btnThem2_Click(object sender, EventArgs e)
        {
            capNhat2 = false;
            // làm trống các trường nhập dữ liệu
            XoaTrangTruongDuLieu();
            cboMaNV.Focus();
            // làm mờ nút Thêm, Sửa, Xóa
            btnThem2.Enabled = false;
            btnSua2.Enabled = false;
            btnXoa2.Enabled = false;
            // làm sáng các trường nhập dữ liệu
            cboMaNV.Enabled = true;
            txtUser.Enabled = true;
            txtPass.Enabled = true;
            cboQuyen.Enabled = true;
            txtGhiChu.Enabled = true;
            //làm sáng nút Lưu và Bỏ qua
            btnLuu2.Enabled = true;
            btnHuyBo2.Enabled = true;
        }

        private void btnSua2_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.SelectedRows.Count > 0)
            {
                // Đánh dấu là Cập nhật
                capNhat2 = true;
                user = txtUser.Text;

                // Làm mờ nút Thêm mới, Sửa và Xóa
                btnThem2.Enabled = false;
                btnSua2.Enabled = false;
                btnXoa2.Enabled = false;

                // Làm sáng nút Lưu và Bỏ qua
                btnLuu2.Enabled = true;
                btnHuyBo2.Enabled = true;

                // làm sáng các trường nhập dữ liệu
                cboMaNV.Enabled = true;
                txtUser.Enabled = true;
                txtPass.Enabled = true;
                cboQuyen.Enabled = true;
                txtGhiChu.Enabled = true;
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 tài khoản để sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoa2_Click(object sender, EventArgs e)
        {
            if (dgvTaiKhoan.SelectedRows.Count > 0)
            {
                DialogResult kq;
                kq = MessageBox.Show("Bạn có muốn xóa Tài khoản " + txtUser.Text + " không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (kq == DialogResult.Yes)
                {
                    string sql = @"DELETE FROM TAIKHOAN WHERE TENDANGNHAP = @tendangnhap";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@tendangnhap", SqlDbType.NVarChar).Value = txtUser.Text;
                    dataTable2.Update(cmd);

                }

                // Tải lại form
                fTaiKhoanVaNhanVien_Load(sender, e);
                XoaTrangTruongDuLieu();
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 tài khoản để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLuu2_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu (Các trường không được rỗng, mã sách không trùng)
            if (txtUser.Text.Trim() == "")
                MessageBox.Show("Tên đăng nhập không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtPass.Text.Trim() == "")
                MessageBox.Show("Mật khẩu không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                bool error = false;
                DataGridViewRow currentRow = dgvTaiKhoan.CurrentRow;
                foreach (DataGridViewRow row in dgvTaiKhoan.Rows)
                {
                    if (capNhat2 && row == currentRow) continue;
                    if (row.Cells["TENDANGNHAP"].Value.ToString() == txtMaNV.Text)
                        error = true;
                }

                if (error)
                    MessageBox.Show("Tên đăng nhập " + txtUser.Text + " đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // Lưu các thông tin trên các control vào CSDL
                    try
                    {
                        if (capNhat2)
                        {
                            string sql = @"UPDATE   TAIKHOAN
                                           SET      MANV = @manv,
                                                    TENDANGNHAP = @tendangnhap,
                                                    MATKHAU = @matkhau,
                                                    QUYENHAN = @quyenhan,
                                                    GHICHU = @ghichu
                                           WHERE    TENDANGNHAP = @tendangnhapcu";
                            SqlCommand cmd = new SqlCommand(sql);
                            cmd.Parameters.Add("@manv", SqlDbType.VarChar).Value = cboMaNV.SelectedValue.ToString();
                            cmd.Parameters.Add("@tendangnhap", SqlDbType.VarChar).Value = txtUser.Text;
                            cmd.Parameters.Add("@matkhau", SqlDbType.VarChar).Value = txtPass.Text;
                            cmd.Parameters.Add("@quyenhan", SqlDbType.VarChar).Value = cboQuyen.SelectedValue.ToString();
                            cmd.Parameters.Add("@ghichu", SqlDbType.NVarChar).Value = txtGhiChu.Text;
                            cmd.Parameters.Add("@tendangnhapcu", SqlDbType.VarChar).Value = user;
                            dataTable2.Update(cmd);
                        }
                        else
                        {
                            string sql = @"INSERT INTO TAIKHOAN (MANV, TENDANGNHAP, MATKHAU, QUYENHAN, GHICHU)
                                           VALUES(@manv, @tendangnhap, @matkhau, @quyenhan, @ghichu)";
                            SqlCommand cmd = new SqlCommand(sql);
                            cmd.Parameters.Add("@manv", SqlDbType.VarChar).Value = cboMaNV.SelectedValue.ToString();
                            cmd.Parameters.Add("@tendangnhap", SqlDbType.VarChar).Value = txtUser.Text;
                            cmd.Parameters.Add("@matkhau", SqlDbType.VarChar).Value = txtPass.Text;
                            cmd.Parameters.Add("@quyenhan", SqlDbType.VarChar).Value = cboQuyen.SelectedValue.ToString();
                            cmd.Parameters.Add("@ghichu", SqlDbType.NVarChar).Value = txtGhiChu.Text;
                            dataTable2.Update(cmd);
                        }

                        // Tải lại form
                        fTaiKhoanVaNhanVien_Load(sender, e);
                        XoaTrangTruongDuLieu();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        private void btnHuyBo2_Click(object sender, EventArgs e)
        {
            fTaiKhoanVaNhanVien_Load(sender, e);
            XoaTrangTruongDuLieu();
        }
        #endregion
        #region Xử lý ảnh
        private void btnUpAnh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "JPG files (*.jpg)|*.jpg|png files(*.jpg)|*.jpg|ALL files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtLocationIMG.Text = openFileDialog1.FileName;
                pictureBox1.ImageLocation = txtLocationIMG.Text;
            }
        }
        private void btnXoaAnh_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }
        // Chuyển ảnh sang dạng Byte
        private byte[] chuyenAnhthanhByte(PictureBox ptb)
        {
            MemoryStream ms = new MemoryStream();
            ptb.Image.Save(ms, ptb.Image.RawFormat);
            return ms.ToArray();
            /*
            FileStream fs;
            fs = new FileStream(txtLocationIMG.Text, FileMode.Open, FileAccess.Read);
            byte[] anhByte = new byte[fs.Length];
            fs.Read(anhByte, 0, System.Convert.ToInt32(fs.Length));
            fs.Close();
            return anhByte;
            */
        }
        // Chuyển Byte sang dạng Ảnh
        private Image chuyenBytethanhImage(byte[] byteArr)
        {
            MemoryStream ms = new MemoryStream(byteArr);
            Image img = Image.FromStream(ms);
            return img;
        }
        #endregion
        // Định dạng lại mật khẩu trong dgvTaiKhoan để tăng tính bảo mật
        private void dgvTaiKhoan_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvTaiKhoan.Columns[e.ColumnIndex].Name == "MATKHAU")
            {
                e.Value = "••••••••••";
            }
        }

        private void txtCCCD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
                label12.Text = "";
            }
            else
            {
                label12.Text = "CCCD chỉ được nhập giá trị là số!";
                e.Handled = true;
            }
        }

        private void txtSDT_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
                label13.Text = "";
            }
            else
            {
                label13.Text = "Số điện thoại chỉ được nhập giá trị là số!";
                e.Handled = true;
            }
        }

        private void dgvNhanVien_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Khi click vào dgvNhanVien thì hiển thị dữ liệu của dòng được chọn lên các control
            XoaCacDataBingdings();

            txtMaNV.DataBindings.Add("Text", dgvNhanVien.DataSource, "MANV", false, DataSourceUpdateMode.Never);
            txtTenNV.DataBindings.Add("Text", dgvNhanVien.DataSource, "TENNV", false, DataSourceUpdateMode.Never);
            txtCCCD.DataBindings.Add("Text", dgvNhanVien.DataSource, "CCCD", false, DataSourceUpdateMode.Never);
            dtpNgaySinh.DataBindings.Add("Value", dgvNhanVien.DataSource, "NGAYSINH", false, DataSourceUpdateMode.Never);
            txtSDT.DataBindings.Add("Text", dgvNhanVien.DataSource, "SDT", false, DataSourceUpdateMode.Never);
            pictureBox1.DataBindings.Add("Image", dgvNhanVien.DataSource, "ANH", true, DataSourceUpdateMode.Never);
            string GioiTinh;
            GioiTinh = dgvNhanVien.CurrentRow.Cells["GIOITINH"].Value.ToString();
            if (GioiTinh == "Nam")
            {
                rdNam.Checked = true;
            }
            else if (GioiTinh == "Nữ")
            {
                rdNu.Checked = true;
            }
        }
        private void dgvTaiKhoan_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Khi click vào dgvTaiKhoan thì hiển thị dữ liệu của dòng được chọn lên các control
            XoaCacDataBingdings();

            cboMaNV.DataBindings.Add("SelectedValue", dgvTaiKhoan.DataSource, "MANV", false, DataSourceUpdateMode.Never);
            txtUser.DataBindings.Add("Text", dgvTaiKhoan.DataSource, "TENDANGNHAP", false, DataSourceUpdateMode.Never);
            txtPass.DataBindings.Add("Text", dgvTaiKhoan.DataSource, "MATKHAU", false, DataSourceUpdateMode.Never);
            cboQuyen.DataBindings.Add("SelectedValue", dgvTaiKhoan.DataSource, "QUYENHAN", false, DataSourceUpdateMode.Never);
            txtGhiChu.DataBindings.Add("Text", dgvTaiKhoan.DataSource, "GHICHU", false, DataSourceUpdateMode.Never);
        }
        private void XoaCacDataBingdings()
        {
            //NHANVIEN
            txtMaNV.DataBindings.Clear();
            txtTenNV.DataBindings.Clear();
            txtCCCD.DataBindings.Clear();
            rdNam.DataBindings.Clear();
            rdNu.DataBindings.Clear();
            dtpNgaySinh.DataBindings.Clear();
            txtSDT.DataBindings.Clear();
            pictureBox1.DataBindings.Clear();
            //TAIKHOAN
            cboMaNV.DataBindings.Clear();
            txtUser.DataBindings.Clear();
            txtPass.DataBindings.Clear();
            cboQuyen.DataBindings.Clear();
            txtGhiChu.DataBindings.Clear();
        }
        private void XoaTrangTruongDuLieu()
        {
            //NHANVIEN
            txtMaNV.Clear();
            txtTenNV.Clear();
            txtCCCD.Clear();
            rdNam.Checked = false;
            rdNu.Checked = false;
            dtpNgaySinh.Value = DateTime.Today;
            txtSDT.Text = "";
            txtLocationIMG.Clear();
            pictureBox1.Image = null;
            //TAIKHOAN
            cboMaNV.Text = "";
            txtUser.Clear();
            txtPass.Clear();
            cboQuyen.Text = "";
            txtGhiChu.Clear();
        }

        private void fTaiKhoanVaNhanVien_Shown(object sender, EventArgs e)
        {
            dgvNhanVien.ClearSelection();
            dgvTaiKhoan.ClearSelection();
        }

        
        /*Hàm kiểm tra dữ liệu trên DataGridView:
public bool KiemTra(string columnName)
{
foreach (DataGridViewRow row in dgvTaiKhoan.Rows)
{
string value = row.Cells[columnName].Value.ToString();
if (string.IsNullOrEmpty(value))
{
MessageBox.Show("Giá trị của ô không được rỗng!", "Lỗi",
MessageBoxButtons.OK, MessageBoxIcon.Error);
return false;
}
}
return true;
}*/
    }
}
