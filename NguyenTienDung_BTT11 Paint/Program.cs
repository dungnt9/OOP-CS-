using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace MSPaint
{
    static class Program
    {
        /// <summary>
        /// Application's main entry point.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PaintForm());
        }
    }
}
