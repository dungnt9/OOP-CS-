using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Collections;
using System.Drawing.Drawing2D;

namespace MSPaint
{
    public partial class PaintForm : Form
    {
        #region MainMenu

        MainMenu mainMenu1 = new MainMenu();

        MenuItem menuItem1 = new MenuItem();
        MenuItem menuItem2 = new MenuItem();
        MenuItem menuItem3 = new MenuItem();
        MenuItem menuItem4 = new MenuItem();
        MenuItem menuItem5 = new MenuItem();
        MenuItem menuItem6 = new MenuItem();
        MenuItem menuItem7 = new MenuItem();
        MenuItem menuItem8 = new MenuItem();
        MenuItem menuItem9 = new MenuItem();
        MenuItem menuItem10 = new MenuItem();
        MenuItem menuItem11 = new MenuItem();
        MenuItem menuItem12 = new MenuItem();
        MenuItem menuItem13 = new MenuItem();
        MenuItem menuItem14 = new MenuItem();
        MenuItem menuItem15 = new MenuItem();
        MenuItem menuItem16 = new MenuItem();
        MenuItem menuItem17 = new MenuItem();
        MenuItem menuItem18 = new MenuItem();
        MenuItem menuItem19 = new MenuItem();
        MenuItem menuItem20 = new MenuItem();
        MenuItem menuItem21 = new MenuItem();
       
        MenuItem menuItem23 = new MenuItem();
        MenuItem menuItem24 = new MenuItem();
        MenuItem menuItem25 = new MenuItem();
        MenuItem menuItem26 = new MenuItem();
       
        MenuItem menuItem65 = new MenuItem();
        MenuItem menuItem66 = new MenuItem();
        MenuItem menuItem67 = new MenuItem();
        

        #endregion

        private StatusBar statusbar;  //Status Bar
        public static StatusBarPanel sbpanel1 = new StatusBarPanel();  //The first status bar panel
        public static StatusBarPanel sbpanel2 = new StatusBarPanel();  //The second status bar panel
        public static StatusBarPanel sbpanel3 = new StatusBarPanel();  //The third status bar panel
        public SaveDialog sd = new SaveDialog();  //Save dialog box
        
