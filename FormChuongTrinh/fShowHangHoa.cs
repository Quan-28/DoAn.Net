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
    public partial class fShowHangHoa : Form
    {
        ConnectData data = new ConnectData();
        public fShowHangHoa()
        {
            InitializeComponent();
        }

        private void fShowHangHoa_Load(object sender, EventArgs e)
        {
            // kết nối với csdl
            data.OpenConnection();
            // Đổ dữ liệu lên datagridview
            LayDuLieu_HangHoa();
            // Việt hóa tiêu đề dgvNhanVien
            dgvHangHoa.Columns["MAHH"].HeaderText = "MÃ HÀNG HÓA";
            dgvHangHoa.Columns["TENHH"].HeaderText = "TÊN HÀNG HÓA";
            dgvHangHoa.Columns["MALOAI"].HeaderText = "LOẠI HÀNG HÓA";
            dgvHangHoa.Columns["SOLUONG"].HeaderText = "SỐ LƯỢNG";
            dgvHangHoa.Columns["DONGIABAN"].HeaderText = "ĐƠN GIÁ BÁN";
            dgvHangHoa.Columns["DONGIANHAP"].HeaderText = "ĐƠN GIÁ NHẬP";
            dgvHangHoa.Columns["MANCC"].HeaderText = "NHÀ CUNG CẤP";
            dgvHangHoa.Columns["MOTA"].HeaderText = "MÔ TẢ";
            dgvHangHoa.Columns["ANH"].HeaderText = "ẢNH";
            // điều chỉnh lại cột ảnh trong datagridview
            DataGridViewImageColumn pic = new DataGridViewImageColumn();
            pic = (DataGridViewImageColumn)dgvHangHoa.Columns["ANH"];
            pic.ImageLayout = DataGridViewImageCellLayout.Zoom;
        }
        // lấy dữ liệu nhân viên
        public void LayDuLieu_HangHoa()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM HANGHOA");
            data.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = data;
            dgvHangHoa.DataSource = binding;
        }
        public void LayDuLieu_TimKiem(string TuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * 
                                            FROM HANGHOA
                                            WHERE MAHH LIKE N'%" + TuKhoa + "%'" +
                                            " or TENHH LIKE N'%" + TuKhoa + "%'" +
                                            " or SOLUONG LIKE N'%" + TuKhoa + "%'" +
                                            " or DONGIABAN LIKE N'%" + TuKhoa + "%'" +
                                            " or DONGIANHAP LIKE N'%" + TuKhoa + "%'" +
                                            " or MANCC LIKE N'%" + TuKhoa + "%'" +
                                            " or MOTA LIKE N'%" + TuKhoa + "%'" +
                                            " or MALOAI LIKE N'%" + TuKhoa + "%'");
            data.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = data;
            dgvHangHoa.DataSource = binding;
            bindingNavigator1.BindingSource = binding;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LayDuLieu_TimKiem(txtTimKiem.Text);
        }
    }
}
