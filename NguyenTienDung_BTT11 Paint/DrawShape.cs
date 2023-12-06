using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Collections;

namespace MSPaint
{
    public class DrawShape
    {
        #region Field

        public enum ShapeType { Pencil, Ellipse, Rectangle, Line, Arrow, Polygon, Eraser, None, RoundedRectangle, FreeSelect, Select, FillWithColor, PickColor, Magnifier, Brush, Airbrush, Text, Curve, Resize }
        private Graphics graphics;  //Drawing Objects
        private Image imagestatus;  //Use of the images MouseUp
        private Image imagedrew;  //Use of the images MouseMove
        private Image imagetofill;  //Fill picture
        private Image startimage;  //Initialization image
        private Graphics gf;  //Memory image object
        private IntPtr hdcfill;  //Memory image device handle
        public Pen pen;  //Pen
        public Brush brush;  //Brush
        public Color brushcolor;
        public Point ps = Point.Empty;  //Brush polygon point
        public bool Shift = false;  //Shift-it
        public int erasersize = 8;  //The size of the starting rubber
        public GraphicsPath eraserpolygonpath = new GraphicsPath();  //Polygons used to draw the path of rubber or
        public int fill = 0;  //Use the fill mode. 0 - to draw transparent graphics; 1 - Drawing a background color of the graphics; 2 - to fill the way to draw
        public bool saved = true;  //Indicates that the current drawing is saved
        public int airbrushstyle = 0;  //Gun style
        public int brushwidth = 8;  //Brush width
        public int brushstyle = 0;  //Brush Style
        private List<DrawProcess> orginprocess = new List<DrawProcess>();  //Reinstated drawing class
        private List<DrawProcess> currentprocess = new List<DrawProcess>();  //Drawing class with withdrawal
        public Point psback = Point.Empty;  //Back up starting point for the polygon, used to start the final connection of a point
        public bool psfinish = false;  //Polygon drawing has been completed to determine whether
        private DrawProcess dpp;  //The current process of drawing
        private bool repeal = false;  //Over whether to withdraw
        public Point curvestartpt = Point.Empty;  //Curve starting point
        public Point curveendpt = Point.Empty;  //Curve end
        public Point curvemidpt1 = Point.Empty;  //Mid-point curve
        public Point curvemidpt2 = Point.Empty;  //Mid-point curve
        private bool textransparent = false;  //Whether to draw text transparent background

        #endregion
        
        #region Events & Properties
        
        public delegate void UndoEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Revocation of the event
        /// </summary>
        public event UndoEventHandler UndoEvent;

        protected virtual void OnUndoEvent(EventArgs e)
        {
            if (UndoEvent != null) UndoEvent(this, e);
        }

        public delegate void RedoEventHandler(object sender, EventArgs e);

        /// <summary>
        /// Recovery events
        /// </summary>
        public event UndoEventHandler RedoEvent;

        protected virtual void OnRedoEvent(EventArgs e)
        {
            if (RedoEvent != null) RedoEvent(this, e);
        }

        /// <summary>
        /// Complete a mapping diagram
        /// </summary>
        public Image ImageStatus
        {
            get { return this.imagestatus; }
            set { this.imagestatus = value; }
        }

        /// <summary>
        /// Process map
        /// </summary>
        public Image ImageDrew
        {
            get { return this.imagedrew; }
            set { this.imagedrew = value; }
        }
        
        /// <summary>
        /// Whether to revoke the
        /// </summary>
        public bool CanUndo
        {
            get { if (this.currentprocess.Count > 0)return true; return false; }
        }

        /// <summary>
        /// Can restore
        /// </summary>
        public bool CanRedo
        {
            get { if (this.currentprocess.Count < this.orginprocess.Count)return true; return false; }
        }

        /// <summary>
        /// Add text to create the transparent background
        /// </summary>
        public bool TextTransparet
        {
            get { return this.textransparent; }
            set { this.textransparent = value; }
        }

        public DrawShape(Image img)
        {
            this.imagestatus = img;
            this.imagetofill = (Image)this.imagestatus.Clone();
            this.startimage = (Image)img.Clone();
            brush = Brushes.White;
            pen = Pens.Black;
            orginprocess.Clear();
            currentprocess.Clear();
        }

        public DrawShape(System.Windows.Forms.Form form)
        {
            this.imagestatus = CreateBitmap(form.Width, form.Height, form.BackColor);
            this.startimage = (Image)imagestatus.Clone();
            brush = Brushes.White;
            pen = Pens.Black;
            orginprocess.Clear();
            currentprocess.Clear();
        }

        public DrawShape(System.Windows.Forms.Control control)
        {
            this.imagestatus = CreateBitmap(control.Width, control.Height, control.BackColor);
            this.startimage = (Image)imagestatus.Clone();
            brush = Brushes.White;
            pen = Pens.Black;
            orginprocess.Clear();
            currentprocess.Clear();
        }
        
        #endregion

        #region General Graphics

        public void DrawGDI(ShapeType st, Point sl, Point p, bool md, bool canadd)
        {            
            if (this.imagedrew == null)
                this.imagedrew = this.imagestatus;
            if (repeal)
            {
                orginprocess.Clear();
                orginprocess.AddRange(currentprocess);
            }
            if (canadd)
            {
                saved = false;
                if (ShapeType.Eraser != st && ShapeType.Pencil != st)
                {
                    int fillstyle = this.fill;
                    if (ShapeType.Rectangle != st && ShapeType.Ellipse != st && ShapeType.Polygon != st && ShapeType.RoundedRectangle != st)
                        fill = 0;
                    dpp = new DrawProcess(st, sl, p, md, this.imagedrew.Size, (Pen)this.pen.Clone(), (Brush)this.brush.Clone(), fillstyle, LinePoint.Empty);
                    orginprocess.Add(dpp);
                    currentprocess.Add(dpp);
                }
                else
                {
                    dpp = new DrawProcess(st, sl, p, md, this.imagedrew.Size, (Pen)this.pen.Clone(), (Brush)this.brush.Clone(), 0, new LinePoint(sl, p));
                    orginprocess.Add(dpp);
                    currentprocess.Add(dpp);
                }
            }
            int ft = this.fill;
            if (ShapeType.Ellipse != st && ShapeType.Rectangle != st && ShapeType.RoundedRectangle != st && ShapeType.Polygon != st)
            {
                this.fill = 0;
            }
            if (this.fill == 0)
                DrawGDI_Line(st, sl, p, md);
            else if (this.fill == 1)
            {
                DrawGDI_Line_Fill(st, sl, p, md, brushcolor);
            }
            else if (this.fill == 2)
                DrawGDI_Fill(st, sl, p, md, this.pen.Color);
            this.fill = ft;
        }