        public string currentfilepath = "";  //The path of the current file
        /// <summary>
        /// 
        /// </summary>
        public PaintForm()
        {
            InitializeComponent();
            canvas1.paintform = this;  //Set canvas1 variables in PaintForm

            this.statusbar = new StatusBar();
            this.statusbar.ShowPanels = true;
            sbpanel1.Width = 250;
            sbpanel1.Text = "For Help, click Help Topics on the Help Menu.";
            sbpanel2.Width = 89;
            sbpanel3.Width = 209;
            this.statusbar.Panels.AddRange(new StatusBarPanel[] { sbpanel1, sbpanel2, sbpanel3 });

            this.Controls.Add(this.statusbar);
            
            sd.Owner = this;

            #region MainMenu

            menuItem1.Text = "File";
            menuItem2.Text = "Edit";
            menuItem3.Text = "View";
            menuItem4.Text = "Image";
            menuItem5.Text = "Color";
            menuItem6.Text = "Help";
            menuItem7.Text = "New...";
            menuItem7.Shortcut = Shortcut.CtrlN;
            menuItem8.Text = "Open...";
            menuItem8.Shortcut = Shortcut.CtrlO;
            menuItem9.Text = "Save";
            menuItem9.Shortcut = Shortcut.CtrlS;
            menuItem10.Text = "Save As...";
            menuItem11.Text = "-";
            menuItem12.Text = "From a scanner or camera...";
            menuItem13.Text = "-";
            menuItem14.Text = "Print Preview";
            menuItem15.Text = "Page Setup...";
            menuItem16.Text = "Print(&P)...";
            menuItem16.Shortcut = Shortcut.CtrlP;
            menuItem17.Text = "-";
            menuItem18.Text = "Send";
            menuItem19.Text = "-";
            menuItem20.Text = "Set As Background(Tiled)";
            menuItem21.Text = "Set As Background(Centered)";
            
            menuItem23.Text = "-";
            menuItem24.Text = "Most recently used files";
            menuItem25.Text = "-";
            menuItem26.Text = "Exit";
			menuItem26.Shortcut = Shortcut.AltF4;
			
            menuItem65.Text = "Help Topics";
            menuItem66.Text = "-";
            menuItem67.Text = "About Paint";
            

            mainMenu1.MenuItems.Add(menuItem1);
            mainMenu1.MenuItems.Add(menuItem2);
            mainMenu1.MenuItems.Add(menuItem3);
            mainMenu1.MenuItems.Add(menuItem4);
            mainMenu1.MenuItems.Add(menuItem5);
            mainMenu1.MenuItems.Add(menuItem6);

            Menu = mainMenu1;
            menuItem1.MenuItems.AddRange(new MenuItem[] { menuItem7, menuItem8, menuItem9, menuItem10, menuItem11, menuItem12, menuItem13, menuItem14, menuItem15, menuItem16, menuItem17, menuItem18, menuItem19, menuItem20, menuItem21,  menuItem23, menuItem24,  menuItem25, menuItem26 });
            
            menuItem6.MenuItems.AddRange(new MenuItem[] { menuItem65, menuItem66, menuItem67 });

            menuItem7.Click += new EventHandler(menuItem7_Click);
            menuItem8.Click += new EventHandler(menuItem8_Click);
            menuItem9.Click += new EventHandler(menuItem9_Click);
            menuItem10.Click += new EventHandler(menuItem10_Click);
            menuItem11.Click += new EventHandler(menuItem11_Click);
            menuItem12.Click += new EventHandler(menuItem12_Click);
            menuItem13.Click += new EventHandler(menuItem13_Click);
            menuItem14.Click += new EventHandler(menuItem14_Click);
            menuItem15.Click += new EventHandler(menuItem15_Click);
            menuItem16.Click += new EventHandler(menuItem16_Click);
            menuItem17.Click += new EventHandler(menuItem17_Click);
            menuItem18.Click += new EventHandler(menuItem18_Click);
            menuItem19.Click += new EventHandler(menuItem19_Click);
            
            
            menuItem23.Click += new EventHandler(menuItem23_Click);
            menuItem24.Click += new EventHandler(menuItem24_Click);
            menuItem25.Click += new EventHandler(menuItem25_Click);
            menuItem26.Click += new EventHandler(menuItem26_Click);
			
            menuItem65.Click += new EventHandler(menuItem65_Click);
            menuItem67.Click += new EventHandler(menuItem67_Click);
            

            menuItem1.Popup += new EventHandler(menuItem1_Popup);
            
            

            #endregion
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            this.canvas1.PaintImage = DrawShape.CreateBitmap(new Size(480, 331), Color.White);  //Create canvas
            this.toolPanel1.psform = this;
            
        }

        #region MainMenuEvent

        #region topmenu

        private void menuItem1_Popup(object sender, EventArgs e)
        {
           
            {
                menuItem20.Enabled = false;
                menuItem21.Enabled = false;
                
            }
        }

        
        #endregion

        #region file

        // New
        private void menuItem7_Click(object sender, EventArgs e)
        {
            string text = this.Text;
            DialogResult dr = DialogResult.Cancel;
            if (!canvas1.ds.saved)
            {
                if (sd.IsDisposed)
                    sd = new SaveDialog();
                dr = sd.ShowDialog();
            }
            if (dr == DialogResult.OK || canvas1.ds.saved)
            {
                canvas1.Initial();
                colors1.PenColor = Color.Black;
                colors1.BrushColor = Color.White;
                this.currentfilepath = "";
            }
            else
                this.Text = text;
        }

        // Open
        private void menuItem8_Click(object sender, EventArgs e)
        {
            string text = this.Text;
            DialogResult dr = DialogResult.Cancel;
            if (!canvas1.ds.saved)
            {
                if (sd.IsDisposed)
                    sd = new SaveDialog();
                dr = sd.ShowDialog();
            }
            
        }

        
        

