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
using System.IO;

namespace petStore.FormChuongTrinh
{
    public partial class fHangHoa : Form
    {
        ConnectData dataHangHoa = new ConnectData();
        bool capNhat = false;
        string mahh = "";
        public fHangHoa()
        {
            InitializeComponent();
        }

        private void fHangHoa_Load(object sender, EventArgs e)
        {
            label11.Text = "";
            label12.Text = "";
            // Kết nối với csdl
            dataHangHoa.OpenConnection();
            // đổ dữ liệu lên datagridview
            LayDuLieu_HangHoa();
            LayDuLieu_HangHoa(cboLoai, "SELECT * FROM LOAIHH", "TENLOAI", "MALOAI");
            LayDuLieu_HangHoa(cboNhaCungCap, "SELECT * FROM NHACUNGCAP", "TENNCC", "MANCC");
            // format lại datagridview khi chưa selection
            dgvHangHoa.ClearSelection();
            
            // Việt hóa tiêu đề dgvHangHoa
            dgvHangHoa.Columns["MAHH"].HeaderText = "MÃ HÀNG HÓA";
            dgvHangHoa.Columns["TENHH"].HeaderText = "TÊN HÀNG HÓA";
            dgvHangHoa.Columns["MALOAI"].HeaderText = "LOẠI HÀNG HÓA";
            dgvHangHoa.Columns["SOLUONG"].HeaderText = "SỐ LƯỢNG";
            dgvHangHoa.Columns["DONGIABAN"].HeaderText = "ĐƠN GIÁ BÁN";
            dgvHangHoa.Columns["DONGIANHAP"].HeaderText = "ĐƠN GIÁ NHẬP";
            dgvHangHoa.Columns["MANCC"].HeaderText = "NHÀ CUNG CẤP";
            dgvHangHoa.Columns["MOTA"].HeaderText = "MÔ TẢ";
            dgvHangHoa.Columns["ANH"].HeaderText = "ẢNH";

            // Làm sáng nút Thêm mới, Sửa và Xóa
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            //Làm mờ nút lưu và bỏ qua
            btnLuu.Enabled = false;
            btnHuyBo.Enabled = false;
            btnChonAnh.Enabled = false;
            btnXoaAnh.Enabled = false;
            // làm mờ các trường nhập dữ liệu
            //NHANVIEN
            txtMaHH.Enabled = false;
            txtTenHH.Enabled = false;
            cboLoai.Enabled = false;
            numSoLuong.Enabled = false;
            numDGB.Enabled = false;
            NumDGN.Enabled = false;
            cboNhaCungCap.Enabled = false;
            txtMoTa.Enabled = false;
            // điều chỉnh lại cột ảnh trong datagridview
            DataGridViewImageColumn pic = new DataGridViewImageColumn();
            pic = (DataGridViewImageColumn)dgvHangHoa.Columns["ANH"];
            pic.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        #region button của dgvNhanVien
        // Sự kiện Click của nút Thêm:
        private void btnThem_Click(object sender, EventArgs e)
        {
            capNhat = false;
            LayDuLieu_HangHoa(cboLoai, "SELECT * FROM LOAIHH", "TENLOAI", "MALOAI");
            LayDuLieu_HangHoa(cboNhaCungCap, "SELECT * FROM NHACUNGCAP", "TENNCC", "MANCC");
            XoaTrangTruongDuLieu();
            txtMaHH.Text = "HH";
            txtMaHH.Focus();
            // làm mờ nút Thêm, Sửa, Xóa
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            // làm sáng các trường nhập dữ liệu
            txtMaHH.Enabled = true;
            txtTenHH.Enabled = true;
            cboLoai.Enabled = true;
            numSoLuong.Enabled = true;
            numDGB.Enabled = true;
            NumDGN.Enabled = true;
            cboNhaCungCap.Enabled = true;
            txtMoTa.Enabled = true;
            //làm sáng nút Lưu và Bỏ qua
            btnLuu.Enabled = true;
            btnHuyBo.Enabled = true;
            btnChonAnh.Enabled = true;
            btnXoaAnh.Enabled = true;
        }
        // Sự kiện nút sửa
        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvHangHoa.SelectedRows.Count > 0)
            {
                // Đánh dấu là Cập nhật
                capNhat = true;
                mahh = txtMaHH.Text;
                LayDuLieu_HangHoa(cboLoai, "SELECT * FROM LOAIHH", "TENLOAI", "MALOAI");
                LayDuLieu_HangHoa(cboNhaCungCap, "SELECT * FROM NHACUNGCAP", "TENNCC", "MANCC");
                // Làm mờ nút Thêm mới, Sửa và Xóa
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;

                // Làm sáng nút Lưu và Bỏ qua
                btnLuu.Enabled = true;
                btnHuyBo.Enabled = true;
                btnChonAnh.Enabled = true;
                btnXoaAnh.Enabled = true;

                // làm sáng các trường nhập dữ liệu
                txtMaHH.Enabled = true;
                txtTenHH.Enabled = true;
                cboLoai.Enabled = true;
                numSoLuong.Enabled = true;
                numDGB.Enabled = true;
                NumDGN.Enabled = true;
                cboNhaCungCap.Enabled = true;
                txtMoTa.Enabled = true;
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 mặt hàng để Sửa thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Sự kiện nút Xóa
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvHangHoa.SelectedRows.Count > 0)
            {
                DialogResult kq;
                kq = MessageBox.Show("Bạn có muốn xóa Hàng hóa " + txtTenHH.Text + " không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (kq == DialogResult.Yes)
                {
                    string sql = @"DELETE FROM HANGHOA WHERE MAHH = @mhh";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@mhh", SqlDbType.NVarChar).Value = txtMaHH.Text;
                    dataHangHoa.Update(cmd);

                }

                // Tải lại form
                fHangHoa_Load(sender, e);
                XoaTrangTruongDuLieu();
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 mặt hàng để xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        // Sự kiện nút Hủy bỏ
        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            fHangHoa_Load(sender, e);
            XoaTrangTruongDuLieu();
        }
        // Sự kiện nút Lưu
        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu (Các trường không được rỗng, mã sách không trùng)
            if (txtMaHH.Text.Trim() == "")
                MessageBox.Show("Mã hàng hóa không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtTenHH.Text.Trim() == "")
                MessageBox.Show("Tên hàng hóa không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                bool error = false;
                DataGridViewRow currentRow = dgvHangHoa.CurrentRow;
                foreach (DataGridViewRow row in dgvHangHoa.Rows)
                {
                    if (capNhat && row == currentRow) continue;
                    if (row.Cells["MAHH"].Value.ToString() == txtMaHH.Text)
                        error = true;
                }

                if (error)
                    MessageBox.Show("Mã hàng hóa " + txtMaHH.Text + " đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // Lưu các thông tin trên các control vào CSDL
                    try
                    {
                        if (capNhat)
                        {
                            string sql1 = @"UPDATE   HANGHOA
                                           SET      MAHH = @mahh,
                                                    TENHH = @tenhh,
                                                    MALOAI = @maloai,
                                                    SOLUONG = @sl,
                                                    DONGIABAN = @dgb,
                                                    DONGIANHAP = @dgn,
                                                    MANCC = @ncc,
                                                    MOTA = @mt,
                                                    ANH = @anh
                                           WHERE    MAHH = @mahhcu";
                            string sql2 = @"UPDATE   HANGHOA
                                           SET      MAHH = @mahh,
                                                    TENHH = @tenhh,
                                                    MALOAI = @maloai,
                                                    SOLUONG = @sl,
                                                    DONGIABAN = @dgb,
                                                    DONGIANHAP = @dgn,
                                                    MANCC = @ncc,
                                                    MOTA = @mt,
                                                    ANH = NULL
                                           WHERE    MAHH = @mahhcu";
                            SqlCommand cmd;
                            if (pictHangHoa.Image != null)
                            {
                                cmd = new SqlCommand(sql1);
                                cmd.Parameters.Add("@mahh", SqlDbType.VarChar).Value = txtMaHH.Text;
                                cmd.Parameters.Add("@tenhh", SqlDbType.NVarChar).Value = txtTenHH.Text;
                                cmd.Parameters.Add("@maloai", SqlDbType.VarChar).Value = cboLoai.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@sl", SqlDbType.TinyInt).Value = numSoLuong.Text;
                                cmd.Parameters.Add("@dgb", SqlDbType.Money).Value = numDGB.Text;
                                cmd.Parameters.Add("@dgn", SqlDbType.Money).Value = NumDGN.Text;
                                cmd.Parameters.Add("@ncc", SqlDbType.VarChar).Value = cboNhaCungCap.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@mt", SqlDbType.NText).Value = txtMoTa.Text;
                                cmd.Parameters.AddWithValue("@anh", chuyenAnhthanhByte(pictHangHoa));
                                cmd.Parameters.Add("@mahhcu", SqlDbType.VarChar).Value = mahh;
                            }
                            else
                            {
                                cmd = new SqlCommand(sql2);
                                cmd.Parameters.Add("@mahh", SqlDbType.VarChar).Value = txtMaHH.Text;
                                cmd.Parameters.Add("@tenhh", SqlDbType.NVarChar).Value = txtTenHH.Text;
                                cmd.Parameters.Add("@maloai", SqlDbType.VarChar).Value = cboLoai.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@sl", SqlDbType.TinyInt).Value = numSoLuong.Text;
                                cmd.Parameters.Add("@dgb", SqlDbType.Money).Value = numDGB.Text;
                                cmd.Parameters.Add("@dgn", SqlDbType.Money).Value = NumDGN.Text;
                                cmd.Parameters.Add("@ncc", SqlDbType.VarChar).Value = cboNhaCungCap.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@mt", SqlDbType.NText).Value = txtMoTa.Text;
                                cmd.Parameters.Add("@mahhcu", SqlDbType.VarChar).Value = mahh;
                            }
                            dataHangHoa.Update(cmd);
                        }
                        else
                        {
                            string sql1 = @"INSERT INTO HANGHOA (MAHH, TENHH, MALOAI, SOLUONG, DONGIABAN, DONGIANHAP, MANCC, MOTA, ANH)
                                           VALUES(@mahh, @tenhh, @maloai, @sl, @dgb, @dgn, @ncc, @mt, @anh)";
                            string sql2 = @"INSERT INTO HANGHOA (MAHH, TENHH, MALOAI, SOLUONG, DONGIABAN, DONGIANHAP, MANCC, MOTA)
                                           VALUES(@mahh, @tenhh, @maloai, @sl, @dgb, @dgn, @ncc, @mt)";
                            SqlCommand cmd;
                            if (pictHangHoa.Image != null)
                            {
                                cmd = new SqlCommand(sql1);
                                cmd.Parameters.Add("@mahh", SqlDbType.VarChar).Value = txtMaHH.Text;
                                cmd.Parameters.Add("@tenhh", SqlDbType.NVarChar).Value = txtTenHH.Text;
                                cmd.Parameters.Add("@maloai", SqlDbType.VarChar).Value = cboLoai.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@sl", SqlDbType.TinyInt).Value = numSoLuong.Text;
                                cmd.Parameters.Add("@dgb", SqlDbType.Money).Value = numDGB.Text;
                                cmd.Parameters.Add("@dgn", SqlDbType.Money).Value = NumDGN.Text;
                                cmd.Parameters.Add("@ncc", SqlDbType.VarChar).Value = cboNhaCungCap.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@mt", SqlDbType.NText).Value = txtMoTa.Text;
                                cmd.Parameters.AddWithValue("@anh", chuyenAnhthanhByte(pictHangHoa));
                            }
                            else
                            {
                                cmd = new SqlCommand(sql2);
                                cmd.Parameters.Add("@mahh", SqlDbType.VarChar).Value = txtMaHH.Text;
                                cmd.Parameters.Add("@tenhh", SqlDbType.NVarChar).Value = txtTenHH.Text;
                                cmd.Parameters.Add("@maloai", SqlDbType.VarChar).Value = cboLoai.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@sl", SqlDbType.TinyInt).Value = numSoLuong.Text;
                                cmd.Parameters.Add("@dgb", SqlDbType.Money).Value = numDGB.Text;
                                cmd.Parameters.Add("@dgn", SqlDbType.Money).Value = NumDGN.Text;
                                cmd.Parameters.Add("@ncc", SqlDbType.VarChar).Value = cboNhaCungCap.SelectedValue.ToString(); ;
                                cmd.Parameters.Add("@mt", SqlDbType.NText).Value = txtMoTa.Text;
                            }
                            dataHangHoa.Update(cmd);
                        }

                        // Tải lại form
                        fHangHoa_Load(sender, e);
                        XoaTrangTruongDuLieu();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
        }
        #endregion
        #region Lấy dữ liệu
        // lấy dữ liệu nhân viên
        public void LayDuLieu_HangHoa()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM HANGHOA");
            dataHangHoa.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = dataHangHoa;
            dgvHangHoa.DataSource = binding;
        }
        // Lấy dữ liệu vào các ComboBox:
        public void LayDuLieu_HangHoa(ComboBox comboBox, string data, string display, string value)
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
        public void LayDuLieu_TimKiem(string TuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * 
                                            FROM HANGHOA
                                            WHERE MAHH LIKE N'%" + TuKhoa + "%'" +
                                            " or TENHH LIKE N'%" + TuKhoa + "%'" +
                                            " or SOLUONG LIKE N'%" + TuKhoa + "%'" +
                                            " or DONGIANHAP LIKE N'%" + TuKhoa + "%'" +
                                            " or DONGIABAN LIKE N'%" + TuKhoa + "%'" +
                                            " or MANCC LIKE N'%" + TuKhoa + "%'" +
                                            " or MOTA LIKE N'%" + TuKhoa + "%'");
            dataHangHoa.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = dataHangHoa;
            dgvHangHoa.DataSource = binding;
        }

        #endregion
        #region Xử lý ảnh

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LayDuLieu_TimKiem(txtTimKiem.Text);
        }

        private void btnChonAnh_Click(object sender, EventArgs e)
        {
            openFileDialog1.Multiselect = false;
            openFileDialog1.Filter = "JPG files (*.jpg)|*.jpg|png files(*.jpg)|*.jpg|ALL files(*.*)|*.*";
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                
                txtLocationIMG.Text = openFileDialog1.FileName;
                pictHangHoa.ImageLocation = txtLocationIMG.Text;
            }
        }

