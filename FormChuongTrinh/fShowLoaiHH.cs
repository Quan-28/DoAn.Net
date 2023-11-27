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
    public partial class fShowLoaiHH : Form
    {
        // Khai báo biến toàn cục
        ConnectData datahanghoa = new ConnectData();
        public fShowLoaiHH()
        {
            InitializeComponent();
        }

        private void fShowLoaiHH_Load(object sender, EventArgs e)
        {
            // kết nối với csdl
            datahanghoa.OpenConnection();
            // Đổ dữ liệu lên datagridview
            LayDuLieu_HangHoa();
            // Việt hóa tiêu đề dgvNhanVien
            dgvLoaiHH.Columns["MALOAI"].HeaderText = "MÃ LOẠI HÀNG";
            dgvLoaiHH.Columns["TENLOAI"].HeaderText = "TÊN LOẠI HÀNG";
        }
        public void LayDuLieu_HangHoa()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM LOAIHH");
            datahanghoa.Fill(cmd);
            BindingSource binding1 = new BindingSource();
            binding1.DataSource = datahanghoa;
            dgvLoaiHH.DataSource = binding1;
            bindingNavigator1.BindingSource = binding1;
        }
        public void LayDuLieu_TimKiem(string TuKhoa)
        {
            SqlCommand cmd = new SqlCommand(@"SELECT * 
                                            FROM LOAIHH
                                            WHERE MALOAI LIKE N'%" + TuKhoa + "%'" +
                                            " or TENLOAI LIKE N'%" + TuKhoa + "%'");
            datahanghoa.Fill(cmd);
            BindingSource binding = new BindingSource();
            binding.DataSource = datahanghoa;
            dgvLoaiHH.DataSource = binding;
            bindingNavigator1.BindingSource = binding;
        }

        private void btnTimKiem_Click(object sender, EventArgs e)
        {
            LayDuLieu_TimKiem(txtTimKiem.Text);
        }

    }
}
