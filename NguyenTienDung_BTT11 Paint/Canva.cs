using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Paint
{
    public partial class Canva : UserControl
    {
        private enum Resize { SizeNS, SizeNWSE, SizeWE, None }
        private Resize resize;
        private Cursor curpanel;
        private Cursor curpicturebox;
        private bool mousedown = false;
        private Size newsize;
        private Icon[] icons;
        private Bitmap background;
        private Rectangle rc1;
        private Rectangle rc2;
        private Rectangle rc3;
        private Rectangle rc4;
        private Rectangle rc5;
        private Rectangle rc6;
        private Rectangle rc7;
        private Rectangle rc8;
        private Pen pensize = new Pen(Color.Gray);

        public Canva()
        {
            InitializeComponent();
            resize = Resize.None;
            icons = new Icon[] 
            {
                new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\icon\\mspaint_1201.ico"),
                new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\icon\\mspaint_1203.ico"),
                new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\icon\\mspaint_1204.ico"),
                new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\icon\\mspaint_1205.ico"),
                new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\icon\\mspaint_1206.ico"),
                new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\icon\\mspaint_1207.ico"),
                new Icon(Path.GetDirectoryName(Application.ExecutablePath) + "\\icon\\mspaint_1208.ico")
            };
            this.curpicturebox = new Cursor(icons[4].Handle);
            rc1 = new Rectangle(0, 0, 3, 3);
            pensize.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
        }

        /// <summary>
        /// Setting the canvas
        /// </summary>
        public Image Image
        {
            get { return this.pictureBox1.Image; }
            set { this.pictureBox1.Image = value; }
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            rc2 = new Rectangle(this.pictureBox1.Width / 2, 0, 3, 3);
            rc3 = new Rectangle(this.pictureBox1.Width + 3, 0, 3, 3);
            rc4 = new Rectangle(0, this.pictureBox1.Height / 2, 3, 3);
            rc5 = new Rectangle(0, this.pictureBox1.Height + 3, 3, 3);
            rc6 = new Rectangle(this.pictureBox1.Width / 2, this.pictureBox1.Height + 3, 3, 3);
            rc7 = new Rectangle(this.pictureBox1.Width + 3, this.pictureBox1.Height + 3, 3, 3);
            rc8 = new Rectangle(this.pictureBox1.Width + 3, this.pictureBox1.Height / 2, 3, 3);

            //Horizontal
            e.Graphics.FillRectangle(Brushes.White, rc1);
            e.Graphics.FillRectangle(Brushes.White, rc2);
            e.Graphics.FillRectangle(Brushes.White, rc3);

            //Vertical
            e.Graphics.FillRectangle(Brushes.White, rc4);
            e.Graphics.FillRectangle(Brushes.White, rc5);

            //Bottom
            e.Graphics.FillRectangle(Brushes.Blue, rc6);
            e.Graphics.FillRectangle(Brushes.Blue, rc7);

            //Intermediate
            e.Graphics.FillRectangle(Brushes.Blue, rc8);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (!mousedown)
            {
                Rectangle rc = new Rectangle(this.pictureBox1.Width / 2, this.pictureBox1.Height + 3, 3, 3);
                bool contain = false;
                resize = Resize.None;
                if (rc.Contains(e.Location))
                {
                    this.curpanel = Cursors.SizeNS;
                    contain = true;
                    resize = Resize.SizeNS;
                }
                else
                    this.curpanel = Cursors.Default;
                rc = new Rectangle(this.pictureBox1.Width + 3, this.pictureBox1.Height + 3, 3, 3);
                if (rc.Contains(e.Location))
                {
                    this.curpanel = Cursors.SizeNWSE;
                    contain = true;
                    resize = Resize.SizeNWSE;
                }
                else if (!contain)
                    this.curpanel = Cursors.Default;
                rc = new Rectangle(this.pictureBox1.Width + 3, this.pictureBox1.Height / 2, 3, 3);
                if (rc.Contains(e.Location))
                {
                    this.curpanel = Cursors.SizeWE;
                    contain = true;
                    resize = Resize.SizeWE;
                }
                else if (!contain)
                    this.curpanel = Cursors.Default;
            }

            this.Cursor = this.curpanel;

            if (mousedown)
            {
                switch (resize)
                {
                    case Resize.SizeNS:
                        {
                            newsize = new Size(this.pictureBox1.Width, e.Y);

                            Point pt = new Point(0, 0);
                            WindowsAPI.ClientToScreen(this.panel1.Handle, ref pt);
                            if (Cursor.Position.Y < pt.Y + this.panel1.Height - 6)
                            {
                                this.panel1.Refresh();
                                ControlPaint.DrawReversibleFrame(new Rectangle(new Point(pt.X + 3, pt.Y + 3), newsize), Color.Gray, FrameStyle.Dashed); ;
                            }
                            break;
                        }
                    case Resize.SizeNWSE:
                        {
                            newsize = new Size(e.X, e.Y);

                            Point pt = new Point(0, 0);
                            WindowsAPI.ClientToScreen(this.panel1.Handle, ref pt);
                            if ((Cursor.Position.Y < pt.Y + this.panel1.Height - 6) && (Cursor.Position.X < pt.X + this.panel1.Width - 6))
                            {
                                this.panel1.Refresh();
                                ControlPaint.DrawReversibleFrame(new Rectangle(new Point(pt.X + 3, pt.Y + 3), newsize), Color.Gray, FrameStyle.Dashed);
                            }
                            break;
                        }
                    case Resize.SizeWE:
                        {
                            newsize = new Size(e.X, this.pictureBox1.Height);

                            Point pt = new Point(0, 0);
                            WindowsAPI.ClientToScreen(this.panel1.Handle, ref pt);
                            if (Cursor.Position.X < pt.X + this.panel1.Width - 6)
                            {
                                this.panel1.Refresh();
                                ControlPaint.DrawReversibleFrame(new Rectangle(new Point(pt.X + 3, pt.Y + 3), newsize), Color.Gray, FrameStyle.Dashed);
                            }
                            break;
                        }
                }                
            }
        }

        private void panel1_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            Rectangle rc = new Rectangle(this.pictureBox1.Width / 2, this.pictureBox1.Height + 3, 3, 3);
            if (rc.Contains(e.Location))
                mousedown = true;
            rc = new Rectangle(this.pictureBox1.Width + 3, this.pictureBox1.Height + 3, 3, 3);
            if (rc.Contains(e.Location))
                mousedown = true;
            rc = new Rectangle(this.pictureBox1.Width + 3, this.pictureBox1.Height / 2, 3, 3);
            if (rc.Contains(e.Location))
                mousedown = true; 
            Point pt = new Point(0, 0);
            WindowsAPI.ClientToScreen(this.panel1.Handle, ref pt);
            background = Accessory.GetScreen(pt.X, pt.Y, this.panel1.Width, this.panel1.Height);
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            if (mousedown)
            {
                Rectangle rect1 = Rectangle.Empty;
                Rectangle rect2 = Rectangle.Empty;
                if (newsize.Width > this.pictureBox1.Width)
                {
                    rect1 = new Rectangle(this.pictureBox1.Width, 0, newsize.Width - this.pictureBox1.Width, this.pictureBox1.Height);
                }
                if (newsize.Height > this.pictureBox1.Height)
                {
                    rect2 = new Rectangle(0, this.pictureBox1.Height, this.pictureBox1.Width, newsize.Height - this.pictureBox1.Height);
                }
                this.pictureBox1.Size = newsize;
                mousedown = false;
                Bitmap bitmap = PhotoShop.CombineBitmap(PhotoShop.CreateBitmap(newsize, Color.White), this.pictureBox1.Image, new Point(0, 0));
                Graphics g = Graphics.FromImage(bitmap);
                if (rect1 != Rectangle.Empty)
                {
                    g.FillRectangle(new SolidBrush(PaintForm.canvascolor), rect1);
                }
                if (rect2 != Rectangle.Empty)
                {
                    g.FillRectangle(new SolidBrush(PaintForm.canvascolor), rect2);
                }
                if (rect1 != Rectangle.Empty && rect2 != Rectangle.Empty)
                {
                    g.FillRectangle(new SolidBrush(PaintForm.canvascolor), new Rectangle(rect1.Left, rect2.Top, rect1.Width, rect2.Width));
                }
                this.pictureBox1.Image = bitmap;
                this.panel1.AutoScrollMinSize = new Size(this.pictureBox1.Width + 10, this.pictureBox1.Height + 10);
                this.panel1.BackgroundImage = null;
                this.panel1.Refresh();
            }
        }

        public void GetCursorStyle(DrawShape.ShapeType shape)
        {
            switch (shape)
            {
                case DrawShape.ShapeType.Arrow:
                    {
                        break;
                    }
                case DrawShape.ShapeType.Text:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Ellipse:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Eraser:
                    {
                        Bitmap bitmap = PhotoShop.CreateBitmap(8, 8, Color.White);
                        Graphics g = Graphics.FromImage(bitmap);
                        g.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, bitmap.Size.Width - 1, bitmap.Size.Width - 1));
                        g.Dispose();
                        this.curpicturebox = new Cursor(Icon.FromHandle(bitmap.GetHicon()).Handle);
                        break;
                    }
                case DrawShape.ShapeType.Line:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Curve:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Pencil:
                    {
                        this.curpicturebox = new Cursor(icons[4].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Polygon:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Rectangle:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.RoundedRectangle:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Airbrush:
                    {
                        this.curpicturebox = new Cursor(icons[6].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Brush:
                    {
                        this.curpicturebox = new Cursor(icons[0].Handle);
                        break;
                    }
                case DrawShape.ShapeType.FillWithColor:
                    {
                        this.curpicturebox = new Cursor(icons[3].Handle);
                        break;
                    }
                case DrawShape.ShapeType.FreeSelect:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Magnifier:
                    {
                        this.curpicturebox = new Cursor(icons[5].Handle);
                        break;
                    }
                case DrawShape.ShapeType.PickColor:
                    {
                        this.curpicturebox = new Cursor(icons[2].Handle);
                        break;
                    }
                case DrawShape.ShapeType.Select:
                    {
                        this.curpicturebox = new Cursor(icons[1].Handle);
                        break;
                    }
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = this.curpicturebox;
            Point pt = new Point(Cursor.Position.X - Cursor.HotSpot.X, Cursor.Position.Y - Cursor.HotSpot.Y);
            //PaintForm.sbpanel2.Text = e.X.ToString() + "," + e.Y.ToString();
            WindowsAPI.ScreenToClient(this.pictureBox1.Handle, ref pt);
            PaintForm.sbpanel2.Text = pt.X.ToString() + "," + pt.Y.ToString();
        }
    }
}
