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
using System.Windows.Forms.VisualStyles;


namespace petStore
{
    public partial class fQuanLyChinh : Form
    {
        
        ConnectData dataTable = new ConnectData();
        
        public fQuanLyChinh()
        {
            FormChuongTrinh.fFlash flash = new FormChuongTrinh.fFlash();
            flash.ShowDialog();
            InitializeComponent();
        }
        #region Biến toàn cục
        public static string HovaTen = "";
        string userDisplay = fDangNhap.user;
        bool flag = true;
        bool maximum = false;
        FormChuongTrinh.fTaiKhoanVaNhanVien TKvaNV = null;
        FormChuongTrinh.fHangHoa hanghoa = null;
        FormChuongTrinh.fLoaiHangHoa loaihanghoa = null;
        FormChuongTrinh.fKhachHang khachhang = null;
        FormChuongTrinh.fNhaCungCap nhacungcap = null;

        FormChuongTrinh.fShowLoaiHH showloaihh = null;
        FormChuongTrinh.fShowNhaCungCap showncc = null;
        FormChuongTrinh.fShowNhanVien shownv = null;
        FormChuongTrinh.fShowHoaDonBan showhdb = null;
        FormChuongTrinh.fShowHangHoa showhh = null;

        FormChuongTrinh.fHoaDonBan HDban = null;
        #endregion
        private void timer1_Tick(object sender, EventArgs e)
        {
            label1.Text = DateTime.Now.ToString("T");
        }
        private void fQuanLyChinh_Load(object sender, EventArgs e)
        {
            DangNhap();
            timer1.Start();
            pnlLeft.Width = 250;
        }

        #region Đăng nhập và Đăng xuất
        public void QuanTriVien()
        {
            label2.Text = "Quản trị viên: \n( " + HovaTen + " )";
        }
        public void NhanVien()
        {
            mnuAdmin.Visible = false;
            mnuHangHoa.Enabled = false;
            tsbtnHangHoa.Enabled = false;
            label2.Text = "Nhân viên: \n( " + HovaTen + " )";
        }
        byte[] text;
        private void DangNhap()
        {
            dataTable.OpenConnection();
            SqlCommand cmd = new SqlCommand("SELECT * FROM TAIKHOAN tk, NHANVIEN nv WHERE tk.TENDANGNHAP = @TDN and tk.MANV = nv.MANV");
            cmd.Parameters.Add("@TDN", SqlDbType.VarChar).Value = userDisplay;
            dataTable.Fill(cmd);
            HovaTen = dataTable.Rows[0]["TENNV"].ToString();
            if (dataTable.Rows[0]["ANH"].ToString() != "")
            {
                text = (byte[])dataTable.Rows[0]["ANH"];
                pictureBox1.Image = chuyenBytethanhImage(text);
            }
            string quyenHan = dataTable.Rows[0]["QUYENHAN"].ToString();
                if (quyenHan == "ad")
                    QuanTriVien();
                else if (quyenHan == "nv")
                    NhanVien();
        }
        //button Đăng xuất
        private void vbtnDangXuat_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
        #region Thao tác với Menu admin
        private void mnuNhanVien_Click(object sender, EventArgs e)
        {
            TKvaNV = new FormChuongTrinh.fTaiKhoanVaNhanVien();
            TabCreating(tabControl1, "Thông tin nhân viên", TKvaNV);
           
        }
        private void mnuLoaiHangHoa_Click(object sender, EventArgs e)
        {
            loaihanghoa = new FormChuongTrinh.fLoaiHangHoa();
            TabCreating(tabControl1, "Thêm loại hàng hóa", loaihanghoa);
            
        }
        // Chuyển Byte sang dạng Ảnh
        private Image chuyenBytethanhImage(byte[] byteArr)
        {
            MemoryStream ms = new MemoryStream(byteArr);
            Image img = Image.FromStream(ms);
            return img;
        }
        #endregion
        #region Thao tác mới Menu Danh mục
        private void mnuHangHoa_Click(object sender, EventArgs e)
        {
            hanghoa = new FormChuongTrinh.fHangHoa();
            TabCreating(tabControl1, "Thông tin hàng hóa", hanghoa);
           
        }
        private void mnuThemKhachHang_Click(object sender, EventArgs e)
        {
            khachhang = new FormChuongTrinh.fKhachHang();
            TabCreating(tabControl1, "Thêm khách hàng", khachhang);
            
        }

