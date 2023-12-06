using System;
using System.Text;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;

namespace MSPaint
{
    #region RECT

    [Serializable, StructLayout(LayoutKind.Sequential)]
    public struct RECT
    {
        public int left;
        public int top;
        public int right;
        public int bottom;

        public int width
        {
            get { return Math.Abs(this.right - this.left); }
        }

        public int height
        {
            get { return Math.Abs(this.bottom - this.top); }
        }

        public RECT(Rectangle rectangle)
        {
            this.left = rectangle.Left;
            this.right = rectangle.Right;
            this.top = rectangle.Top;
            this.bottom = rectangle.Bottom;
        }

        public RECT(int x, int y, int width, int height)
        {
            this.left = x;
            this.right = this.left + width;
            this.top = y;
            this.bottom = this.top + height;
        }

        public RECT(Point pt, Size size)
        {
            this.left = pt.X;
            this.right = pt.X + size.Width;
            this.top = pt.Y;
            this.bottom = pt.Y + size.Height;
        }

        public override string ToString()
        {
            return "{X=" + this.left.ToString() + ",Y=" + this.top.ToString() + ",Width=" + this.width.ToString() + ",Height=" + this.height.ToString() + "}";
        }

        public bool Contains(Point pt)
        {
            return WindowsAPI.PtInRect(ref this, pt);
        }

        public Size Size
        {
            get { return new Size(this.width, this.height); }
        }

        public Point Location
        {
            get { return new Point(this.left, this.top); }
        }

        public bool IsEmpty
        {
            get { return WindowsAPI.IsRectEmpty(ref this); }
        }

        public static implicit operator Rectangle(RECT rect)
        {
            return new Rectangle(rect.left, rect.top, rect.width, rect.height);
        }
    }

    #endregion

    #region LOGBRUSH

    [StructLayout(LayoutKind.Sequential)]
    public struct LOGBRUSH
    {
        public uint lbStyle;        //brush style
        public UInt32 lbColor;    //colorref RGB(...)
        public HatchStyle lbHatch;        //hatch style
    }

    #endregion

    #region API_const

    /// <summary>
    /// Windows API Constant
    /// </summary>
    public class CommonConst
    {
        #region PEN & BRUSH

        public const int WHITE_BRUSH = 0;
        public const int LTGRAY_BRUSH = 1;
        public const int GRAY_BRUSH = 2;
        public const int DKGRAY_BRUSH = 3;
        public const int BLACK_BRUSH = 4;
        public const int NULL_BRUSH = 5;
        public const int HOLLOW_BRUSH = NULL_BRUSH;
        public const int WHITE_PEN = 6;
        public const int BLACK_PEN = 7;
        public const int NULL_PEN = 8;
        public const int OEM_FIXED_FONT = 10;
        public const int ANSI_FIXED_FONT = 11;
        public const int ANSI_VAR_FONT = 12;
        public const int SYSTEM_FONT = 13;
        public const int DEVICE_DEFAULT_FONT = 14;
        public const int DEFAULT_PALETTE = 15;
        public const int SYSTEM_FIXED_FONT = 16;
        public const int DEFAULT_GUI_FONT = 17;
        public const int DC_BRUSH = 18;
        public const int DC_PEN = 19;

        #endregion

        public const int SRCCOPY = 0x00CC0020;
        public const int CAPTUREBLT = 0x40000000;
		

        #region PS
		

        public const int PS_SOLID = 0;
        public const int PS_DASH = 1;
        public const int PS_DOT = 2;
        public const int PS_DASHDOT = 3;
        public const int PS_DASHDOTDOT = 4;
        public const int PS_NULL = 5;
        public const int PS_INSIDEFRAME = 6;
        public const int PS_USERSTYLE = 7;
        public const int PS_ALTERNATE = 8;
        public const int PS_STYLE_MASK = 0xF;
        public const int PS_ENDCAP_ROUND = 0x0;
        public const int PS_ENDCAP_SQUARE = 0x100;
        public const int PS_ENDCAP_FLAT = 0x200;
        public const int PS_ENDCAP_MASK = 0xF00;
        public const int PS_JOIN_ROUND = 0x0;
        public const int PS_JOIN_BEVEL = 0x1000;
        public const int PS_JOIN_MITER = 0x2000;
        public const int PS_JOIN_MASK = 0xF000;
        public const int PS_COSMETIC = 0x0;
        public const int PS_GEOMETRIC = 0x10000;
        public const int PS_TYPE_MASK = 0xF0000;

        #endregion
		

        #region FLOODFILL

        public const uint FLOODFILLBORDER = 0;
        public const uint FLOODFILLSURFACE = 1;

        #endregion
        
        #region BDR

        public const int BDR_RAISEDOUTER = 0x1;
        public const int BDR_SUNKENOUTER = 0x2;
        public const int BDR_RAISEDINNER = 0x4;
        public const int BDR_SUNKENINNER = 0x8;

        #endregion
        
        #region BF

