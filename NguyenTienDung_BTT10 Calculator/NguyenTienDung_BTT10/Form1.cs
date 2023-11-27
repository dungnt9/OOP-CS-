using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace NguyenTienDung_BTT10
{
    public partial class frmCal : Form
    {
        public frmCal()
        {
            InitializeComponent();
        }

        private void frmCal_Load(object sender, EventArgs e)
        {

        }

        string _show = "";
        string _operator = "";
        double _val1 = 0;
        double _val2 = 0;

        private void btnNum_Click(object sender, EventArgs e)
        {
            Show(((System.Windows.Forms.Button)sender).Text);
        }

        private void Show(string s)
        {
            _show += s;
            txtShow.Text = _show;
        }

        private void txtShow_TextChanged(object sender, EventArgs e)
        {
            //double _val = double.Parse(txtShow.Text);
            //txtShow.Text = string.Format("{0:N0}", _val);
        }




        private void btnSign_Click(object sender, EventArgs e)
        {
            double _val = double.Parse(_show);
            _val = -_val;
            txtShow.Text = _val.ToString();
        }

        private void btnSqrt_Click(object sender, EventArgs e)
        {
            double a = double.Parse(_show);
            txtShow.Text = Math.Sqrt(a).ToString();
            _show = txtShow.Text;
        }

        private void btnSqr_Click(object sender, EventArgs e)
        {
            double a = double.Parse(_show);
            txtShow.Text = Math.Pow(a, 2).ToString();
            _show = txtShow.Text;
        }

        private void btnInverse_Click(object sender, EventArgs e)
        {
            double a = double.Parse(txtShow.Text);
            txtShow.Text = (1/a).ToString();
            _show = txtShow.Text;
        }

        private void btnPercent_Click(object sender, EventArgs e)
        {
            double value = double.Parse(_show);
            double result = value / 100;

            txtShow.Text = result.ToString();
            _show = result.ToString();
        }




        private void btnPlus_Click(object sender, EventArgs e)
        {
            PerformOperation("+");
        }

        private void btnSubstract_Click(object sender, EventArgs e)
        {
            PerformOperation("-");
        }

        private void btnMultiply_Click(object sender, EventArgs e)
        {
            PerformOperation("*");
        }

        private void btnDivide_Click(object sender, EventArgs e)
        {
            PerformOperation("/");
        }



        private void btnEqual_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(_show) && !string.IsNullOrEmpty(_operator))
            {
                _val2 = double.Parse(_show);

                switch (_operator)
                {
                    case "+":
                        _show = (_val1 + _val2).ToString();
                        break;
                    case "-":
                        _show = (_val1 - _val2).ToString();
                        break;
                    case "*":
                        _show = (_val1 * _val2).ToString();
                        break;
                    case "/":
                        if (_val2 != 0)
                            _show = (_val1 / _val2).ToString();
                        else
                            _show = "Error";
                        break;
                }

                txtShow.Text = _show.ToString();

            }
        }

        private void PerformOperation(string op)
        {
            _val1 = double.Parse(_show);
            _operator = op;
            _show = "";
        }

        private void btnDot_Click(object sender, EventArgs e)
        {
            if (!_show.Contains("."))
            {
                _show += ".";
                txtShow.Text = _show;
            }
        }

        private void btnCE_Click(object sender, EventArgs e)
        {
            _show = "";
            txtShow.Text = "0";
        }

        private void btnC_Click_1(object sender, EventArgs e)
        {
            _show = "";
            txtShow.Text = "0";
            _val1 = 0;
            _val2 = 0;
            _operator = "";
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            RemoveDigitFromRight();
        }

        private void RemoveDigitFromRight()
        {
            _show = _show.Substring(0, _show.Length - 1);

            txtShow.Text = _show;

            if (string.IsNullOrEmpty(_show))
            {
                txtShow.Text = "0";
            }
        }
    }

}