/*using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MSPaint
{
    public partial class Property : Form
    {
        private const int standardwidth = 480;
        private const int standardheight = 331;
        private int x;
        private int y;
        public double width;
        public double height;
        public bool colorful = true;

        public Property()
        {
            InitializeComponent();
        }

        public Property(int width, int height)
        {
            InitializeComponent();
            textBox1.Text = width.ToString();
            textBox2.Text = height.ToString();
            this.width = width;
            this.height = height;
            x = Convert.ToInt32(Math.Round(standardwidth / Accessory.PixelToInchH(standardwidth)));
            y = Convert.ToInt32(Math.Round(standardheight / Accessory.PixelToInchV(standardheight)));
            label8.Text = x + " x " + y + " dots"+"Per inch ";
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = width.ToString();
            textBox2.Text = height.ToString();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = (width * 1.0 / x * 2.45).ToString("0.00");
            textBox2.Text = (height * 1.0 / y * 2.45).ToString("0.00");
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            textBox1.Text = (width * 1.0 / x).ToString("0.00");
            textBox2.Text = (height * 1.0 / y).ToString("0.00");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Dispose();            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox1.Text = standardwidth.ToString();
            textBox2.Text = standardheight.ToString();
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            colorful = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            colorful = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            bool dot = false;
            int num1 = 0;
            int num2 = 0;
            foreach (char ch in textBox1.Text)
            {
                if (ch >= '0' && ch <= '9')
                {
                    if (!dot)
                    {
                        num1 = num1 * 10 + ch;
                    }
                    else
                    {
                        num2 = num2 * 10 + ch;
                    }
                }
                else if (ch == '.')
                {
                    if (dot)
                        break;
                    dot = true;
                }
                else
                    break;
            }
            width = num1;
            if (num2 != 0)
                width = double.Parse(num1 + "." + num2);
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            bool dot = false;
            int num1 = 0;
            int num2 = 0;
            foreach (char ch in textBox2.Text)
            {
                if (ch >= '0' && ch <= '9')
                {
                    if (!dot)
                    {
                        num1 = num1 * 10 + ch;
                    }
                    else
                    {
                        num2 = num2 * 10 + ch;
                    }
                }
                else if (ch == '.')
                {
                    if (dot)
                        break;
                    dot = true;
                }
                else
                    break;
            }
            height = num1;
            if (num2 != 0)
                height = double.Parse(num1 + "." + num2);
        }
    }
}
*/