        // Save
        public void menuItem9_Click(object sender, EventArgs e)
        {
            if (!canvas1.ds.saved)
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    switch (saveFileDialog1.FilterIndex)
                    {
                        case 0:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case 1:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case 2:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case 3:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                            break;
                        case 4:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                            break;
                        case 5:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                            break;
                        case 6:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                            break;
                        case 7:
                            canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                            break;
                    }
                    menuItem20.Enabled = true;
                    menuItem21.Enabled = true;
                    
                    canvas1.ds.saved = true;
                    this.Text = Path.GetFileName(saveFileDialog1.FileName) + " - Paint";
                    this.currentfilepath = saveFileDialog1.FileName;
                }
            }
            else
            {

                switch (saveFileDialog1.FilterIndex)
                {
                    case 0:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 1:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 2:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 4:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 5:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 6:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    case 7:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }
                this.currentfilepath = saveFileDialog1.FileName;
            }
        }

        // Save As
        #region Vung test
        public void menuItem10_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                switch (saveFileDialog1.FilterIndex)
                {
                    case 0:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 1:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 2:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 3:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Bmp);
                        break;
                    case 4:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;
                    case 5:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                    case 6:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Tiff);
                        break;
                    case 7:
                        canvas1.PaintImage.Save(saveFileDialog1.FileName, System.Drawing.Imaging.ImageFormat.Png);
                        break;
                }
                menuItem20.Enabled = true;
                menuItem21.Enabled = true;
                
                canvas1.ds.saved = true;
                sd.saveclick = true;
                this.currentfilepath = saveFileDialog1.FileName;
            }
            else
                this.Text = Path.GetFileName(saveFileDialog1.FileName) + " - Paint";
        }

        private void menuItem11_Click(object sender, EventArgs e)
        {
        }
        #endregion
        private void menuItem12_Click(object sender, EventArgs e)
        {
        }

        private void menuItem13_Click(object sender, EventArgs e)
        {
        }

        private void menuItem14_Click(object sender, EventArgs e)
        {
        }

        private void menuItem15_Click(object sender, EventArgs e)
        {
        }

        private void menuItem16_Click(object sender, EventArgs e)
        {
        }

        private void menuItem17_Click(object sender, EventArgs e)
        {
        }
             
        // Send
        private void menuItem18_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("mailto:vantruong1088@gmail.com?subject=Hello!");
        }

        private void menuItem19_Click(object sender, EventArgs e)
        {
        }
        
        private void menuItem23_Click(object sender, EventArgs e)
        {
        }

        private void menuItem24_Click(object sender, EventArgs e)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void menuItem25_Click(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// Giai thich ham
        /// </summary>
        /// <param name="sender">giai thich sender</param>
        /// <param name="e">giai thich e</param>
        private void menuItem26_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        #endregion

        #region help

        [DllImport("kernel32.dll", EntryPoint = "GetWindowsDirectory")]
        public static extern int GetWindowsDirectory(StringBuilder lpBuffer, int nSize);

        private void menuItem65_Click(object sender, EventArgs e)
        {
            StringBuilder lpBuffer = new StringBuilder(512);
            GetWindowsDirectory(lpBuffer, lpBuffer.Capacity);
            string path = lpBuffer.ToString() + "\\Help\\mspaint.chm";
            if (File.Exists(path))
                Help.ShowHelp(this, path);
            else
                MessageBox.Show("Help file not found!", "Drawing board", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
        }

        [DllImport("shell32.dll")]
        public extern static void ShellAbout(IntPtr hand, string caption, string text, int icon);

        private void menuItem67_Click(object sender, EventArgs e)
        {
            ShellAbout(this.Handle, "Paint", "NguyenVanTruong-ToanTin1_K51 - DHBKHN\nMicrosoft Visual C# 2008 Development", this.Icon.Handle.ToInt32());
        }

        #endregion

        #endregion

        private void colors1_ColorChange(object sender, EventArgs e)
        {
            canvas1.PenColor = colors1.PenColor;
            canvas1.BrushColor = colors1.BrushColor;
        }

        private void toolPanel1_ToolChanged(object sender, ToolPanel.ToolChange e)
        {
            canvas1.ShapeType = e.ShapeType;
            canvas1.GetCursorStyle(e.ShapeType);  //Shown for the mouse on the canvas style
           
        }

        private void PaintForm_KeyDown(object sender, KeyEventArgs e)
        {
            canvas1.ds.Shift = e.Shift;  //Hold down the Shift key to determine whether, if the hold, then draw the polygon graphics are positive
        }

        private void PaintForm_KeyUp(object sender, KeyEventArgs e)
        {
            canvas1.ds.Shift = false;  //Release Shift key
        }

        

        private void PaintForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!canvas1.ds.saved)
            {
                if (sd.IsDisposed)
                    sd = new SaveDialog();
                if (sd.ShowDialog() == DialogResult.Cancel)
                    e.Cancel = true;
            }
        }

        private void PaintForm_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.All;
            }
        }

        private void PaintForm_DragDrop(object sender, DragEventArgs e)
        {
            string text = this.Text;
            DialogResult dr = DialogResult.Cancel;
            if (!canvas1.ds.saved)
            {
                if (sd.IsDisposed)
                    sd = new SaveDialog();
                dr = sd.ShowDialog();
            }
            if (dr == DialogResult.OK)
            {
                string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
                canvas1.ChangeBackgroundImage(path);
            }
            else
                this.Text = text;
        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
    }
}