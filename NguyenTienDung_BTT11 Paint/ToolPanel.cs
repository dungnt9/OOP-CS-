using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;

namespace MSPaint
{
    public partial class ToolPanel : UserControl
    {
        private PictureBox prepic;  //PictureBox last selected tool
        public PaintForm psform;  //The main form (main form used to obtain the DrawShape objects)
        private Rectangle selrc = new Rectangle(0, 0, 48, 11);
        private int drawtype = 0;  //0 - select, 1 - Rubber, 2 - empty 3 - Magnifier, 4 - brush, 5 - gun, 6 - Geometry
        private Point selpt1 = new Point();  //Select starting point shadow
        private Point selpt2 = new Point(15, 31);  //Select starting point rubber
        
        private Point selpt4 = new Point(2, 1);  //Select starting point brush
        
        private Point selpt6 = new Point(0, 2);  //Select starting point geometry
        private SolidBrush solidbrush = new SolidBrush(Color.FromArgb(53, 154, 255));  //Shadow brush
        private PictureBox selpic1 = new PictureBox();  //Select PictureBox
        private PictureBox selpic2 = new PictureBox();  //Select PictureBox
        
        
        private DrawShape.ShapeType previousshapetype = DrawShape.ShapeType.Pencil;  //Last Draw Type

        public ToolPanel()
        {
            InitializeComponent();
            PictureBox_Click(pictureBox7, new EventArgs());
            
        }

        /// <summary>
        /// Magnification
        /// </summary>
        

        /// <summary>
        /// Drawing toolbar icon
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_Paint(object sender, PaintEventArgs e)
        {
            PictureBox pic = (PictureBox)sender;
            Point pt = pic.Location;
            string name = pic.Name.Remove(0, 10);
            int index = int.Parse(name) - 1;
            Image image = imageList1.Images[index];
            ImageAttributes imageAttributes = new ImageAttributes();
            int width = image.Width;
            int height = image.Height;
            ColorMap colorMap = new ColorMap();

            colorMap.OldColor = Color.FromArgb(255, 255, 0, 0);  // opaque red
            colorMap.NewColor = Color.Transparent;  // opaque blue

            ColorMap[] remapTable = { colorMap };

            imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap);
            
            e.Graphics.DrawImage(
               image,
               new Rectangle(2, 2, width, height),  // destination rectangle 
               0, 0,        // upper-left corner of source rectangle 
               width,       // width of source rectangle
               height,      // height of source rectangle
               GraphicsUnit.Pixel,
               imageAttributes);  //Image background transparent type of treatment
             
        }
             

        /// <summary>
        /// Change Tool
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PictureBox_Click(object sender, EventArgs e)
        {
            if (selpic1.Visible)
            {
                selpic1.Visible = false;
                selpic2.Visible = false;
            }
            
            if (prepic != null)
                prepic.BorderStyle = BorderStyle.None;
            PictureBox pic = (PictureBox)sender;
            pic.BorderStyle = BorderStyle.Fixed3D;
            prepic = pic;
            DrawShape.ShapeType st = DrawShape.ShapeType.None;
             if (pic.Name == "pictureBox4")
            {
                st = DrawShape.ShapeType.FillWithColor;
                if (!panel1.Visible)
                    panel1.Visible = true;
                if (panel2.Visible)
                    panel2.Visible = false;
                drawtype = 2;
            }
            
            else if (pic.Name == "pictureBox7")
            {
                st = DrawShape.ShapeType.Pencil;
                if (!panel1.Visible)
                    panel1.Visible = true;
                if (panel2.Visible)
                    panel2.Visible = false;
                drawtype = 2;
            }
            else if (pic.Name == "pictureBox8")
            {
                st = DrawShape.ShapeType.Brush;
                if (!panel1.Visible)
                    panel1.Visible = true;
                if (panel2.Visible)
                    panel2.Visible = false;
                drawtype = 4;
            }
            else if (pic.Name == "pictureBox11")
            {
                st = DrawShape.ShapeType.Line;
                if (panel1.Visible)
                    panel1.Visible = false;
                if (!panel2.Visible)
                    panel2.Visible = true;
                panel2.Location = panel1.Location;
            }
            else if (pic.Name == "pictureBox13")
            {
                st = DrawShape.ShapeType.Rectangle;
                if (!panel1.Visible)
                    panel1.Visible = true;
                if (!panel2.Visible)
                    panel2.Visible = true;
                panel2.Location = new Point(5, 270);
                drawtype = 6;
            }
            else if (pic.Name == "pictureBox15")
            {
                st = DrawShape.ShapeType.Ellipse;
                if (!panel1.Visible)
                    panel1.Visible = true;
                if (!panel2.Visible)
                    panel2.Visible = true;
                panel2.Location = new Point(5, 270);
                drawtype = 6;
            }
            
            panel1.Invalidate();
            OnToolChanged(new ToolChange(st, previousshapetype));
            previousshapetype = st;
        }

