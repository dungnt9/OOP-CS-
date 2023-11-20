//Nguyễn Tiến Dũng 20216805
using System;
using System.Collections.Generic;
class SinhVien
{
    public string HoTen;
    public string MSSV;
    public int DiemGT1;
    public int DiemGT2;
    public int DiemGT3;
    public bool TrangThaiThiLai
    {
        get
        {
            return DiemGT1 < 5 || DiemGT2 < 5 || DiemGT3 < 5;
        }
    }

    public SinhVien() { }

    public SinhVien(string mssv, string hoTen)
    {
        MSSV = mssv;
        HoTen = hoTen;
    }
}

class Program
{
    static void Main(string[] args)
    {
        List<SinhVien> LopHocPhan = new List<SinhVien>
        {
            new SinhVien("2021", "Le Van A") { DiemGT1 = 7, DiemGT2 = 6, DiemGT3 = 8 },
            new SinhVien("2022", "Le Van B") { DiemGT1 = 4, DiemGT2 = 6, DiemGT3 = 7 },
            new SinhVien("2023", "Le Van C") { DiemGT1 = 8, DiemGT2 = 5, DiemGT3 = 9 },
            new SinhVien("2024", "Le Van D") { DiemGT1 = 6, DiemGT2 = 7, DiemGT3 = 3 },
        };

        List<SinhVien> sinhVienThiLai = LopHocPhan.FindAll(sv => sv.TrangThaiThiLai);

        Console.WriteLine("Danh sach sinh viên thi lai:");
        foreach (var sv in sinhVienThiLai)
        {
            Console.WriteLine("MSSV: {0}, Ho ten: {1}", sv.MSSV, sv.HoTen);
        }
        foreach (var sv in LopHocPhan)
        {
            Console.WriteLine("MSSV: {0}, Ho ten: {1}, Trang thai thi lai: {2}", sv.MSSV, sv.HoTen, sv.TrangThaiThiLai);
        }
    }
}
