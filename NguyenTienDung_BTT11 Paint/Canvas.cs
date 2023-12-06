using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using NguyenTienDung_BTT6;

namespace MSPaint
{
    public partial class Canvas : UserControl
    {
        #region Field

        public DrawShape ds;
        
        private Cursor curpicturebox;  //Style in the mouse on the picturebox
        public bool mousedown = false;  //The mouse is pressed
       
        private Cursor[] icons;  //An array of styles using the mouse
        private DrawShape.ShapeType shape;
        public Size customesize = new Size(480, 331);
        public PaintForm paintform;

        

        private Pen pensize = new Pen(Color.Gray);  //Pen
        private Point sl = Point.Empty;  //Starting point

        

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern IntPtr LoadCursorFromFile(string fileName);

        public Canvas()
        {
            InitializeComponent();

            icons = new Cursor[] 
            {
                GetFileCursor((Path.GetDirectoryName(Application.ExecutablePath) + "\\res\\cross2.cur")),
                GetFileCursor((Path.GetDirectoryName(Application.ExecutablePath) + "\\res\\cross3.cur")),
                GetFileCursor((Path.GetDirectoryName(Application.ExecutablePath) + "\\res\\pickcolor.cur")),
                GetFileCursor((Path.GetDirectoryName(Application.ExecutablePath) + "\\res\\fillcolor.cur")),
                GetFileCursor((Path.GetDirectoryName(Application.ExecutablePath) + "\\res\\pencil.cur")),
                GetFileCursor((Path.GetDirectoryName(Application.ExecutablePath) + "\\res\\magnifier.cur")),
                GetFileCursor((Path.GetDirectoryName(Application.ExecutablePath) + "\\res\\airbrush.cur"))
            };
            this.curpicturebox = icons[4];
            pensize.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.AutoScrollMargin = new Size(3, 3);

            Initial();
        }

        #endregion

        #region Property

        /// <summary>
        /// Picture Settings canvas
        /// </summary>
        public Image PaintImage
        {
            get { return this.canvaspaint.Image; }
            set { this.canvaspaint.Image = value; }
        }

        /// <summary>
        /// Set the graphics rendering
        /// </summary>
        public DrawShape.ShapeType ShapeType
        {
            set { this.shape = value; }
        }

        public Color PenColor
        {
            set { this.ds.pen.Color = value; }
        }

        public Color BrushColor
        {
            set { this.ds.brush = new SolidBrush(value); this.ds.brushcolor = value; }
        }

        #endregion

        #region Main
       
        public void Initial()
        {
            this.canvaspaint.Image = DrawShape.CreateBitmap(this.customesize, Color.White);
            if (this.canvaspaint.Size != this.customesize)
                this.canvaspaint.Size = this.customesize;
            ds = new DrawShape(this.canvaspaint.Image);
            ds.pen = new Pen(Color.Black);
            ds.brush = Brushes.White;
            ds.UndoEvent += new DrawShape.UndoEventHandler(ds_UndoEvent);
            ds.RedoEvent += new DrawShape.UndoEventHandler(ds_RedoEvent);
            Invalidate();
        }

        public void ChangeBackgroundImage(string path)
        {
            Bitmap bmp = new Bitmap(path);
            this.canvaspaint.Image = bmp;
            this.canvaspaint.Size = bmp.Size;
            Pen oldpen = (Pen)ds.pen.Clone();
            Brush oldbursh = (Brush)ds.brush.Clone();
            ds = new DrawShape(this.canvaspaint.Image);
            ds.pen = (Pen)oldpen.Clone();
            ds.brush = (Brush)oldbursh.Clone();
            ds.UndoEvent += new DrawShape.UndoEventHandler(ds_UndoEvent);
            ds.RedoEvent += new DrawShape.UndoEventHandler(ds_RedoEvent);
            paintform.Text = Path.GetFileName(path) + " - Paint";
            paintform.openFileDialog1.FileName = path;
           
            ds.saved = true;
            paintform.currentfilepath = path;
            
            Invalidate();
        }

        public void ClearBackgroundImage()
        {
            this.canvaspaint.Image = DrawShape.CreateBitmap(this.canvaspaint.Size, ds.brushcolor);
        }

        public void ChangeBackgroundImage(Bitmap bitmap)
        {
            this.canvaspaint.Image = bitmap;
        }

