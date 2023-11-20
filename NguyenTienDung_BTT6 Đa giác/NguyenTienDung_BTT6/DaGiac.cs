using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenTienDung_BTT6
{
    internal class DaGiac:Hinh2D
    {
        public List<Diem> TapDinh = new List<Diem>();
        public int SoDinh
        {
            get { return TapDinh.Count; }
        }
        
        public DaGiac() { }
        public override double TinhChuVi()
        {
            return 0;
        }
        public override double TinhDienTich()
        {
            return 0;
        }
    }
}
