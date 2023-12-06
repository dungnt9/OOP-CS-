using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenTienDung_BTT12
{
    internal class UngVien
    {
        public UngVien() {}

        private string _Ten;
        public int _SoNamKinhNghiem;
        public double _CPA;

        public UngVien(string i_Ten)
        {
            Ten = i_Ten;
        }
        public string Ten
        {
            set
            {
                _Ten = value;
            }
            get
            {
                return _Ten;
            }
        }

        public delegate void XulyGuiCV(UngVien _UngVien, double a, int b);
        
        public event XulyGuiCV SuKienUngTuyen;

        public void GuiCV(double _CPA, int _SoNamKinhNghiem)
        {
            if (SuKienUngTuyen != null)
            {
                SuKienUngTuyen(this, _CPA, _SoNamKinhNghiem);
            }

        }
    }
}