        private void ds_UndoEvent(object sender, EventArgs e)
        {
            this.canvaspaint.Image = ds.ImageDrew;
            this.canvaspaint.Size = new Size(this.canvaspaint.Image.Width, this.canvaspaint.Image.Height);
            Invalidate();
        }

        public void ds_RedoEvent(object sender, EventArgs e)
        {
            this.canvaspaint.Image = ds.ImageDrew;
            this.canvaspaint.Size = new Size(this.canvaspaint.Image.Width, this.canvaspaint.Image.Height);
            Invalidate();
        }

        /// <summary>
        /// Replacement of the mouse style
        /// </summary>
        /// <param name="shape"></param>
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
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.Ellipse:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.Eraser:
                    {
                        Bitmap bitmap = DrawShape.CreateBitmap(ds.erasersize, ds.erasersize, Color.White);
                        Graphics g = Graphics.FromImage(bitmap);
                        g.DrawRectangle(new Pen(Color.Black), new Rectangle(0, 0, bitmap.Size.Width - 1, bitmap.Size.Width - 1));
                        g.Dispose();
                        this.curpicturebox = new Cursor(Icon.FromHandle(bitmap.GetHicon()).Handle);
                        break;
                    }
                case DrawShape.ShapeType.Line:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.Curve:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.Pencil:
                    {
                        this.curpicturebox = icons[4];
                        break;
                    }
                case DrawShape.ShapeType.Polygon:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.Rectangle:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.RoundedRectangle:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.Airbrush:
                    {
                        this.curpicturebox = icons[6];
                        break;
                    }
                case DrawShape.ShapeType.Brush:
                    {
                        this.curpicturebox = icons[0];
                        break;
                    }
                case DrawShape.ShapeType.FillWithColor:
                    {
                        this.curpicturebox = icons[3];
                        break;
                    }
                case DrawShape.ShapeType.FreeSelect:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
                case DrawShape.ShapeType.Magnifier:
                    {
                        this.curpicturebox = icons[5];
                        break;
                    }
                case DrawShape.ShapeType.PickColor:
                    {
                        this.curpicturebox = icons[2];
                        break;
                    }
                case DrawShape.ShapeType.Select:
                    {
                        this.curpicturebox = icons[1];
                        break;
                    }
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            ds.Shift = e.Shift;
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            ds.Shift = false;
            base.OnKeyUp(e);
        }

        public Cursor GetFileCursor(string file)
        {
            Cursor myCursor = new Cursor(Cursor.Current.Handle);
            IntPtr colorCursorHandle = LoadCursorFromFile(file);
            myCursor.GetType().InvokeMember("handle", System.Reflection.BindingFlags.Public |
              System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance |
              System.Reflection.BindingFlags.SetField, null, myCursor,
              new object[] { colorCursorHandle });
            return myCursor;
        }

        #endregion
        
        #region Drawing

        private void CanvasPaint_MouseDown(object sender, MouseEventArgs e)
        {
            if (SystemInformation.MouseButtonsSwapped)
            {
                if (e.Button != MouseButtons.Right)
                    return;
            }
            else
            {
                if (e.Button != MouseButtons.Left)
                    return;
            }
            this.mousedown = true;
            sl = e.Location;

            if (DrawShape.ShapeType.Pencil == shape)
            {
                ds.DrawGDI(shape, sl, e.Location, false, true);
                sl = e.Location;
                this.canvaspaint.Image = ds.ImageDrew;
            }
            
            else if (DrawShape.ShapeType.Brush == shape)
            {
                ds.DrawGDI(shape, sl, e.Location, true, true);
                this.canvaspaint.Image = ds.ImageDrew;
            }
            else if (DrawShape.ShapeType.FillWithColor == shape)
            {
                int ft = ds.fill;
                ds.fill = 0;
                ds.DrawGDI(shape, sl, e.Location, true, true);
                ds.fill = ft;
                this.canvaspaint.Image = ds.ImageDrew;
            }
            
            Point pt = new Point(e.Location.X - 8, e.Location.Y + 3);
            this.Text = pt.ToString();
           
        }
        



        double _chuvi = 0;
        double _dientich = 0;
        double r;

