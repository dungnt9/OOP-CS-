namespace MSPaint
{
    partial class PaintForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up all the resources being used.
        /// </summary>
        /// <param name="disposing">If managed resources should be released, as true; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Designer support of the approach - do not
        /// Using the code editor to modify the contents of this method.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PaintForm));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolPanel1 = new MSPaint.ToolPanel();
            this.canvas1 = new MSPaint.Canvas();
            this.colors1 = new MSPaint.Colors();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = resources.GetString("openFileDialog1.Filter");
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.Filter = resources.GetString("saveFileDialog1.Filter");
            this.saveFileDialog1.Title = "Save";
            this.saveFileDialog1.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialog1_FileOk);
            // 
            // toolPanel1
            // 
            this.toolPanel1.Location = new System.Drawing.Point(13, 16);
            this.toolPanel1.Margin = new System.Windows.Forms.Padding(5);
            this.toolPanel1.Name = "toolPanel1";
            this.toolPanel1.Size = new System.Drawing.Size(75, 460);
            this.toolPanel1.TabIndex = 13;
            this.toolPanel1.ToolChanged += new MSPaint.ToolPanel.ToolChangeEventHandler(this.toolPanel1_ToolChanged);
            // 
            // canvas1
            // 
            this.canvas1.AllowDrop = true;
            this.canvas1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.canvas1.AutoScroll = true;
            this.canvas1.AutoScrollMargin = new System.Drawing.Size(3, 3);
            this.canvas1.BackColor = System.Drawing.Color.Silver;
            this.canvas1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.canvas1.Location = new System.Drawing.Point(96, 69);
            this.canvas1.Margin = new System.Windows.Forms.Padding(5);
            this.canvas1.Name = "canvas1";
            this.canvas1.PaintImage = null;
            this.canvas1.Size = new System.Drawing.Size(733, 462);
            this.canvas1.TabIndex = 12;
            // 
            // colors1
            // 
            this.colors1.BrushColor = System.Drawing.Color.White;
            this.colors1.Location = new System.Drawing.Point(96, 16);
            this.colors1.Margin = new System.Windows.Forms.Padding(5);
            this.colors1.Name = "colors1";
            this.colors1.PenColor = System.Drawing.Color.Black;
            this.colors1.Size = new System.Drawing.Size(349, 52);
            this.colors1.TabIndex = 11;
            this.colors1.ColorChange += new MSPaint.Colors.ColorChangeEventHandler(this.colors1_ColorChange);
            // 
            // PaintForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(832, 562);
            this.Controls.Add(this.toolPanel1);
            this.Controls.Add(this.canvas1);
            this.Controls.Add(this.colors1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PaintForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "untitled - Paint";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PaintForm_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.PaintForm_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.PaintForm_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.PaintForm_KeyDown);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PaintForm_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        private Colors colors1;
        private ToolPanel toolPanel1;
        public System.Windows.Forms.SaveFileDialog saveFileDialog1;
        public System.Windows.Forms.OpenFileDialog openFileDialog1;
        public Canvas canvas1;
    }
}

