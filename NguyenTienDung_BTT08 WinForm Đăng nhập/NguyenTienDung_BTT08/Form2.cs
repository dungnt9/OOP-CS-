using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NguyenTienDung_BTT08
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "Họ Tên: " + textBox1.Text + "\n"
            + "Quê Quán: " + comboBox1.SelectedItem + "\n"
            + "Ngày Sinh: " + dateTimePicker1.Text + "\n"
            + "Giới Tính: ";
            if (Nam.Checked) { richTextBox1.Text += Nam.Text; }
            else if (Nữ.Checked) { richTextBox1.Text += Nữ.Text; }
            richTextBox1.Text += "\n";

            richTextBox1.Text += "Free time: ";
            if (checkBox1.Checked) { richTextBox1.Text += checkBox1.Text + ", "; };
            if (checkBox2.Checked) { richTextBox1.Text += checkBox2.Text + ", "; }
            if (checkBox3.Checked) { richTextBox1.Text += checkBox3.Text + ", "; }
            richTextBox1.Text += "\n";

            string thethao ="";
            foreach(string a in this.lstthethao.SelectedItems)
            {
                thethao += a + " - ";
            }
            
            string doc = "";
            foreach (string a in this.lstdoc.CheckedItems)
            {
                doc += a + " - " ;
            }
            richTextBox1.Text += "Thể Thao: " + thethao + "\n" + "Đọc: " + lstdoc.Text;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void lstdoc_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
