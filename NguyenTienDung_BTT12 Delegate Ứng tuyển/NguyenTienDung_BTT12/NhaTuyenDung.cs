using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenTienDung_BTT12
{
    internal class NhaTuyenDung
    {
        private string _TenNhaTuyenDung;
        private int _SoNamKinhNghiemYeuCau;
        private double _CPA_YeuCau;

        public NhaTuyenDung(string i_TenNhaTuyenDung, int i_SoNamKinhNghiemYeuCau, double i_CPA_YeuCau)
        {
            TenNhaTuyenDung = i_TenNhaTuyenDung;
            SoNamKinhNghiemYeuCau = i_SoNamKinhNghiemYeuCau;
            CPA_YeuCau = i_CPA_YeuCau;
        }
        public string TenNhaTuyenDung
        {
            set
            {
                _TenNhaTuyenDung = value;
            }
            get
            {
                return _TenNhaTuyenDung;
            }
        }
        public int SoNamKinhNghiemYeuCau
        {
            set
            {
                _SoNamKinhNghiemYeuCau = value;
            }
            get
            {
                return _SoNamKinhNghiemYeuCau;
            }
        }
        public double CPA_YeuCau
        {
            set
            {
                _CPA_YeuCau = value;
            }
            get
            {
                return _CPA_YeuCau;
            }
        }

        public NhaTuyenDung() { }

        public void DangKyUngTuyen(UngVien _UngVien)
        {
            _UngVien.SuKienUngTuyen += new UngVien.XulyGuiCV(_UngVien_SuKienUngTuyen);
        }

        void _UngVien_SuKienUngTuyen(UngVien _UngVien, double a, int b)
        {
            if (a >= CPA_YeuCau && b >= SoNamKinhNghiemYeuCau)
            {
                Console.WriteLine($"Chuc mung {_UngVien.Ten} da trung tuyen vao {TenNhaTuyenDung}");
            }
            else
            {
                Console.WriteLine($"Rat tiec! {_UngVien.Ten} khong du dieu kien trung tuyen vao {TenNhaTuyenDung}");
            }
        }
    }
}
