using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenTienDung_BTT6
{
    internal abstract class Hinh2D
    {
        public string Ten;
        public string MoTa;
        public Color Mau;
        public Hinh2D() { }
        public abstract double TinhChuVi();
        public abstract double TinhDienTich();
    }
}
