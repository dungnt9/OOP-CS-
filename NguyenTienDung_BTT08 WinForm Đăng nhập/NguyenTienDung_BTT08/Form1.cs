using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenTienDung_BTT08
{
    public partial class frmDangNhap : Form
    {
        public frmDangNhap()
        {
            InitializeComponent();
        }
        private void label3_Click(object sender, EventArgs e) {}
        private void Form1_Load(object sender, EventArgs e) {}

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (txtTenDangNhap.Text=="Admin" && txtMatKhau.Text=="Admin")
            {
                this.Hide();
                MainForm f = new MainForm();
                f.ShowDialog();
                
            }
            else if (txtTenDangNhap.Text != "Admin")
            {
                MessageBox.Show("Sai tên đăng nhập", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (txtMatKhau.Text != "Admin")
            {
                MessageBox.Show("Sai mật khẩu", "Thông báo!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (txtMatKhau.PasswordChar != 0)
            {
                txtMatKhau.PasswordChar = (char)0;
            }
            else txtMatKhau.PasswordChar = '*';
        }

        private void txtMatKhau_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
