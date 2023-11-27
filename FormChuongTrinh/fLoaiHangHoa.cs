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
    public partial class fLoaiHangHoa : Form
    {
        // Khai báo biến toàn cục
        ConnectData datahanghoa = new ConnectData();
        bool capNhat = false;
        string maloai = "";
        public fLoaiHangHoa()
        {
            InitializeComponent();
        }
        // Form load
        private void fLoaiHangHoa_Load(object sender, EventArgs e)
        {
            // kết nối với csdl
            datahanghoa.OpenConnection();
            // Đổ dữ liệu lên datagridview
            LayDuLieu_HangHoa();
            // format lại datagridview khi chưa selection
            dgvLoaiHangHoa.ClearSelection();
            // Việt hóa tiêu đề dgvNhanVien
            dgvLoaiHangHoa.Columns["MALOAI"].HeaderText = "MÃ LOẠI HÀNG";
            dgvLoaiHangHoa.Columns["TENLOAI"].HeaderText = "TÊN LOẠI HÀNG";
            // Làm sáng nút Thêm mới, Sửa và Xóa
            btnThem.Enabled = true;
            btnSua.Enabled = true;
            btnXoa.Enabled = true;
            //Làm mờ nút lưu và bỏ qua
            btnLuu.Enabled = false;
            btnHuyBo.Enabled = false;
            // làm mờ các trường nhập dữ liệu
            txtMaLoai.Enabled = false;
            txtTenLoai.Enabled = false;
        }
        #region Lấy dữ liệu
        public void LayDuLieu_HangHoa()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LOAIHH");
            datahanghoa.Fill(cmd);
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = datahanghoa;
            dgvLoaiHangHoa.DataSource = binding1;
        }
        // Lấy dữ liệu theo từ khóa
        public void LayDuLieu_TimKiem(string TuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * 
                                            FROM LOAIHH
                                            WHERE MALOAI LIKE N'%" + TuKhoa + "%'" +
                                            " or TENLOAI LIKE N'%" + TuKhoa + "%'");
            datahanghoa.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = datahanghoa;
            dgvLoaiHangHoa.DataSource = binding;
        }
        #endregion

        #region sự kiện các nút của dgvLoaiHangHoa
        private void btnThem_Click(object sender, EventArgs e)
        {
            capNhat = false;
            XoaTrangTruongDuLieu();
            txtMaLoai.Focus();
            // làm mờ nút Thêm, Sửa, Xóa
            btnThem.Enabled = false;
            btnSua.Enabled = false;
            btnXoa.Enabled = false;
            // làm sáng các trường nhập dữ liệu
            txtMaLoai.Enabled = true;
            txtTenLoai.Enabled = true;
            //làm sáng nút Lưu và Bỏ qua
            btnLuu.Enabled = true;
            btnHuyBo.Enabled = true;
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if (dgvLoaiHangHoa.SelectedRows.Count > 0)
            {
                // Đánh dấu là Cập nhật
                capNhat = true;
                maloai = txtMaLoai.Text;

                // Làm mờ nút Thêm mới, Sửa và Xóa
                btnThem.Enabled = false;
                btnSua.Enabled = false;
                btnXoa.Enabled = false;

                // Làm sáng nút Lưu và Bỏ qua
                btnLuu.Enabled = true;
                btnHuyBo.Enabled = true;

                // làm sáng các trường nhập dữ liệu
                txtMaLoai.Enabled = true;
                txtTenLoai.Enabled = true;
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 loại hàng hóa để sửa thông tin", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvLoaiHangHoa.SelectedRows.Count > 0)
            {
                    DialogResult kq;
                kq = MessageBox.Show("Bạn có muốn xóa Loại hàng tên là" + txtTenLoai.Text + " không?", "Xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (kq == DialogResult.Yes)
                {
                    string sql = @"DELETE FROM LOAIHH WHERE MALOAI = @ml";
                    SqlCommand cmd = new SqlCommand(sql);
                    cmd.Parameters.Add("@ml", SqlDbType.NVarChar).Value = txtMaLoai.Text;
                    datahanghoa.Update(cmd);

                }

                // Tải lại form
                fLoaiHangHoa_Load(sender, e);
                XoaTrangTruongDuLieu();
            }
            else
            {
                DialogResult kq1;
                kq1 = MessageBox.Show("Bạn cần chọn 1 loại hàng hóa để Xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
}

        private void btnHuyBo_Click(object sender, EventArgs e)
        {
            fLoaiHangHoa_Load(sender, e);
            XoaTrangTruongDuLieu();
        }

        private void btnLuu_Click(object sender, EventArgs e)
        {
            // Kiểm tra dữ liệu (Các trường không được rỗng, mã sách không trùng)
            if (txtMaLoai.Text.Trim() == "")
                MessageBox.Show("Mã loại hàng không được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (txtTenLoai.Text.Trim() == "")
                MessageBox.Show("Tên loại hàng được bỏ trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                bool error = false;
                DataGridViewRow currentRow = dgvLoaiHangHoa.CurrentRow;
                foreach (DataGridViewRow row in dgvLoaiHangHoa.Rows)
                {
                    if (capNhat && row == currentRow) continue;
                    if (row.Cells["MALOAI"].Value.ToString() == txtMaLoai.Text)
                        error = true;
                }

                if (error)
                    MessageBox.Show("Mã loại hàng " + txtMaLoai.Text + " đã tồn tại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                {
                    // Lưu các thông tin trên các control vào CSDL
                    try
                    {
                        if (capNhat)
                        {
                            string sql = @"UPDATE   LOAIHH
                                           SET      MALOAI = @ml,
                                                    TENLOAI = @tl
                                           WHERE    MALOAI = @mlcu";
                            SqlCommand cmd = new SqlCommand(sql);
                            cmd.Parameters.Add("@ml", SqlDbType.VarChar).Value = txtMaLoai.Text;
                            cmd.Parameters.Add("@tl", SqlDbType.NVarChar).Value = txtTenLoai.Text;
                            cmd.Parameters.Add("@mlcu", SqlDbType.VarChar).Value = maloai;
                            datahanghoa.Update(cmd);
                        }
                        else
                        {
                            string sql = @"INSERT INTO LOAIHH
                                           VALUES(@ml, @tl)";
                            SqlCommand cmd = new SqlCommand(sql);
                            cmd.Parameters.Add("@ml", SqlDbType.VarChar).Value = txtMaLoai.Text;
                            cmd.Parameters.Add("@tl", SqlDbType.NVarChar).Value = txtTenLoai.Text;
                            datahanghoa.Update(cmd);
                        }

                        // Tải lại form
                        fLoaiHangHoa_Load(sender, e);
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

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LayDuLieu_TimKiem(txtTimKiem.Text);
        }

        private void fLoaiHangHoa_Shown(object sender, EventArgs e)
        {
            dgvLoaiHangHoa.ClearSelection();
        }
        private void XoaTrangTruongDuLieu()
        {
            // làm trống các trường nhập dữ liệu
            txtMaLoai.Clear();
            txtTenLoai.Clear();
        }
        private void dgvLoaiHangHoa_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtMaLoai.DataBindings.Clear();
            txtTenLoai.DataBindings.Clear();
            // Khi click vào dgvNhanVien thì hiển thị dữ liệu của dòng được chọn lên các control
            txtMaLoai.DataBindings.Add("Text", dgvLoaiHangHoa.DataSource, "MALOAI", false, DataSourceUpdateMode.Never);
            txtTenLoai.DataBindings.Add("Text", dgvLoaiHangHoa.DataSource, "TENLOAI", false, DataSourceUpdateMode.Never);
        }
    }
}
