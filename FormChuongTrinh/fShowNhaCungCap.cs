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
    public partial class fShowNhaCungCap : Form
    {
        // Khai báo biến toàn cục
        ConnectData datanhacungcap = new ConnectData();
        public fShowNhaCungCap()
        {
            InitializeComponent();
        }

        private void fShowNhaCungCap_Load(object sender, EventArgs e)
        {
            // kết nối với csdl
            datanhacungcap.OpenConnection();
            // Đổ dữ liệu lên datagridview
            LayDuLieu_NhaCungCap();
            // Việt hóa tiêu đề dgvKhachHang
            dgvNhaCungCap.Columns["MANCC"].HeaderText = "MÃ NHÀ CUNG CẤP";
            dgvNhaCungCap.Columns["TENNCC"].HeaderText = "TÊN NHÀ CUNG CẤP";
            dgvNhaCungCap.Columns["DIACHI"].HeaderText = "ĐỊA CHỈ";
            dgvNhaCungCap.Columns["SDT"].HeaderText = "SỐ ĐIỆN THOẠI";
        }
        public void LayDuLieu_NhaCungCap()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM NHACUNGCAP");
            datanhacungcap.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = datanhacungcap;
            dgvNhaCungCap.DataSource = binding;
            bindingNavigator1.BindingSource = binding;
        }
        public void LayDuLieu_TimKiem(string TuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * 
                                            FROM NHACUNGCAP
                                            WHERE MANCC LIKE N'%" + TuKhoa + "%'" +
                                           " or TENNCC LIKE N'%" + TuKhoa + "%'" +
                                           " or SDT LIKE N'%" + TuKhoa + "%'" +
                                           " or DIACHI LIKE N'%" + TuKhoa + "%'");
            datanhacungcap.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = datanhacungcap;
            dgvNhaCungCap.DataSource = binding;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LayDuLieu_TimKiem(txtTimKiem.Text);
        }
    }
}