        public const int BF_LEFT = 0x1;
        public const int BF_TOP = 0x2;
        public const int BF_RIGHT = 0x4;
        public const int BF_BOTTOM = 0x8;
        public const int BF_TOPLEFT = (BF_TOP | BF_LEFT);
        public const int BF_TOPRIGHT = (BF_TOP | BF_RIGHT);
        public const int BF_BOTTOMLEFT = (BF_BOTTOM | BF_LEFT);
        public const int BF_BOTTOMRIGHT = (BF_BOTTOM | BF_RIGHT);
        public const int BF_RECT = (BF_LEFT | BF_TOP | BF_RIGHT | BF_BOTTOM);
        public const int BF_DIAGONAL = 0x10;
        public const int BF_DIAGONAL_ENDTOPRIGHT = (BF_DIAGONAL | BF_TOP | BF_RIGHT);
        public const int BF_DIAGONAL_ENDTOPLEFT = (BF_DIAGONAL | BF_TOP | BF_LEFT);
        public const int BF_DIAGONAL_ENDBOTTOMLEFT = (BF_DIAGONAL | BF_BOTTOM | BF_LEFT);
        public const int BF_DIAGONAL_ENDBOTTOMRIGHT = (BF_DIAGONAL | BF_BOTTOM | BF_RIGHT);
        public const int BF_MIDDLE = 0x800;
        public const int BF_SOFT = 0x1000;
        public const int BF_ADJUST = 0x2000;
        public const int BF_FLAT = 0x4000;
        public const int BF_MONO = 0x8000;

        #endregion
        
        #region DFC

        public const uint DFC_CAPTION = 1;
        public const uint DFC_MENU = 2;
        public const uint DFC_SCROLL = 3;
        public const uint DFC_BUTTON = 4;
        public const uint DFC_POPUPMENU = 5;

        #endregion

        #region DFCS

        public const uint DFCS_CAPTIONCLOSE = 0;
        public const uint DFCS_CAPTIONMIN = 1;
        public const uint DFCS_CAPTIONMAX = 2;
        public const uint DFCS_CAPTIONRESTORE = 3;
        public const uint DFCS_CAPTIONHELP = 4;

        public const uint DFCS_MENUARROW = 0;
        public const uint DFCS_MENUCHECK = 1;
        public const uint DFCS_MENUBULLET = 2;
        public const uint DFCS_MENUARROWRIGHT = 4;

        public const uint DFCS_SCROLLUP = 0;
        public const uint DFCS_SCROLLDOWN = 1;
        public const uint DFCS_SCROLLLEFT = 2;
        public const uint DFCS_SCROLLRIGHT = 3;
        public const uint DFCS_SCROLLCOMBOBOX = 5;
        public const uint DFCS_SCROLLSIZEGRIP = 8;
        public const uint DFCS_SCROLLSIZEGRIPRIGHT = 0x10;

        public const uint DFCS_BUTTONCHECK = 0;
        public const uint DFCS_BUTTONRADIOIMAGE = 1;
        public const uint DFCS_BUTTONRADIOMASK = 2;
        public const uint DFCS_BUTTONRADIO = 4;
        public const uint DFCS_BUTTON3STATE = 8;
        public const uint DFCS_BUTTONPUSH = 0x10;

        public const uint DFCS_INACTIVE = 0x100;
        public const uint DFCS_PUSHED = 0x200;
        public const uint DFCS_CHECKED = 0x400;
        public const uint DFCS_TRANSPARENT = 0x800;
        public const uint DFCS_HOT = 0x1000;
        public const uint DFCS_ADJUSTRECT = 0x2000;
        public const uint DFCS_FLAT = 0x4000;
        public const uint DFCS_MONO = 0x8000;

        #endregion
        
    }

    #endregion

    public class WindowsAPI
    {
        #region SystemParametersInfo

        [DllImport("user32.dll", EntryPoint = "SystemParametersInfo")]
        public static extern bool SystemParametersInfo(uint uiAction, uint uiParam, string pvParam, uint fuWinIni);

        #endregion

        #region IsRectEmpty

        [DllImport("user32.dll")]
        public static extern bool IsRectEmpty(ref RECT lprc);

        #endregion

        #region SelectObject

        [DllImport("gdi32.dll")]
        public static extern IntPtr SelectObject(IntPtr hdc, IntPtr hgdiobj);

        #endregion

        #region ReleaseDC