        private void CanvasPaint_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = this.curpicturebox;
            PaintForm.sbpanel3.Text = "";
            PaintForm.sbpanel2.Text = e.X.ToString() + "," + e.Y.ToString();
            Point pt = e.Location;

            
            if (mousedown)
            {
                if (DrawShape.ShapeType.Rectangle == shape)
                {
                    _chuvi = (Math.Abs(sl.X - pt.X) + Math.Abs(sl.Y - pt.Y)) * 2;
                    _dientich = Math.Abs(sl.X - pt.X) * Math.Abs(sl.Y - pt.Y);
                    PaintForm.sbpanel3.Text = "Chu vi: " + _chuvi.ToString() + ", " + "Diện tích: " + _dientich.ToString();
                }
                if (DrawShape.ShapeType.Ellipse == shape)
                {
                    r = Math.Sqrt((Math.Pow((sl.X - pt.X) / 2, 2) + Math.Pow((sl.Y - pt.Y) / 2, 2)) / 2);
                    _chuvi = 2 * r * Math.PI;
                    _dientich = (sl.X - pt.X) / 2 * (sl.Y - pt.Y) / 2 * Math.PI;
                    PaintForm.sbpanel3.Text = String.Format("Chu vi: {0:0.00}, Diện tích: {1:0.0}", _chuvi, _dientich);
                }
            }
            




            if (!mousedown)
                return;

            if (DrawShape.ShapeType.Rectangle == shape)
            {
                if (ds.Shift)
                    pt.Y = pt.X - sl.X + sl.Y;
                ds.DrawGDI(shape, sl, pt, true, false);
                this.canvaspaint.Image = ds.ImageDrew;
            }
            else if (DrawShape.ShapeType.Ellipse == shape)
            {
                if (ds.Shift)
                {
                    if (Math.Abs(pt.Y - sl.Y) < Math.Abs(pt.X - sl.X))
                        pt.Y = pt.X - sl.X + sl.Y;
                    else
                        pt.X = pt.Y - sl.Y + sl.X;
                }
                ds.DrawGDI(shape, sl, pt, true, false);
                this.canvaspaint.Image = ds.ImageDrew;
            }
            
            else if (DrawShape.ShapeType.Line == shape)
            {
                ds.DrawGDI(shape, sl, pt, true, false);
                this.canvaspaint.Image = ds.ImageDrew;
            }
            
            else if (DrawShape.ShapeType.Pencil == shape)
            {
                ds.DrawGDI(shape, sl, pt, true, false);
                sl = pt;
                this.canvaspaint.Image = ds.ImageDrew;
            }
            
            else if (DrawShape.ShapeType.Brush == shape)
            {
                ds.DrawGDI(shape, sl, pt, true, true);
                sl = pt;
                this.canvaspaint.Image = ds.ImageDrew;
            }
        }

        public void CanvasPaint_MouseUp(object sender, MouseEventArgs e)
        {
            if (!this.mousedown)
                return;
            Point pt = new Point();
            if (e != null)
                pt = e.Location;


            if (DrawShape.ShapeType.Rectangle == shape)
            {
                if (ds.Shift)
                    pt.Y = pt.X - sl.X + sl.Y;
                ds.DrawGDI(shape, sl, pt, false, true);
                this.canvaspaint.Image = ds.ImageDrew;
            }
            else if (DrawShape.ShapeType.Ellipse == shape)
            {
                if (ds.Shift)
                    pt.Y = pt.X - sl.X + sl.Y;
                ds.DrawGDI(shape, sl, pt, false, true);
                this.canvaspaint.Image = ds.ImageDrew;
            }
            
            else if (DrawShape.ShapeType.Line == shape)
            {
                ds.DrawGDI(shape, sl, pt, false, true);
                this.canvaspaint.Image = ds.ImageDrew;
            }
            
            else if (DrawShape.ShapeType.Pencil == shape)
            {
                ds.DrawGDI(shape, sl, pt, false, false);
                this.canvaspaint.Image = ds.ImageDrew;
            }
           
            else if (DrawShape.ShapeType.Brush == shape)
            {
                ds.DrawGDI(shape, sl, pt, false, true);
                ds.eraserpolygonpath.Reset();
                this.canvaspaint.Image = ds.ImageDrew;
            }

            this.mousedown = false;
        }

        
        #endregion
    }
}