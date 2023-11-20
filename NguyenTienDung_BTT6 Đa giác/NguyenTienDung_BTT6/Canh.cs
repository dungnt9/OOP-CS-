using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenTienDung_BTT6
{
    internal class Canh
    {
        public Diem DiemDau;
        public Diem DiemCuoi;
        public Canh(Diem diemDau, Diem diemCuoi)
        {
            DiemDau = diemDau;
            DiemCuoi = diemCuoi;
        }
        public double DoDai
        {
            get { return (DiemDau - DiemCuoi).Length; }
        }
    }
}
