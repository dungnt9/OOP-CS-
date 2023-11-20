using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class PhanSo
{
    private int ts;
    private int ms;

    // Phương thức khởi tạo mặc định, tử số = 0, mẫu số = 1
    public PhanSo()
    {
        ts = 0;
        ms = 1;
    }

    // Phương thức khởi tạo với tử số và mẫu số cho trước
    public PhanSo(int ts, int ms)
    {
        this.ts = ts;
        this.ms = ms;
        this.RutGon();
    }


    public PhanSo(string s)
    {
        string[] xau = s.Split('/');
        ts = int.Parse(xau[0]);
        ms= int.Parse(xau[1]);
    }

    public void Nhap(int ts, int ms)
    {
        this.ts = ts;
        this.ms = ms;
        this.RutGon();
    }


    public static PhanSo Cong(PhanSo ps1, PhanSo ps2)
    {
        PhanSo _kq=new PhanSo();
        _kq.ts = ps1.ts * ps2.ms + ps2.ts * ps1.ms;
        _kq.ms = ps1.ms * ps2.ms;
        _kq.RutGon();
        return _kq;
    }

    public static PhanSo Tru(PhanSo ps1, PhanSo ps2)
    {
        PhanSo kq = new PhanSo();
        kq.ts = ps1.ts * ps2.ms - ps2.ts * ps1.ms;
        kq.ms = ps1.ms * ps2.ms;
        kq.RutGon();
        return kq;
    }

    public static PhanSo Cong(PhanSo ps, int number)
    {
        PhanSo kq = new PhanSo();
        kq.ts = ps.ts + (number * ps.ms);
        kq.ms = ps.ms;
        kq.RutGon();
        return kq;
    }

    public static PhanSo Tru(int number, PhanSo ps)
    {
        PhanSo kq = new PhanSo();
        kq.ts = -ps.ts + (number * ps.ms);
        kq.ms = ps.ms;
        kq.RutGon();
        return kq;
    }

    public static PhanSo Nhan(PhanSo ps1, PhanSo ps2)
    {
        PhanSo kq = new PhanSo();
        kq.ts = ps1.ts * ps2.ts;
        kq.ms = ps1.ms * ps2.ms;
        kq.RutGon();
        return kq;
    }

    public static PhanSo operator+ (PhanSo ps1, PhanSo ps2)
    {
        return Cong(ps1, ps2);
    }

    public static PhanSo operator+(PhanSo ps1, int number)
    {
        return Cong(ps1, number);
    }

    public static PhanSo operator- (PhanSo ps1, PhanSo ps2)
    {
        return Tru(ps1, ps2);
    }

    public static PhanSo operator- ( int number, PhanSo ps1)
    {
        return Tru(number, ps1);
    }

    public static PhanSo operator *(PhanSo ps1, PhanSo ps2)
    {
        return Nhan(ps1, ps2);
    }

    public PhanSo Power(int n)
    {
        PhanSo ps1= new PhanSo();
        ps1.ts = (int)( Math.Pow(ts, n));
        ps1.ms = (int)( Math.Pow(ms, n));
        return ps1;
    }


    public void RutGon()
    {
        int gcd = TimUCLN(ts, ms);
        ts /= gcd;
        ms /= gcd;
    }

    // Phương thức tìm ước chung lớn nhất (GCD)
    private int TimUCLN(int a, int b)
    {
        while (b != 0)
        {
            int temp = b;
            b = a % b;
            a = temp;
        }
        return a;
    }

    // Phương thức in kết quả ra màn hình
    public void InPhanSo()
    {
        Console.WriteLine($"{ts}/{ms}");
    }
}