        private void DrawGDI_Line(ShapeType st, Point sl, Point p, bool md)
        {
            PointF rectStartPointF = sl;
            switch (st)
            {
                case ShapeType.Rectangle:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            imagedrew = imagestatus;
                        }
                        else
                        {
                            if (this.Shift)
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            else
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                        }
                        break;
                    }
                case ShapeType.Ellipse:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            else
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            imagedrew = imagestatus;
                        }
                        else
                        {
                            if (this.Shift)
                            {
                                height = width;
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            else
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                        }
                        break;
                    }
                
                case ShapeType.Pencil:
                    {
                        dpp.AddLinePoint(new LinePoint(sl, p));
                        if (!md)
                        {
                            Bitmap bitmap = new Bitmap(1, 1);
                            graphics = Graphics.FromImage(bitmap);
                            graphics.FillRectangle(Brushes.Black, new Rectangle(0, 0, 1, 1));
                            graphics.Dispose();
                            graphics = Graphics.FromImage(imagedrew);
                            graphics.DrawImage(bitmap, p);
                        }
                        else
                        {
                            graphics = Graphics.FromImage(imagestatus);
                            LineCap lc = this.pen.EndCap;
                            this.pen.StartCap = this.pen.EndCap = LineCap.Round;
                            graphics.DrawLine(pen, sl, p);
                            this.pen.StartCap = this.pen.EndCap = lc;
                            imagedrew = imagestatus;
                        }
                        break;
                    }
                
                case ShapeType.Line:
                    {
                        if (!md)
                        {
                            graphics = Graphics.FromImage(imagestatus);
                            graphics.DrawLine(pen, sl.X, sl.Y, p.X, p.Y);
                            imagedrew = imagestatus;
                        }
                        else
                        {
                            imagedrew = (Image)this.imagestatus.Clone();
                            graphics = Graphics.FromImage(imagedrew);
                            graphics.DrawLine(pen, sl.X, sl.Y, p.X, p.Y);
                        }
                        break;
                    }
                