        #region Events

        /// <summary>
        /// Tool to change the event class
        /// </summary>
        public class ToolChange : EventArgs
        {
            private DrawShape.ShapeType shapetype;  //The current use of painting types
            private DrawShape.ShapeType previousshapetype;  //Last used the type of painting

            public ToolChange(DrawShape.ShapeType ShapeType, DrawShape.ShapeType PreviousShapeType)
            {
                this.shapetype = ShapeType;
                this.previousshapetype = PreviousShapeType;
            }

            /// <summary>
            /// The current use of painting types
            /// </summary>
            public DrawShape.ShapeType ShapeType
            {
                get { return this.shapetype; }
            }

            /// <summary>
            /// Last used the type of painting
            /// </summary>
            public DrawShape.ShapeType PreviousShapeType
            {
                get { return this.previousshapetype; }
            }
        }

        public delegate void ToolChangeEventHandler(object sender, ToolChange e);

        /// <summary>
        /// Tool to change the event
        /// </summary>
        public event ToolChangeEventHandler ToolChanged;

        protected virtual void OnToolChanged(ToolChange e)
        {
            if (ToolChanged != null) ToolChanged(this, e);
        }

        #endregion

        #region Status bar tips

        private void pictureBox1_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox2_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox3_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox4_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox5_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox6_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox7_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox8_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox9_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox10_MouseEnter(object sender, EventArgs e)
        {
           // 
        }

        private void pictureBox11_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox12_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox13_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox14_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox15_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void pictureBox16_MouseEnter(object sender, EventArgs e)
        {
            //
        }

        private void PictureBox_MouseLeave(object sender, EventArgs e)
        {
            //
        }

        #endregion

