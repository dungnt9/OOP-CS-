using System;
using static PhanSo;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace BTT04
{
    class Program
    {
        static void Main()
        {
            PhanSo ps1 = new PhanSo("1/2");
            PhanSo ps2 = new PhanSo("1/3");
            int number=1;


            PhanSo ketQuaCong = PhanSo.Cong(ps1,ps2);
            Console.Write("Ket qua Cong: ");
            ketQuaCong.InPhanSo();

            PhanSo ketQuaCongSoNguyen = Cong(ps1, number);
            Console.Write("Ket qua cong so nguyen: ");
            ketQuaCongSoNguyen.InPhanSo();

            PhanSo ketQuaTru = Tru(ps1, ps2);
            Console.Write("Ket qua tru: ");
            ketQuaTru.InPhanSo();

            PhanSo ketQuaTruSoNguyen = Tru(number, ps1);
            Console.Write("Ket qua tru so nguyen: ");
            ketQuaTruSoNguyen.InPhanSo();

            PhanSo ketQuaNhan = Nhan(ps1, ps2);
            Console.Write("Ket qua Nhan: ");
            ketQuaNhan.InPhanSo();

            PhanSo ketQuaLuyThua = ps1.Power(3);
            Console.Write("Ket qua luy thua: ");
            ketQuaLuyThua.InPhanSo();

            PhanSo c1 = new PhanSo("1/2");
            PhanSo c2 = new PhanSo("1/2");
            PhanSo c3 = new PhanSo("1/2");
            int c4 = 1;
            PhanSo c5 = new PhanSo("1/5");

            PhanSo kq = new PhanSo();
            kq = c1.Power(2) * (c2 + c3) * (c4 - c5);
            kq.RutGon();
            kq.InPhanSo();



        }
    }

}


    