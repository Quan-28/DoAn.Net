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

namespace petStore
{
    public partial class fDangNhap : Form
    {
        public static string user;
        string pass;
        public fDangNhap()
        {
            InitializeComponent();
        }
        #region thao tác với form
        private void vbtnThoat_Click(object sender, EventArgs e)
        {
            DialogResult messagebox = MessageBox.Show("Bạn có chắc chắn muốn thoát?",
                "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (messagebox == DialogResult.Yes)
                this.Close();
        }
        private void vbnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void DangNhapMD(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Capture = false;

                const int WM_NCLBUTTONDOWN = 0x00A1;
                const int HTCAPTION = 2;
                Message msg =
                    Message.Create(this.Handle, WM_NCLBUTTONDOWN,
                        new IntPtr(HTCAPTION), IntPtr.Zero);
                this.DefWndProc(ref msg);
            }
        }
        private void picCloseEye_Click(object sender, EventArgs e)
        {
            if (txtPass.UseSystemPasswordChar == true)
            {
                picOpenEye.BringToFront();
                txtPass.UseSystemPasswordChar = false;
            }
        }
        private void picOpenEye_Click(object sender, EventArgs e)
        {
            if (txtPass.UseSystemPasswordChar == false)
            {
                picCloseEye.BringToFront();
                txtPass.UseSystemPasswordChar = true;
            }
        }
        private void vbtnNhapLai_Click(object sender, EventArgs e)
        {
            txtPass.Clear();
            txtUser.Clear();
            txtUser.Focus();
        }
        #endregion
        #region Đăng nhập
        private void vbtnDangnhap_Click(object sender, EventArgs e)
        {
            user = txtUser.Text;
            pass = txtPass.Text;
            if (txtUser.Text.Trim() == "")
            {
                DialogResult messagebox = MessageBox.Show("Tên đăng nhập không được bỏ trống!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (messagebox == DialogResult.OK)
                    txtUser.Focus();
            }
            else if (txtPass.Text.Trim() == "")
            {
                DialogResult messagebox = MessageBox.Show("Mật khẩu không được bỏ trống!",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (messagebox == DialogResult.OK)
                    txtPass.Focus();
            }
            else
            {
                if (Kiemtra(user, pass))
                {
                    MessageBox.Show("Đăng nhập thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    fQuanLyChinh f = new fQuanLyChinh();
                    this.Hide();
                    f.ShowDialog();
                    this.Show();
                    txtPass.Text = "";
                    txtUser.Focus();
                }
                else
                {
                    MessageBox.Show("Đăng nhập thất bại! Tài khoản hoặc mật khẩu không đúng. ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        #endregion
        public bool Kiemtra(string user, string pass)
        {
            ConnectData dataTable = new ConnectData();
            dataTable.OpenConnection();           
            string sql = "SELECT * FROM TAIKHOAN " +
                       "WHERE TENDANGNHAP = '" + user +
                       "'COLLATE SQL_Latin1_General_CP1_CS_AS" +
                       " and MATKHAU = '" + pass +
                       "'COLLATE SQL_Latin1_General_CP1_CS_AS";
            SqlCommand cmd = new SqlCommand(sql);
            dataTable.Fill(cmd);
            SqlDataReader read = cmd.ExecuteReader();
            if (read.Read())
                return true;
            else
                return false;
        }

        private void fDangNhap_Load(object sender, EventArgs e)
        {

        }
    }
}
