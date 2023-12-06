using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace MSPaint
{
    public partial class SaveDialog : Form
    {
        PaintForm psform;
        public bool saveclick = false;
        
        public SaveDialog()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (!psform.canvas1.ds.saved)
                {
                    psform.menuItem10_Click(null, null);
                }
                else
                {
                    psform.menuItem9_Click(null, null);
                }
            }
            catch { }
            if (saveclick)
                this.DialogResult = DialogResult.OK;
            else
                this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SaveDialog_Load(object sender, EventArgs e)
        {
            psform = (PaintForm)this.Owner;
            if (psform.Text == "untitled - Paint")
            {
                label2.Text = "Untitled?";
            }
            else
            {
                this.Size = new Size(354, 150);
                panel1.Size = new Size(348,78);
                button1.Location = new Point(button1.Location.X, button1.Location.Y + 20);
                button2.Location = new Point(button2.Location.X, button2.Location.Y + 20);
                button3.Location = new Point(button3.Location.X, button3.Location.Y + 20);
                label2.Location = new Point(label1.Location.X, label1.Location.Y + 20);
                if (psform.saveFileDialog1.FileName != "")
                    label2.Text = psform.saveFileDialog1.FileName + "?";
                else label2.Text = psform.openFileDialog1.FileName + "?";
            }
        }
    }
}