                case ShapeType.Brush:
                    {
                        graphics = Graphics.FromImage(imagestatus);
                        if (md)
                        {
                            if (sl != p)
                            {
                                Pen temppen = new Pen(pen.Color);
                                temppen.Width = this.brushwidth;
                                if (this.brushstyle == 0)
                                {
                                    temppen.StartCap = LineCap.Round;
                                    temppen.EndCap = LineCap.Round;
                                }
                                else if (this.brushstyle == 1)
                                {
                                    temppen.StartCap = LineCap.Square;
                                    temppen.EndCap = LineCap.Square;
                                }
                                else if (this.brushstyle == 2)
                                {
                                    temppen.StartCap = LineCap.AnchorMask;
                                    temppen.EndCap = LineCap.AnchorMask;
                                }
                                else if (this.brushstyle == 3)
                                {
                                }

                                eraserpolygonpath.AddLine(new Point(sl.X, sl.Y), new Point(p.X, p.Y));
                                graphics.DrawPath(temppen, eraserpolygonpath);
                            }
                            else
                                graphics.FillRectangle(brush, new Rectangle(new Point(sl.X - this.erasersize / 2, sl.Y - this.erasersize / 2), new Size(this.erasersize, this.erasersize)));
                        }
                        else
                        {
                            Pen temppen = new Pen(pen.Color);
                            temppen.Width = this.brushwidth;
                            if (this.brushstyle == 0)
                            {
                                temppen.StartCap = LineCap.Round;
                                temppen.EndCap = LineCap.Round;
                            }
                            else if (this.brushstyle == 1)
                            {
                                temppen.StartCap = LineCap.Square;
                                temppen.EndCap = LineCap.Square;
                            }
                            else if (this.brushstyle == 2)
                            {
                            }
                            else if (this.brushstyle == 3)
                            {
                            }

                            eraserpolygonpath.AddLine(new Point(sl.X, sl.Y), new Point(p.X, p.Y));
                            graphics.DrawPath(temppen, eraserpolygonpath);
                        }
                        imagedrew = imagestatus;
                        break;
                    }
                case ShapeType.FillWithColor:
                    {
                        PlayDrawing();
                        break;
                    }
            }
        }

        private void DrawGDI_Fill(ShapeType st, Point sl, Point p, bool md, Color color)
        {
            PointF rectStartPointF = sl;
            switch (st)
            {
                case ShapeType.Rectangle:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillRectangle_API_Ex(hdcfill, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillRectangle_API_Ex(hdcfill, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            imagedrew = imagestatus;
                        }
                        else
                        {
                            if (this.Shift)
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height); DrawEllipse_API_Ex(hdcfill, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                        }
                        break;
                    }
                case ShapeType.Ellipse:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillEllipse_API_Ex(hdcfill, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillEllipse_API_Ex(hdcfill, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            imagedrew = imagestatus;
                        }
                        else
                        {
                            if (this.Shift)
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            else
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                        }
                        break;
                    }
                
            }
        }

        private void DrawGDI_Line_Fill(ShapeType st, Point sl, Point p, bool md, Color color)
        {
            PointF rectStartPointF = sl;
            switch (st)
            {
                case ShapeType.Rectangle:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillRectangle_API_Ex(hdcfill, brushcolor, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawRectangle_API_Ex(hdcfill, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillRectangle_API_Ex(hdcfill, brushcolor, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawRectangle_API_Ex(hdcfill, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            imagedrew = imagestatus;
                        }
                        else
                        {
                            if (this.Shift)
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            else
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillRectangle(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawRectangle(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                        }
                        break;
                    }
                case ShapeType.Ellipse:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillEllipse_API_Ex(hdcfill, brushcolor, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawEllipse_API_Ex(hdcfill, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                graphics = Graphics.FromImage(imagestatus);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                                FillEllipse_API_Ex(hdcfill, brushcolor, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawEllipse_API_Ex(hdcfill, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            imagedrew = imagestatus;
                        }
                        else
                        {
                            if (this.Shift)
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                            else
                            {
                                imagedrew = (Image)this.imagestatus.Clone();
                                graphics = Graphics.FromImage(imagedrew);
                                graphics.FillEllipse(new SolidBrush(color), rectStartPointF.X, rectStartPointF.Y, width, height);
                                graphics.DrawEllipse(pen, rectStartPointF.X, rectStartPointF.Y, width, height);
                            }
                        }
                        break;
                    }
                
            }
        }

        #endregion

        /// <summary>
        /// When an event using the method fill fill area color
        /// </summary>
        public void PlayDrawing()
        {
            Pen oldpen = (Pen)this.pen.Clone();
            Brush oldbrush = (Brush)this.brush.Clone();
            Color oldbrushcolor = this.brushcolor;
            this.imagetofill = (Image)this.startimage.Clone();
            gf = Graphics.FromImage(this.imagetofill);
            hdcfill = gf.GetHdc();
            for (int i = 0; i < currentprocess.Count; i++)
            {
                DrawProcess dp = (DrawProcess)currentprocess[i];
                this.pen = dp.PenUsed;
                this.brush = dp.BrushUsed;
                if (ShapeType.Resize == dp.ShapeTypeValue)
                {
                    Bitmap bitmap = DrawShape.CreateBitmap(dp.CanvasSize, brushcolor);
                    this.imagetofill = DrawShape.CombineBitmap(bitmap, this.imagetofill, new Point(0, 0));
                }
                else
                {
                    
                    DrawGDI2(dp.ShapeTypeValue, dp.StartPoint, dp.EndPoint, dp.MouseDown, dp.FillType, dp.BrushUsed, dp.PenUsed, hdcfill, dp.CurvePoints.ToArray());
                    if (dp.ShapeTypeValue == ShapeType.Pencil)
                    {
                        for (int j = 0; j < dp.LinePoints.Count; j++)
                        {
                            DrawGDI2(dp.ShapeTypeValue, dp.LinePoints[j].startpoint, dp.LinePoints[j].endpoint, true, dp.FillType, dp.BrushUsed, dp.PenUsed, hdcfill, dp.CurvePoints.ToArray());
                        }
                    }
                    
                }
            }
            this.pen = (Pen)oldpen.Clone();
            this.brush = (Brush)oldbrush.Clone();
            this.brushcolor = oldbrushcolor;
            gf.ReleaseHdc();
            gf.Dispose();
            this.imagestatus = this.imagedrew = (Image)this.imagetofill.Clone();
        }

        #region The principal means of memory mapping

        public void DrawGDI2(ShapeType st, Point sl, Point p, bool md, int filltype, Brush brush, Pen pen, IntPtr hdcd, Point[] points)
        {
            if (filltype == 0)
                DrawGDI_Line2(st, sl, p, md, hdcd, points);
            else if (filltype == 1)
            {
                SolidBrush solidbrush = (SolidBrush)brush;
                DrawGDI_Line_Fill2(st, sl, p, md, solidbrush.Color, pen, hdcd);
            }
            else if (filltype == 2)
                DrawGDI_Fill2(st, sl, p, md, pen.Color, hdcd);
        }

        private void DrawGDI_Line2(ShapeType st, Point sl, Point p, bool md, IntPtr hdcd, Point[] points)
        {
            PointF rectStartPointF = sl;
            switch (st)
            {
                case ShapeType.Rectangle:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                DrawRectangle_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                DrawRectangle_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                        }
                        break;
                    }
                case ShapeType.Ellipse:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                DrawEllipse_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                DrawEllipse_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                        }
                        break;
                    }
                
                case ShapeType.Pencil:
                    {
                        if (!md)
                        {
                            WindowsAPI.SetPixel(hdcd, p.X, p.Y, ColorTranslator.ToWin32(this.pen.Color));
                        }
                        else
                        {
                            IntPtr vPen = WindowsAPI.CreatePen(CommonConst.PS_SOLID, 1, ColorTranslator.ToWin32(this.pen.Color));
                            IntPtr vPreviousPen = WindowsAPI.SelectObject(hdcd, vPen);
                            Point pt = new Point();
                            WindowsAPI.MoveToEx(hdcd, sl.X, sl.Y, ref pt);
                            WindowsAPI.LineTo(hdcd, p.X, p.Y);
                            WindowsAPI.SelectObject(hdcd, vPreviousPen);
                            WindowsAPI.DeleteObject(vPen);
                        }
                        break;
                    }
                
                case ShapeType.Line:
                    {
                        if (!md)
                        {
                            DrawLine_API_Ex(hdcd, pen, sl, p);
                        }
                        break;
                    }
                case ShapeType.Brush:
                    {
                        graphics = Graphics.FromImage(imagestatus);
                        if (md)
                        {
                            if (sl != p)
                            {
                                Pen temppen = new Pen(pen.Color);
                                temppen.Width = this.brushwidth;
                                if (this.brushstyle == 0)
                                {
                                    temppen.StartCap = LineCap.Round;
                                    temppen.EndCap = LineCap.Round;
                                }
                                else if (this.brushstyle == 1)
                                {
                                    temppen.StartCap = LineCap.Square;
                                    temppen.EndCap = LineCap.Square;
                                }
                                else if (this.brushstyle == 2)
                                {
                                }
                                else if (this.brushstyle == 3)
                                {
                                }

                                eraserpolygonpath.AddLine(new Point(sl.X, sl.Y), new Point(p.X, p.Y));
                                graphics.DrawPath(temppen, eraserpolygonpath);
                            }
                            else
                                graphics.FillRectangle(brush, new Rectangle(new Point(sl.X - this.erasersize / 2, sl.Y - this.erasersize / 2), new Size(this.erasersize, this.erasersize)));
                        }
                        else
                        {
                            Pen temppen = new Pen(pen.Color);
                            temppen.Width = this.brushwidth;
                            if (this.brushstyle == 0)
                            {
                                temppen.StartCap = LineCap.Round;
                                temppen.EndCap = LineCap.Round;
                            }
                            else if (this.brushstyle == 1)
                            {
                                temppen.StartCap = LineCap.Square;
                                temppen.EndCap = LineCap.Square;
                            }
                            else if (this.brushstyle == 2)
                            {
                            }
                            else if (this.brushstyle == 3)
                            {
                            }

                            eraserpolygonpath.AddLine(new Point(sl.X, sl.Y), new Point(p.X, p.Y));
                            graphics.DrawPath(temppen, eraserpolygonpath);
                        }
                        imagedrew = imagestatus;
                        break;
                    }
                case ShapeType.FillWithColor:
                    {
                        FloodFill1(hdcd, p.X, p.Y, pen.Color);
                        break;
                    }
            }
        }

        private void DrawGDI_Fill2(ShapeType st, Point sl, Point p, bool md, Color color, IntPtr hdcd)
        {
            PointF rectStartPointF = sl;
            switch (st)
            {
                case ShapeType.Rectangle:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                FillRectangle_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                FillRectangle_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                        }
                        break;
                    }
                case ShapeType.Ellipse:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                FillEllipse_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                FillEllipse_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                        }
                        break;
                    }
                
            }
        }

        private void DrawGDI_Line_Fill2(ShapeType st, Point sl, Point p, bool md, Color color, Pen pen, IntPtr hdcd)
        {
            PointF rectStartPointF = sl;
            switch (st)
            {
                case ShapeType.Rectangle:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                FillRectangle_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawRectangle_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                FillRectangle_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawRectangle_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                        }
                        break;
                    }
                case ShapeType.Ellipse:
                    {
                        float width = Math.Abs(p.X - sl.X);
                        float height = Math.Abs(p.Y - sl.Y);
                        if (p.X < sl.X)
                        {
                            rectStartPointF.X = p.X;
                        }
                        if (p.Y < sl.Y)
                        {
                            rectStartPointF.Y = p.Y;
                        }

                        if (!md)
                        {
                            if (this.Shift)
                            {
                                FillEllipse_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawEllipse_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                            else
                            {
                                FillEllipse_API_Ex(hdcd, color, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                                DrawEllipse_API_Ex(hdcd, pen, Convert.ToInt32(rectStartPointF.X), Convert.ToInt32(rectStartPointF.Y), Convert.ToInt32(width), Convert.ToInt32(height));
                            }
                        }
                        break;
                    }
                
            }
        }

        #endregion

        #region Create a solid color image

        /// <summary>
        /// Create a solid color image
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap CreateBitmap(int width, int height, Color color)
        {
            if (width == 0 || height == 0)
                return null;
            if (color == Color.Empty)
                color = Color.White;
            Bitmap bitmap = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(0, 0), new Size(width, height)));
            g.Dispose();
            return bitmap;
        }

        /// <summary>
        /// Create a solid color image
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="color"></param>
        /// <param name="pixel"></param>
        /// <returns></returns>
        public static Bitmap CreateBitmap(int width, int height, Color color, PixelFormat pixel)
        {
            if (width == 0 || height == 0)
                return null;
            if (color == Color.Empty)
                color = Color.White;
            Bitmap bitmap = new Bitmap(width, height, pixel);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(0, 0), new Size(width, height)));
            g.Dispose();
            return bitmap;
        }

        /// <summary>
        /// Create a solid color image
        /// </summary>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap CreateBitmap(Size size, Color color)
        {
            if (size == Size.Empty)
                return null;
            if (color == Color.Empty)
                color = Color.White;
            Bitmap bitmap = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(0, 0), size));
            g.Dispose();
            return bitmap;
        }

        /// <summary>
        /// Create a solid color image
        /// </summary>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap CreateBitmap(Size size, Color color, PixelFormat pixel)
        {
            if (size == Size.Empty)
                return null;
            if (color == Color.Empty)
                color = Color.White;
            Bitmap bitmap = new Bitmap(size.Width, size.Height, pixel);
            Graphics g = Graphics.FromImage(bitmap);
            g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(0, 0), size));
            g.Dispose();
            return bitmap;
        }

        #endregion
        
        #region Draw / fill rectangle

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawRectangle_API_Ex(Image img, Pen pen, int x, int y, int width, int height)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawRectangle_API_Ex(vGraphics, pen, x, y, width, height);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawRectangle_API_Ex(Graphics vGraphics, Pen pen, int x, int y, int width, int height)
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            DrawCreate(vDC, pen, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Rectangle(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
            vGraphics.ReleaseHdc(vDC);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawRectangle_API_Ex(IntPtr vDC, Pen pen, int x, int y, int width, int height)
        {
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            DrawCreate(vDC, pen, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Rectangle(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pen"></param>
        /// <param name="rc"></param>
        public static void DrawRectangle_API_Ex(Image img, Pen pen, Rectangle rc)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawRectangle_API_Ex(vGraphics, pen, rc);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="pen"></param>
        /// <param name="rc"></param>
        public static void DrawRectangle_API_Ex(Graphics vGraphics, Pen pen, Rectangle rc)
        {
            DrawRectangle_API_Ex(vGraphics, pen, rc.X, rc.Y, rc.Width, rc.Height);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="img"></param>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FillRectangle_API_Ex(Image img, Color color, int x, int y, int width, int height)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            FillRectangle_API_Ex(vGraphics, color, x, y, width, height);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FillRectangle_API_Ex(Graphics vGraphics, Color color, int x, int y, int width, int height)
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            FillCreate(vDC, color, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Rectangle(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
            vGraphics.ReleaseHdc(vDC);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FillRectangle_API_Ex(IntPtr vDC, Color color, int x, int y, int width, int height)
        {
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            FillCreate(vDC, color, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Rectangle(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="img"></param>
        /// <param name="color"></param>
        /// <param name="rc"></param>
        public static void FillRectangle_API_Ex(Image img, Color color, Rectangle rc)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            FillRectangle_API_Ex(vGraphics, color, rc);
        }

        /// <summary>
        /// Draw a rectangle
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="color"></param>
        /// <param name="rc"></param>
        public static void FillRectangle_API_Ex(Graphics vGraphics, Color color, Rectangle rc)
        {
            FillRectangle_API_Ex(vGraphics, color, rc.X, rc.Y, rc.Width, rc.Height);
        }

        #endregion

        #region Draw / Fill ellipse

        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawEllipse_API_Ex(Image img, Pen pen, int x, int y, int width, int height)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawEllipse_API_Ex(vGraphics, pen, x, y, width, height);
        }

        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawEllipse_API_Ex(Graphics vGraphics, Pen pen, int x, int y, int width, int height)
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            DrawCreate(vDC, pen, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Ellipse(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
            vGraphics.ReleaseHdc(vDC);
        }

        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="pen"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawEllipse_API_Ex(IntPtr vDC, Pen pen, int x, int y, int width, int height)
        {
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            DrawCreate(vDC, pen, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Ellipse(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
        }

        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pen"></param>
        /// <param name="rc"></param>
        public static void DrawEllipse_API_Ex(Image img, Pen pen, Rectangle rc)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawEllipse_API_Ex(vGraphics, pen, rc);
        }

        /// <summary>
        /// Draw ellipse
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="pen"></param>
        /// <param name="rc"></param>
        public static void DrawEllipse_API_Ex(Graphics vGraphics, Pen pen, Rectangle rc)
        {
            DrawEllipse_API_Ex(vGraphics, pen, rc.X, rc.Y, rc.Width, rc.Height);
        }

        /// <summary>
        /// Filled Ellipse
        /// </summary>
        /// <param name="img"></param>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FillEllipse_API_Ex(Image img, Color color, int x, int y, int width, int height)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            FillEllipse_API_Ex(vGraphics, color, x, y, width, height);
        }

        /// <summary>
        /// Filled Ellipse
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FillEllipse_API_Ex(Graphics vGraphics, Color color, int x, int y, int width, int height)
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            FillCreate(vDC, color, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Ellipse(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
            vGraphics.ReleaseHdc(vDC);
        }

        /// <summary>
        /// Filled Ellipse
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="color"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void FillEllipse_API_Ex(IntPtr vDC, Color color, int x, int y, int width, int height)
        {
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            FillCreate(vDC, color, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            WindowsAPI.Ellipse(vDC, x, y, x + width, y + height);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
        }

        /// <summary>
        /// Filled Ellipse
        /// </summary>
        /// <param name="img"></param>
        /// <param name="color"></param>
        /// <param name="rc"></param>
        public static void FillEllipse_API_Ex(Image img, Color color, Rectangle rc)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            FillEllipse_API_Ex(vGraphics, color, rc);
        }

        /// <summary>
        /// Filled Ellipse
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="color"></param>
        /// <param name="rc"></param>
        public static void FillEllipse_API_Ex(Graphics vGraphics, Color color, Rectangle rc)
        {
            FillEllipse_API_Ex(vGraphics, color, rc.X, rc.Y, rc.Width, rc.Height);
        }

        #endregion

        #region Draw a straight line

        /// <summary>
        /// Draw a straight line
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pen"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void DrawLine_API_Ex(Image img, Pen pen, Point pt1, Point pt2)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawLine_API_Ex(vGraphics, pen, pt1, pt2);
        }

        /// <summary>
        /// Draw a straight line
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="pen"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void DrawLine_API_Ex(Graphics vGraphics, Pen pen, Point pt1, Point pt2)
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            DrawCreate(vDC, pen, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            Point pt = new Point();
            WindowsAPI.MoveToEx(vDC, pt1.X, pt1.Y, ref pt);
            WindowsAPI.LineTo(vDC, pt2.X, pt2.Y);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
            vGraphics.ReleaseHdc(vDC);
        }

        /// <summary>
        /// Draw a straight line
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="pen"></param>
        /// <param name="pt1"></param>
        /// <param name="pt2"></param>
        public static void DrawLine_API_Ex(IntPtr vDC, Pen pen, Point pt1, Point pt2)
        {
            IntPtr vPreviouseBrush, vPreviousePen, vBrush, vPen;
            DrawCreate(vDC, pen, out vPreviouseBrush, out vPreviousePen, out vBrush, out vPen);
            Point pt = new Point();
            WindowsAPI.MoveToEx(vDC, pt1.X, pt1.Y, ref pt);
            WindowsAPI.LineTo(vDC, pt2.X, pt2.Y);
            GDIRelease(vDC, vPreviousePen, vPreviousePen, vBrush, vPen);
        }

        /// <summary>
        /// Draw a straight line
        /// </summary>
        /// <param name="img"></param>
        /// <param name="pen"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static void DrawLine_API_Ex(Image img, Pen pen, int x1, int y1, int x2, int y2)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawLine_API_Ex(vGraphics, pen, x1, y1, x2, y2);
        }

        /// <summary>
        /// Draw a straight line
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="pen"></param>
        /// <param name="x1"></param>
        /// <param name="y1"></param>
        /// <param name="x2"></param>
        /// <param name="y2"></param>
        public static void DrawLine_API_Ex(Graphics vGraphics, Pen pen, int x1, int y1, int x2, int y2)
        {
            DrawLine_API_Ex(vGraphics, pen, new Point(x1, y1), new Point(x2, y2));
        }

        #endregion
        
        #region Fill area

        /// <summary>
        /// Fill area
        /// </summary>
        /// <param name="img"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void FloodFill1(Image img, int x, int y, Color color)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            FloodFill1(vGraphics, x, y, color);
        }

        /// <summary>
        /// Fill area
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void FloodFill1(Graphics vGraphics, int x, int y, Color color)
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vBrush = WindowsAPI.CreateSolidBrush(ColorTranslator.ToWin32(color));
            IntPtr vPreviouseBrush = WindowsAPI.SelectObject(vDC, vBrush);
            WindowsAPI.ExtFloodFill(vDC, x, y, WindowsAPI.GetPixel(vDC, x, y), CommonConst.FLOODFILLSURFACE);
            WindowsAPI.SelectObject(vDC, vPreviouseBrush);
            WindowsAPI.DeleteObject(vBrush);
        }

        /// <summary>
        /// Fill area
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void FloodFill1(IntPtr vDC, int x, int y, Color color)
        {
            IntPtr vBrush = WindowsAPI.CreateSolidBrush(ColorTranslator.ToWin32(color));
            IntPtr vPreviouseBrush = WindowsAPI.SelectObject(vDC, vBrush);
            WindowsAPI.ExtFloodFill(vDC, x, y, WindowsAPI.GetPixel(vDC, x, y), CommonConst.FLOODFILLSURFACE);
            WindowsAPI.SelectObject(vDC, vPreviouseBrush);
            WindowsAPI.DeleteObject(vBrush);
        }

        /// <summary>
        /// Fill area
        /// </summary>
        /// <param name="img"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void FloodFill2(Image img, int x, int y, Color color)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            FloodFill2(vGraphics, x, y, color);
        }

        /// <summary>
        /// Fill area
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void FloodFill2(Graphics vGraphics, int x, int y, Color color)
        {
            IntPtr vDC = vGraphics.GetHdc();
            IntPtr vBrush = WindowsAPI.CreateSolidBrush(ColorTranslator.ToWin32(color));
            IntPtr vPreviouseBrush = WindowsAPI.SelectObject(vDC, vBrush);
            WindowsAPI.ExtFloodFill(vDC, x, y, WindowsAPI.GetPixel(vDC, x, y), CommonConst.FLOODFILLBORDER);
            WindowsAPI.SelectObject(vDC, vPreviouseBrush);
            WindowsAPI.DeleteObject(vBrush);
        }

        /// <summary>
        /// Fill area
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        public static void FloodFill2(IntPtr vDC, int x, int y, Color color)
        {
            IntPtr vBrush = WindowsAPI.CreateSolidBrush(ColorTranslator.ToWin32(color));
            IntPtr vPreviouseBrush = WindowsAPI.SelectObject(vDC, vBrush);
            WindowsAPI.ExtFloodFill(vDC, x, y, WindowsAPI.GetPixel(vDC, x, y), CommonConst.FLOODFILLBORDER);
            WindowsAPI.SelectObject(vDC, vPreviouseBrush);
            WindowsAPI.DeleteObject(vBrush);
        }

        #endregion

        #region Draw Border

        /// <summary>
        /// Draw Border
        /// </summary>
        /// <param name="img"></param>
        /// <param name="rc"></param>
        public static void DrawEdge_API_Ex(Image img, Rectangle rc)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawEdge_API_Ex(vGraphics, rc);
        }

        /// <summary>
        /// Draw Border
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="rc"></param>
        public static void DrawEdge_API_Ex(Graphics vGraphics, Rectangle rc)
        {
            IntPtr vDC = vGraphics.GetHdc();
            RECT rect = new RECT(rc);
            WindowsAPI.DrawEdge(vDC, ref rect, CommonConst.BDR_RAISEDINNER | CommonConst.BDR_SUNKENOUTER, CommonConst.BF_RECT | CommonConst.BF_FLAT);
            
        }

        /// <summary>
        /// Draw Border
        /// </summary>
        /// <param name="img"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawEdge_API_Ex(Image img, int x, int y, int width, int height)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawEdge_API_Ex(vGraphics, x, y, width, height);
        }

        /// <summary>
        /// Draw Border
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawEdge_API_Ex(Graphics vGraphics, int x, int y, int width, int height)
        {
            
            DrawEdge_API_Ex(vGraphics, new RECT(x, y, width, height));
        }

        #endregion

        #region Dotted rectangle drawn

        /// <summary>
        /// Dotted rectangle drawn
        /// </summary>
        /// <param name="img"></param>
        /// <param name="rc"></param>
        public static void DrawFocusRect_API_Ex(Image img, Rectangle rc)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawFocusRect_API_Ex(vGraphics, rc);
        }

        /// <summary>
        /// Dotted rectangle drawn
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="rc"></param>
        public static void DrawFocusRect_API_Ex(Graphics vGraphics, Rectangle rc)
        {
            IntPtr vDC = vGraphics.GetHdc();
            RECT rect = new RECT(rc);
            WindowsAPI.DrawFocusRect(vDC, ref rect);
        }

        /// <summary>
        /// Dotted rectangle drawn
        /// </summary>
        /// <param name="img"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawFocusRect_API_Ex(Image img, int x, int y, int width, int height)
        {
            Graphics vGraphics = Graphics.FromImage(img);
            DrawFocusRect_API_Ex(vGraphics, x, y, width, height);
        }

        /// <summary>
        /// Dotted rectangle drawn
        /// </summary>
        /// <param name="vGraphics"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public static void DrawFocusRect_API_Ex(Graphics vGraphics, int x, int y, int width, int height)
        {
            DrawFocusRect_API_Ex(vGraphics, new RECT(x, y, width, height));
        }

        #endregion
        
        #region Draw dot-matrix

        /// <summary>
        /// Draw dot-matrix
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap DrawSquare(Image Source, int size, Color color)
        {
            if (Source == null || size < 0)
                return null;
            Bitmap dest = (Bitmap)Source.Clone();
            int width = dest.Size.Width, height = dest.Size.Height;
            Graphics g = Graphics.FromImage(dest);
            for (int i = size; i < width; i += size * 2)
            {
                for (int j = size; j < height; j += size * 2)
                {
                    g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(i, j), new Size(size, size)));
                }
            }
            return dest;
        }

        /// <summary>
        /// Draw dot-matrix
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        /// <returns></returns>
        public static Bitmap DrawSquareEx(Image Source, int size, Color color)
        {
            if (Source == null || size < 0)
                return null;
            Bitmap dest = (Bitmap)Source.Clone();
            int width = dest.Size.Width, height = dest.Size.Height;
            Graphics g = Graphics.FromImage(dest);
            int style = 0;
            for (int i = 0; i < height; i += size)
            {
                if (style == size)
                    style = 0;
                else
                    style = size;
                for (int j = style; j < width; j += size * 2)
                {
                    g.FillRectangle(new SolidBrush(color), new Rectangle(new Point(i, j), new Size(size, size)));
                }
            }
            return dest;
        }

        #endregion

        #region Photo Synthesis

        /// <summary>
        /// Photo Synthesis
        /// </summary>
        /// <param name="Destination"></param>
        /// <param name="Source"></param>
        /// <param name="X"></param>
        /// <param name="Y"></param>
        /// <returns></returns>
        public static Bitmap CombineBitmap(Image Destination, Image Source, int X, int Y)
        {
            if (Destination == null || Source == null)
                return null;
            Image source = (Image)Destination.Clone();
            Graphics g = Graphics.FromImage(source);
            g.DrawImage(Source, X, Y);
            return new Bitmap(source);
        }

        /// <summary>
        /// Photo Synthesis
        /// </summary>
        /// <param name="Destination"></param>
        /// <param name="Source"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public static Bitmap CombineBitmap(Image Destination, Image Source, Point p)
        {
            if (Destination == null || Source == null)
                return null;
            Image source = (Image)Destination.Clone();
            Graphics g = Graphics.FromImage(source);
            g.DrawImage(Source, p.X, p.Y);
            return new Bitmap(source);
        }

        #endregion

        #region Color reversal

        /// <summary>
        /// Color reversal
        /// </summary>
        /// <param name="bmp"></param>
        /// <returns></returns>
        public static bool Invert(ref Bitmap bmp)
        {
            P2PFunc func = new P2PFunc(InvertFunc);

            return P2PCore(ref bmp, func);
        }

        private static void InvertFunc(ref byte r, ref byte g, ref byte b, params double[] param)
        {
            r = (byte)(255 - r);
            g = (byte)(255 - g);
            b = (byte)(255 - b);
        }

        private delegate void P2PFunc(ref byte r, ref byte g, ref byte b, params double[] param);

        private static bool P2PCore(ref Bitmap bmp, P2PFunc func, params double[] param)
        {
            if (bmp.PixelFormat != PixelFormat.Format24bppRgb)
                return false;

            int w = bmp.Width;
            int h = bmp.Height;

            byte r, g, b;
            int ir;

            BmpProc24 bd = new BmpProc24(bmp);

            for (int y = 0; y < h; y++)
                for (int x = 0; x < w; x++)
                {
                    ir = bd.IndexR(x, y);

                    r = bd[ir]; g = bd[ir - 1]; b = bd[ir - 2];

                    func(ref r, ref g, ref b, param);

                    bd[ir] = r; bd[ir - 1] = g; bd[ir - 2] = b;
                }

            bd.Dispose();

            return true;
        }

        private class BmpProc24 : IDisposable
        {
            private bool flagDispose = false;

            private Bitmap rbmp;
            private int w, h;
            private BitmapData bmpData;
            private IntPtr ptr;
            private int stride;
            private int bytes;
            private byte[] data;
            

            public BmpProc24(Bitmap bmp)
            {
                rbmp = bmp;
                w = bmp.Width;
                h = bmp.Height;
                Rectangle rect = new Rectangle(0, 0, w, h);
                bmpData = bmp.LockBits(rect, ImageLockMode.ReadWrite,
                                              PixelFormat.Format24bppRgb);
                ptr = bmpData.Scan0;
                stride = Math.Abs(bmpData.Stride);

                bytes = stride * h;
                data = new byte[bytes];
                Marshal.Copy(ptr, data, 0, bytes);
            }

           
            public byte this[int index]
            {
                get { return data[index]; }
                set { data[index] = value; }
            }

            public int IndexR(int x, int y)
            {
                return stride * y + x * 3 + 2;
            }
            
            protected virtual void Dispose(bool flag)
            {
                if (!flagDispose)
                {
                    if (flag)
                    {
                        Marshal.Copy(data, 0, ptr, bytes);
                        rbmp.UnlockBits(bmpData);
                    }
                    this.flagDispose = true;
                }
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            ~BmpProc24()
            {
                Dispose(false);
            }
        }

        
        #endregion

        #region GDI preparation

        /// <summary>
        /// Replacement of GDI objects
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="pen"></param>
        /// <param name="vPreviouseBrush"></param>
        /// <param name="vPreviousePen"></param>
        /// <param name="vBrush"></param>
        /// <param name="vPen"></param>
        public static void DrawCreate(IntPtr vDC, Pen pen, out IntPtr vPreviouseBrush, out IntPtr vPreviousePen, out IntPtr vBrush, out IntPtr vPen)
        {
            vBrush = WindowsAPI.GetStockObject(CommonConst.NULL_BRUSH);  //Create a transparent brush
            //vPen = WindowsAPI.CreatePen(CommonConst.PS_SOLID, Convert.ToInt32(pen.Width), ColorTranslator.ToWin32(pen.Color));
            vPen = CreateFlatPen(pen.Color, (uint)pen.Width);
            vPreviouseBrush = WindowsAPI.SelectObject(vDC, vBrush);
            vPreviousePen = WindowsAPI.SelectObject(vDC, vPen);
        }

        /// <summary>
        /// Restore and delete the newly created object
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="vPreviouseBrush"></param>
        /// <param name="vPreviousePen"></param>
        /// <param name="vBrush"></param>
        /// <param name="vPen"></param>
        public static void GDIRelease(IntPtr vDC, IntPtr vPreviouseBrush, IntPtr vPreviousePen, IntPtr vBrush, IntPtr vPen)
        {
            WindowsAPI.SelectObject(vDC, vPreviouseBrush);
            WindowsAPI.SelectObject(vDC, vPreviousePen);
            WindowsAPI.DeleteObject(vBrush);
            WindowsAPI.DeleteObject(vPen);
        }

        /// <summary>
        /// Replacement of GDI objects
        /// </summary>
        /// <param name="vDC"></param>
        /// <param name="color"></param>
        /// <param name="vPreviouseBrush"></param>
        /// <param name="vPreviousePen"></param>
        /// <param name="vBrush"></param>
        /// <param name="vPen"></param>
        public static void FillCreate(IntPtr vDC, Color color, out IntPtr vPreviouseBrush, out IntPtr vPreviousePen, out IntPtr vBrush, out IntPtr vPen)
        {
            vBrush = WindowsAPI.CreateSolidBrush(ColorTranslator.ToWin32(color));
            vPreviouseBrush = WindowsAPI.SelectObject(vDC, vBrush);
            vPen = WindowsAPI.CreatePen(CommonConst.PS_SOLID, 1, ColorTranslator.ToWin32(color));
            vPreviousePen = WindowsAPI.SelectObject(vDC, vPen);
        }

        /// <summary>
        /// Create a circular pen
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static IntPtr CreateRoundPen(Color color, uint width)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(color);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_ROUND | CommonConst.PS_JOIN_MITER, width, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a circular pen
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static IntPtr CreateRoundPen(uint width)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(Color.Black);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_ROUND | CommonConst.PS_JOIN_MITER, width, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a circular pen
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IntPtr CreateRoundPen(Color color)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(color);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_ROUND | CommonConst.PS_JOIN_MITER, 1, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a square pen
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static IntPtr CreateSquarePen(Color color, uint width)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(color);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_SQUARE | CommonConst.PS_JOIN_MITER, width, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a square pen
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static IntPtr CreateSquarePen(uint width)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(Color.Black);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_SQUARE | CommonConst.PS_JOIN_MITER, width, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a square pen
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IntPtr CreateSquarePen(Color color)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(color);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_SQUARE | CommonConst.PS_JOIN_MITER, 1, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a flat-shaped pen
        /// </summary>
        /// <param name="color"></param>
        /// <param name="width"></param>
        /// <returns></returns>
        public static IntPtr CreateFlatPen(Color color, uint width)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(color);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_FLAT | CommonConst.PS_JOIN_MITER, width, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a flat-shaped pen
        /// </summary>
        /// <param name="width"></param>
        /// <returns></returns>
        public static IntPtr CreateFlatPen(uint width)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(Color.Black);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_FLAT | CommonConst.PS_JOIN_MITER, width, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create a flat-shaped pen
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IntPtr CreateFlatPen(Color color)
        {
            LOGBRUSH lp = new LOGBRUSH();
            lp.lbColor = WindowsAPI.RGB(color);
            IntPtr vPen = WindowsAPI.ExtCreatePen(CommonConst.PS_GEOMETRIC | CommonConst.PS_SOLID | CommonConst.PS_ENDCAP_FLAT | CommonConst.PS_JOIN_MITER, 1, ref lp, 0, null);
            return vPen;
        }

        /// <summary>
        /// Create an empty brush
        /// </summary>
        public static IntPtr CreateNullBrush()
        {
            IntPtr vBrush = WindowsAPI.GetStockObject(CommonConst.NULL_BRUSH);  //Create a transparent brush
            return vBrush;
        }

        /// <summary>
        /// Create colored brush
        /// </summary>
        /// <param name="color"></param>
        /// <returns></returns>
        public static IntPtr CreateColorBrush(Color color)
        {
            IntPtr vBrush = WindowsAPI.CreateSolidBrush(ColorTranslator.ToWin32(color));
            return vBrush;
        }

        /// <summary>
        /// Create an empty document
        /// </summary>
        public static IntPtr CreateNullPen()
        {
            IntPtr vPen = WindowsAPI.GetStockObject(CommonConst.NULL_PEN);
            return vPen;
        }

        /// <summary>
        /// Replace Color brush
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="color"></param>
        public static void ChangeBrushColor(IntPtr hdc, Color color)
        {
            WindowsAPI.SetDCBrushColor(hdc, WindowsAPI.RGB(color));
        }

        /// <summary>
        /// Replacement pen color
        /// </summary>
        /// <param name="hdc"></param>
        /// <param name="color"></param>
        public static void ChangePenColor(IntPtr hdc, Color color)
        {
            WindowsAPI.SetDCPenColor(hdc, WindowsAPI.RGB(color));
        }

        #endregion

        #region The process of drawing class

        private class DrawProcess
        {
            private ShapeType st;  //Draw Type
            private Point sl, p;  //Starting point coordinates and end coordinates
            private bool md;  //Is the mouse to leave the event
            private Size sz;  //Canvas Size
            private Pen pen;  //Current pen
            private Brush brush;  //Current brush
            private int filltype;  //Fill type used for drawing geometry
            private List<LinePoint> lp = new List<LinePoint>();  //When drawing the path of pen or rubber
            private List<Point> pp = new List<Point>();  //Draw the path of polygons
            private List<Point> cp = new List<Point>();  //Curved path

            public DrawProcess(ShapeType st, Point sl, Point p, bool md, Size sz, Pen pen, Brush brush, int filltype, LinePoint lp)
            {
                this.st = st;
                this.sl = sl;
                this.p = p;
                this.md = md;
                this.sz = sz;
                this.pen = pen;
                this.brush = brush;
                this.filltype = filltype;
                if (lp != LinePoint.Empty)
                    this.lp.Add(lp);
            }

            /// <summary>
            /// Draw Type
            /// </summary>
            public ShapeType ShapeTypeValue
            {
                get { return this.st; }
            }

            /// <summary>
            /// Start pos
            /// </summary>
            public Point StartPoint
            {
                get { return this.sl; }
            }

            /// <summary>
            /// End coordinates
            /// </summary>
            public Point EndPoint
            {
                get { return this.p; }
            }

            /// <summary>
            /// Is the mouse to leave the event (that is, to determine whether the MouseUp event)
            /// </summary>
            public bool MouseDown
            {
                get { return this.md; }
            }

            /// <summary>
            /// Canvas Size
            /// </summary>
            public Size CanvasSize
            {
                get { return this.sz; }
            }

            /// <summary>
            /// Current pen
            /// </summary>
            public Pen PenUsed
            {
                get { return this.pen; }
            }

            /// <summary>
            /// Current brush
            /// </summary>
            public Brush BrushUsed
            {
                get { return this.brush; }
            }

            /// <summary>
            /// Fill type used for drawing geometry
            /// </summary>
            public int FillType
            {
                get { return this.filltype; }
            }

            /// <summary>
            /// When drawing the path of pen or rubber
            /// </summary>
            public List<LinePoint> LinePoints
            {
                get { return this.lp; }
            }

            /// <summary>
            /// Add a pen or the eraser path
            /// </summary>
            /// <param name="lp"></param>
            public void AddLinePoint(LinePoint lp)
            {
                if (lp != LinePoint.Empty)
                    this.lp.Add(lp);
            }

            /// <summary>
            /// Draw the path of polygons
            /// </summary>
            public List<Point> PolygonPoints
            {
                get { return this.pp; }
            }

            /// <summary>
            /// Drawing curved path
            /// </summary>
            public List<Point> CurvePoints
            {
                get { return this.cp; }
            }
        }

        /// <summary>
        /// Create a pen or rubber path
        /// </summary>
        private struct LinePoint
        {
            public Point startpoint;
            public Point endpoint;

            public LinePoint(Point startpoint, Point endpoint)
            {
                this.startpoint = startpoint;
                this.endpoint = endpoint;
            }

            /// <summary>
            /// Determine the current LinePoint object is empty
            /// </summary>
            public static LinePoint Empty = new LinePoint();

            /// <summary>
            /// Overloading == to compare objects to Empty
            /// </summary>
            /// <param name="lp1"></param>
            /// <param name="lp2"></param>
            /// <returns></returns>
            public static bool operator ==(LinePoint lp1, LinePoint lp2)
            {
                if (lp1.startpoint == lp2.startpoint && lp1.endpoint == lp2.endpoint)
                    return true;
                return false;
            }

            /// <summary>
            /// Heavy! = Used to compare the object with the Empty
            /// </summary>
            /// <param name="lp1"></param>
            /// <param name="lp2"></param>
            /// <returns></returns>
            public static bool operator !=(LinePoint lp1, LinePoint lp2)
            {
                if (lp1.startpoint != lp2.startpoint || lp1.endpoint != lp2.endpoint)
                    return true;
                return false;
            }

            public override bool Equals(object obj)
            {
                if (((LinePoint)obj).startpoint == this.startpoint && ((LinePoint)obj).endpoint == this.endpoint)
                    return true;
                return false;
                //return base.Equals(obj);
            }
        }

        #endregion
    }
}