        /// <summary>
        /// Drawing toolbar
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.Black, 2), this.panel1.Left, this.panel1.Top, this.panel1.Left, this.panel1.Bottom);

            switch (drawtype)
            {
                case 0:
                    {
                        e.Graphics.FillRectangle(solidbrush, new Rectangle(selpt1, new Size(48, 30)));
                        selpic1.Visible = true;
                        selpic2.Visible = true;
                        break;
                    }
                case 1:
                    {
                        e.Graphics.FillRectangle(solidbrush, new Rectangle(selpt2, new Size(14, 14)));
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(20, 5, 4, 4));
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(19, 19, 6, 6));
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(18, 34, 8, 8));
                        e.Graphics.FillRectangle(Brushes.Black, new Rectangle(17, 49, 10, 10));
                        switch (selpt2.Y)
                        {
                            case 0:
                                e.Graphics.FillRectangle(Brushes.White, new Rectangle(20, 5, 4, 4));
                                break;
                            case 15:
                                e.Graphics.FillRectangle(Brushes.White, new Rectangle(19, 19, 6, 6));
                                break;
                            case 31:
                                e.Graphics.FillRectangle(Brushes.White, new Rectangle(18, 34, 8, 8));
                                break;
                            case 47:
                                e.Graphics.FillRectangle(Brushes.White, new Rectangle(17, 49, 10, 10));
                                break;
                        }
                        break;
                    }
                
               
                case 6:
                    {
                        e.Graphics.FillRectangle(solidbrush, new Rectangle(selpt6, new Size(48, 20)));
                        if (selpt6.Y == 0)
                            e.Graphics.DrawRectangle(Pens.White, new Rectangle(4, 5, 35, 10));
                        else
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(4, 5, 35, 10));
                        e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(4, 25, 35, 10));
                        if (selpt6.Y == 23)
                            e.Graphics.DrawRectangle(Pens.White, new Rectangle(4, 25, 35, 10));
                        else
                            e.Graphics.DrawRectangle(Pens.Black, new Rectangle(4, 25, 35, 10));
                        e.Graphics.FillRectangle(Brushes.Gray, new Rectangle(4, 45, 36, 12));
                        break;
                    }
            }
        }

        /// <summary>
        /// Select the tool to modify the position of the toolbar, and draw the selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            psform.canvas1.ds.fill = 0;
            switch (drawtype)
            {
                case 4:  //4 - The current tool brush
                    {
                        if (e.X <= 16)
                        {
                            selpt4 = new Point(2, 1);
                            psform.canvas1.ds.brushwidth = 8;
                        }
                        else if (e.X <= 32)
                        {
                            selpt4 = new Point(18, 0);
                            psform.canvas1.ds.brushwidth = 4;
                        }
                        else
                        {
                            selpt4 = new Point(33, 0);
                            psform.canvas1.ds.brushwidth = 1;
                        }

                        if (e.Y <= 14)
                        {
                            selpt4 = new Point(selpt4.X, 1);
                            psform.canvas1.ds.brushstyle = 0;
                        }
                        else if (e.Y <= 30)
                        {
                            selpt4 = new Point(selpt4.X, 15);
                            psform.canvas1.ds.brushstyle = 1;
                        }
                        else if (e.Y <= 46)
                        {
                            selpt4 = new Point(selpt4.X, 31);
                            psform.canvas1.ds.brushstyle = 2;
                        }
                        else
                        {
                            selpt4 = new Point(selpt4.X, 47);
                            psform.canvas1.ds.brushstyle = 3;
                        }
                        break;
                    }
               case 6:  //6 - The current tools for geometric
                    {
                        if (e.Y < 23)
                        {
                            psform.canvas1.ds.fill = 0;
                            selpt6 = new Point(0, 2);
                        }
                        else if (e.Y < 43)
                        {
                            psform.canvas1.ds.fill = 1;
                            selpt6 = new Point(0, 20);
                        }
                        else
                        {
                            psform.canvas1.ds.fill = 2;
                            selpt6 = new Point(0, 41);
                        }
                        break;
                    }
            }
            panel1.Invalidate();
        }

        /// <summary>
        /// Thickness of the brush to draw the optional
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(solidbrush, selrc);
            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(5, 5, 33, 1));
            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(5, 17, 33, 2));
            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(5, 27, 33, 3));
            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(5, 37, 33, 4));
            e.Graphics.FillRectangle(Brushes.Black, new Rectangle(5, 48, 33, 5));
        }

        /// <summary>
        /// The width of the brush replacement
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Y < 11)
            {
                selrc.Y = 0;
                psform.canvas1.ds.pen.Width = 1;
            }
            else if (e.Y < 22)
            {
                selrc.Y = 12;
                psform.canvas1.ds.pen.Width = 2;
            }
            else if (e.Y < 33)
            {
                selrc.Y = 23;
                psform.canvas1.ds.pen.Width = 3;
            }
            else if (e.Y < 44)
            {
                selrc.Y = 34;
                psform.canvas1.ds.pen.Width = 4;
            }
            else
            {
                selrc.Y = 45;
                psform.canvas1.ds.pen.Width = 5;
            }

            panel2.Invalidate();
        }
    }
}