        [DllImport("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hDC);

        #endregion

        #region RoundRect

        [DllImport("gdi32.dll")]
        public static extern bool RoundRect(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect, int nWidth, int nHeight);

        #endregion

        #region PtInRect

        [DllImport("user32.dll")]
        public static extern bool PtInRect(ref RECT lprc, Point pt);

        #endregion

        #region CreateSolidBrush Heavy _2

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(uint crColor);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateSolidBrush(int crColor);

        #endregion

        #region CreatePen Heavy _2

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, int crColor);

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreatePen(int fnPenStyle, int nWidth, uint crColor);

        #endregion

        #region SelectClipPath

        [DllImport("gdi32.dll")]
        public static extern bool SelectClipPath(IntPtr hdc, int iMode);

        #endregion

        #region GetStockObject

        [DllImport("gdi32.dll")]
        public static extern IntPtr GetStockObject(int fnObject);

        #endregion

        #region SetDCPenColor

        [DllImport("gdi32.dll")]
        public static extern uint SetDCPenColor(IntPtr hdc, uint crColor);

        #endregion

        #region SetDCBrushColor

        [DllImport("gdi32.dll")]
        public static extern uint SetDCBrushColor(IntPtr hdc, uint crColor);

        #endregion
        
        #region CreateCompatibleDC

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleDC(IntPtr hdc);

        #endregion

        #region CreateCompatibleBitmap

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateCompatibleBitmap(IntPtr hdc, int nWidth, int nHeight);

        #endregion

        #region DeleteObject

        [DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        #endregion

        #region Rectangle

        [DllImport("gdi32.dll")]
        public static extern bool Rectangle(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        #endregion

        #region Ellipse

        [DllImport("gdi32.dll")]
        public static extern bool Ellipse(IntPtr hdc, int nLeftRect, int nTopRect, int nRightRect, int nBottomRect);

        #endregion
        
        #region ExtCreatePen

        [DllImport("gdi32.dll", ExactSpelling = true, PreserveSig = true, SetLastError = true)]
        public static extern IntPtr ExtCreatePen(uint dwPenStyle, uint dwWidth, [In] ref LOGBRUSH lplb, uint dwStyleCount, uint[] lpStyle);

        #endregion

        #region FloodFill

        [DllImport("gdi32.dll")]
        static extern bool FloodFill(IntPtr hdc, int nXStart, int nYStart, uint crFill);

        #endregion

        #region MoveToEx

        [DllImport("gdi32.dll")]
        public static extern bool MoveToEx(IntPtr hdc, int X, int Y, ref Point lpPoint);

        #endregion

        #region GetPixel

        [DllImport("gdi32.dll")]
        public static extern int GetPixel(IntPtr hdc, int nXPos, int nYPos);

        #endregion

        #region LineTo

        [DllImport("gdi32.dll")]
        public static extern bool LineTo(IntPtr hdc, int nXEnd, int nYEnd);

        #endregion

        #region ExtFloodFill Heavy _2

        [DllImport("gdi32.dll")]
        public static extern bool ExtFloodFill(IntPtr hdc, int nXStart, int nYStart, uint crColor, uint fuFillType);

        [DllImport("gdi32.dll")]
        public static extern bool ExtFloodFill(IntPtr hdc, int nXStart, int nYStart, int crColor, uint fuFillType);

        #endregion

        #region DrawEdge

        [DllImport("user32.dll")]
        public static extern bool DrawEdge(IntPtr hdc, ref RECT qrc, uint edge, uint grfFlags);

        #endregion

        #region DrawFocusRect

        [DllImport("user32.dll")]
        public static extern bool DrawFocusRect(IntPtr hDC, [In] ref RECT lprc);

        #endregion

        #region CreateBrushIndirect

        [DllImport("gdi32.dll")]
        public static extern IntPtr CreateBrushIndirect([In] ref LOGBRUSH lplb);

        #endregion

        #region PolyBezier

        [DllImport("gdi32.dll")]
        public static extern bool PolyBezier(IntPtr hdc, Point[] lppt, uint cPoints);

        #endregion

        #region DrawFrameControl

        [DllImport("user32.dll")]
        public static extern bool DrawFrameControl(IntPtr hdc, [In] ref RECT lprc, uint uType, uint uState);

        #endregion

        #region SetPixel Heavy _2

        [DllImport("gdi32.dll")]
        public static extern int SetPixel(IntPtr hdc, int X, int Y, int crColor);

        [DllImport("gdi32.dll")]
        public static extern int SetPixel(IntPtr hdc, int X, int Y, uint crColor);

        #endregion

        #region PolyBezierTo

        [DllImport("gdi32.dll")]
        public static extern bool PolyBezierTo(IntPtr hdc, Point[] lppt, uint cCount);

        #endregion

        #region GetDeviceCaps

        [DllImport("gdi32.dll")]
        public static extern int GetDeviceCaps(IntPtr hdc, int nIndex);

        #endregion

        #region GetDC

        [DllImport("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hWnd);

        #endregion
        /*
        #region GetDesktopWindow

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        #endregion
        */

        #region RGB

        public static uint RGB(Color color)
        {
            byte byRed, byGreen, byBlue, RESERVED;

            byRed = color.R;
            byGreen = color.G;
            byBlue = color.B;
            RESERVED = 0;
            byte[] RGBCOLORS = new byte[4];
            RGBCOLORS[0] = byRed;
            RGBCOLORS[1] = byGreen;
            RGBCOLORS[2] = byBlue;
            RGBCOLORS[3] = RESERVED;
            return BitConverter.ToUInt32(RGBCOLORS, 0);
        }

        #endregion
    }

    //public class Accessory
    //{
        
    //}
}