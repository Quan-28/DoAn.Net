using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace petStore.FormChuongTrinh
{
    public partial class fFlash : Form
    {
        public fFlash()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            LoadingPanel.Width += 15;
            if(LoadingPanel.Width >= 300)
            {
                timer1.Stop();
                this.Close();
            }
        }

        private void fFlash_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void LoadingPanel_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
