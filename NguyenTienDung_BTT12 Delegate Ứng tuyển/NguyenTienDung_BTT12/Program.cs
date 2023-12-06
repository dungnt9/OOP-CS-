using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NguyenTienDung_BTT12
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Thong bao Ket qua Ung tuyen\n");

            NhaTuyenDung _NhaTuyenDung1 = new NhaTuyenDung("Cong ty TNHH Food", 5, 3.12);
            NhaTuyenDung _NhaTuyenDung2 = new NhaTuyenDung("Cong ty CP Book", 0, 2.5);
            NhaTuyenDung _NhaTuyenDung3 = new NhaTuyenDung("Tap doan Dien tu", 5, 3.12);

            UngVien _UngVien1 = new UngVien("Nguyen Van A");
            UngVien _UngVien2 = new UngVien("Nguyen Tien Dung");
            UngVien _UngVien3 = new UngVien("Nguyen Thi B");

            _NhaTuyenDung1.DangKyUngTuyen(_UngVien1);

            _NhaTuyenDung2.DangKyUngTuyen(_UngVien2);

            _NhaTuyenDung3.DangKyUngTuyen(_UngVien1);
            _NhaTuyenDung3.DangKyUngTuyen(_UngVien2);
            _NhaTuyenDung3.DangKyUngTuyen(_UngVien3);

            _UngVien1.GuiCV(3.4, 6);
            _UngVien2.GuiCV(3.5, 5);
            _UngVien3.GuiCV(3.6, 4);
            
            Console.ReadLine();
        }
    }
}
