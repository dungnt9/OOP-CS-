using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace MSPaint
{
    public partial class Colors : UserControl
    {
        public Colors()
        {
            InitializeComponent();
            Bitmap bitmap = DrawShape.CreateBitmap(this.bigpb.Size.Width, this.bigpb.Size.Height, Color.White);
            this.bigpb.Image = DrawShape.DrawSquareEx(bitmap, 1, this.bigpb.BackColor); ;
        }

        private bool changecolor = false;

        //Change the color of the commission defined
        public delegate void ColorChangeEventHandler(object sender, EventArgs e);

        //Custom color change event
        public event ColorChangeEventHandler ColorChange;

        //Defined event handling
        protected virtual void OnColorChange(EventArgs e)
        {
            if (ColorChange != null) ColorChange(this, e);
        }

        /// <summary>
        /// Pen color
        /// </summary>
        public Color PenColor
        {
            get { return this.foreground.BackColor; }
            set { this.foreground.BackColor = value; }
        }

        /// <summary>
        /// Brush color
        /// </summary>
        public Color BrushColor
        {
            get { return this.background.BackColor; }
            set { this.background.BackColor = value; }
        }

        /// <summary>
        /// Click the controls change the color of the event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ColorChange_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                foreground.BackColor = ((PictureBox)sender).BackColor;
            else if(e.Button==MouseButtons.Right)
                background.BackColor = ((PictureBox)sender).BackColor;
            OnColorChange(new EventArgs());
        }

        private void ColorChange_DoubleClick(object sender, EventArgs e)
        {
            ShowColor();
            if (changecolor)
            {
                ((PictureBox)sender).BackColor = colorDialog1.Color;
                changecolor = false;
            }
        }

        /// <summary>
        /// Replacement pen color
        /// </summary>
        public void ShowColor()
        {
            colorDialog1.Color = PenColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.PenColor = colorDialog1.Color;
                changecolor = true;
            }
        }
    }
}