        private void btnXoaAnh_Click(object sender, EventArgs e)
        {
            pictHangHoa.Image = null;
        }
        // Chuyển ảnh sang dạng Byte
        private byte[] chuyenAnhthanhByte(PictureBox ptb)
        {
            MemoryStream ms = new MemoryStream();
            ptb.Image.Save(ms, ptb.Image.RawFormat);
            return ms.ToArray();
        }
        // Chuyển Byte sang dạng Ảnh
        private Image chuyenBytethanhImage(byte[] byteArr)
        {
            MemoryStream ms = new MemoryStream(byteArr);
            Image img = Image.FromStream(ms);
            return img;
        }
        #endregion

        private void numSoLuong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
                label11.Text = "";
            }
            else
            {
                label11.Text = "Số lượng chỉ được nhập giá trị là số!";
                e.Handled = true;
            }
        }

        private void numDGB_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
                label11.Text = "";
            }
            else
            {
                label11.Text = "Đơn giá bán chỉ được nhập giá trị là số!";
                e.Handled = true;
            }
        }

        private void NumDGN_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (((e.KeyChar >= '0') && (e.KeyChar <= '9')) || (Convert.ToInt32(e.KeyChar) == 8))
            {
                e.Handled = false;
                label12.Text = "";
            }
            else
            {
                label12.Text = "Đơn giá nhập chỉ được nhập giá trị là số!";
                e.Handled = true;
            }
        }
        private void XoaTrangTruongDuLieu()
        {
            // làm trống các trường nhập dữ liệu
            txtMaHH.Clear();
            txtTenHH.Clear();
            cboLoai.Text = "";
            numSoLuong.Value = 0;
            numDGB.Value = 0;
            NumDGN.Value = 0;
            cboNhaCungCap.Text = "";
            txtMoTa.Clear();
            pictHangHoa.Image = null;
        }
        private void dgvHangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Khi click vào dgvNhanVien thì hiển thị dữ liệu của dòng được chọn lên các control
            txtMaHH.DataBindings.Clear();
            txtTenHH.DataBindings.Clear();
            cboLoai.DataBindings.Clear();
            numSoLuong.DataBindings.Clear();
            numDGB.DataBindings.Clear();
            NumDGN.DataBindings.Clear();
            cboNhaCungCap.DataBindings.Clear();
            txtMoTa.DataBindings.Clear();
            pictHangHoa.DataBindings.Clear();

            txtMaHH.DataBindings.Add("Text", dgvHangHoa.DataSource, "MAHH", false, DataSourceUpdateMode.Never);
            txtTenHH.DataBindings.Add("Text", dgvHangHoa.DataSource, "TENHH", false, DataSourceUpdateMode.Never);
            cboLoai.DataBindings.Add("SelectedValue", dgvHangHoa.DataSource, "MALOAI", false, DataSourceUpdateMode.Never);
            numSoLuong.DataBindings.Add("Text", dgvHangHoa.DataSource, "SOLUONG", false, DataSourceUpdateMode.Never);
            numDGB.DataBindings.Add("Text", dgvHangHoa.DataSource, "DONGIABAN", false, DataSourceUpdateMode.Never);
            NumDGN.DataBindings.Add("Text", dgvHangHoa.DataSource, "DONGIANHAP", false, DataSourceUpdateMode.Never);
            cboNhaCungCap.DataBindings.Add("SelectedValue", dgvHangHoa.DataSource, "MANCC", false, DataSourceUpdateMode.Never);
            txtMoTa.DataBindings.Add("Text", dgvHangHoa.DataSource, "MOTA", false, DataSourceUpdateMode.Never);
            pictHangHoa.DataBindings.Add("Image", dgvHangHoa.DataSource, "ANH", true, DataSourceUpdateMode.Never);
        }

        private void fHangHoa_Shown(object sender, EventArgs e)
        {
            dgvHangHoa.ClearSelection();
        }

        private void load_Click(object sender, EventArgs e)
        {
            fHangHoa_Load(sender, e);
        }
    }
}