        private void mnuThemNhaCungCap_Click(object sender, EventArgs e)
        {
            nhacungcap = new FormChuongTrinh.fNhaCungCap();
            TabCreating(tabControl1, "Thêm nhà cung cấp", nhacungcap);
           
        }
        private void mnuBanHang_Click(object sender, EventArgs e)
        {
            HDban = new FormChuongTrinh.fHoaDonBan();
            TabCreating(tabControl1, "Lập HD bán", HDban);
        }
        #endregion
        #region Thao tác với Menu Tra cứu
        private void mnuShowLoaiHH_Click(object sender, EventArgs e)
        {
            showloaihh = new FormChuongTrinh.fShowLoaiHH();
            TabCreating(tabControl1, "Tra cứu Loại hàng hóa", showloaihh);
        }
        private void mnuShowNCC_Click(object sender, EventArgs e)
        {
            showncc = new FormChuongTrinh.fShowNhaCungCap();
            TabCreating(tabControl1, "Tra cứu Nhà cung cấp", showncc);
        }
        private void mnuShowNhanVien_Click(object sender, EventArgs e)
        {
            shownv = new FormChuongTrinh.fShowNhanVien();
            TabCreating(tabControl1, "Tra cứu Nhân viên", shownv);
        }
        private void mnuShowHDB_Click(object sender, EventArgs e)
        {
            showhdb = new FormChuongTrinh.fShowHoaDonBan();
            TabCreating(tabControl1, "Tra cứu Hóa đơn bán", showhdb);
        }
        private void mnuShowHangHoa_Click(object sender, EventArgs e)
        {
            showhh = new FormChuongTrinh.fShowHangHoa();
            TabCreating(tabControl1, "Tra cứu Hàng hóa", showhh);
        }
        #endregion
        #region Thao tác với Menu Trợ giúp
        private void mnuThongTinPM_Click(object sender, EventArgs e)
        {
            
          
        }
        #endregion
        #region Thao tác với Báo cáo thống
        private void mnuThongKe_Click(object sender, EventArgs e)
        {
            
        }
        #endregion
        #region Thao tác với Form
        // button ẩn hiện panel Trái
        private void btnPnlShowHide_Click(object sender, EventArgs e)
        {
            if (!flag)
            {
                pnlLeft.Width = 250;
                monthCalendar1.Visible = true;
            }
            else
            {
                pnlLeft.Width = 50;
                monthCalendar1.Visible = false;
            }
            flag = !flag;
        }
        //button Thoát Form
        private void vbtnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        //button Maximum Form
        private void vbtnMaximum_Click(object sender, EventArgs e)
        {
            if (!maximum)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
            maximum = !maximum;
        }
        //button Minimize Form
        private void vbtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        #endregion
        #region tabcontrol
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            Graphics g = e.Graphics;
            Font drawFont = new Font("Arial", 10, FontStyle.Bold);
            g.FillRectangle(new SolidBrush(Color.PeachPuff), e.Bounds);
            e.Graphics.DrawString("x", drawFont, Brushes.DarkSlateGray, e.Bounds.Right - 15, e.Bounds.Top + 4);
            e.Graphics.DrawString(this.tabControl1.TabPages[e.Index].Text, e.Font, Brushes.DarkCyan, e.Bounds.Left + 1, e.Bounds.Top + 4);
            e.DrawFocusRectangle();
        }

        private void tabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            for (int i = 0; i < this.tabControl1.TabPages.Count; i++)
            {
                Rectangle r = tabControl1.GetTabRect(i);
                //Lấy tọa độ cho X
                Rectangle closeButton = new Rectangle(r.Right - 12, r.Top + 4, 9, 7);
                if (closeButton.Contains(e.Location))
                {
                    //if (MessageBox.Show("Would you like to Close this Tab ?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    //{
                        this.tabControl1.TabPages.RemoveAt(i);
                        break;
                    //}
                }
            }
        }
        static int KiemTraTonTai(TabControl TabControlName, string TabName)
        {
            int temp = -1;
            for (int i = 0; i < TabControlName.TabPages.Count; i++)
            {
                if (TabControlName.TabPages[i].Text == TabName)
                {
                    temp = i;
                    break;
                }
            }
            return temp;
        }
        public void TabCreating(TabControl TabControl, string Text, Form Form)
        {
            int Index = KiemTraTonTai(TabControl, Text);
            if (Index >= 0)
            {
                TabControl.SelectedTab = TabControl.TabPages[Index];
                TabControl.SelectedTab.Text = Text;
            }
            else
            {
                TabPage TabPage = new TabPage { Text = Text };
                TabControl.TabPages.Add(TabPage);
                TabControl.SelectedTab = TabPage;
                Form.TopLevel = false;
                Form.Parent = TabPage;
                //  Form.MdiParent = this;
                Form.Show();
                Form.Dock = DockStyle.Fill;
            }
        }

        #endregion
        #region toolstripbutton
        private void tsbtnBanHang_Click(object sender, EventArgs e)
        {
            mnuBanHang_Click(sender, e);
        }

        private void tsbtnHangHoa_Click(object sender, EventArgs e)
        {
            mnuHangHoa_Click(sender, e);
        }

        private void tsbtnThemKhachHang_Click(object sender, EventArgs e)
        {
            mnuThemKhachHang_Click(sender, e);
        }

        private void tsbtnThemNhaCungCap_Click(object sender, EventArgs e)
        {
            mnuThemNhaCungCap_Click(sender, e);
        }

        private void tsbtnShowHoaDon_Click(object sender, EventArgs e)
        {
            mnuShowHDB_Click(sender, e);
        }

        private void tsbtnThongKe_Click(object sender, EventArgs e)
        {
            mnuThongKe_Click(sender, e);
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
