using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AForge.Math;
using NguyenTienDung_BTT6;
class NguyenTienDung20216805
{
    static void Main(string[] args)
    {
        VD4();
        VD1();
        VD2();
        VD2b();
    }
    
    public static void VD4()
    {
        Console.WriteLine("Vi du 4:");
        Vector3D point_A = new Vector3D(1,2,1);
        Vector3D point_B = new Vector3D(2,-1,3);
        Vector3D point_C = new Vector3D(5,2,-3);
        Vector3D point_D = new Vector3D(4,5,-6);
        Vector3D AB = Vector3D.Tru(point_A, point_B);
        Vector3D AC = Vector3D.Tru(point_A, point_C);
        Vector3D AD = Vector3D.Tru(point_A, point_D);
        float theTich1 = Math.Abs(Vector3D.Dot(Vector3D.Cross(AB, AC), AD));
        Console.WriteLine("a. The tich cua hinh hop dung tren cac canh AB, AC, AD = {0}", theTich1);
        float thetich2 = Math.Abs(Vector3D.Dot(Vector3D.Cross(AB, AC), AD)) / 6;
        Console.WriteLine("b. The tich cua tu dien ABCD = {0}", thetich2);
        float dienTich = (Vector3D.Cross(AB, AC)).Norm / 2;
        Console.WriteLine("c. Dien tich tam giac ABC = {0}", dienTich);
        if (Vector3D.Dot(Vector3D.Cross(AB, AC), AD) != 0)
        {
            Console.WriteLine("d. AB,AC,AD khong dong phang => A,B,C,D la 4 dinh cua 1 tu dien\n");
        }
        else Console.WriteLine("d. AB,AC,AD dong phang\n");
    }
    public static void VD1()
    {
        Console.WriteLine("Vi du 1:");
        Vector3D point_A = new Vector3D(1, 3, 1);
        Vector3D point_B = new Vector3D(0, 1, 2);
        Vector3D point_C = new Vector3D(3, 7, -1);
        Vector3D AB = Vector3D.Tru(point_A, point_B);
        Vector3D AC = Vector3D.Tru(point_A, point_C);
        Vector3D BC = Vector3D.Tru(point_B, point_C);
        if ((Vector3D.Divide(AB, AC).GetType() == typeof(int)) ||
            (Vector3D.Divide(AC, AB).GetType() == typeof(int)) ||
            (Vector3D.Divide(AB, BC).GetType() == typeof(int)) ||
            (Vector3D.Divide(BC, AB).GetType() == typeof(int)) ||
            (Vector3D.Divide(BC, AC).GetType() == typeof(int)) ||
            (Vector3D.Divide(AC, BC).GetType() == typeof(int))
            )
        {
            Console.WriteLine("A, B, C thang hang\n");
        }
        else Console.WriteLine("A, B, C khong thang hang\n");
    }
    public static void VD2()
    {
        Console.WriteLine("Vi du 2:");
        Vector3D point_A = new Vector3D(1, -1, 1);
        Vector3D point_B = new Vector3D(1, 3, 1);
        Vector3D point_C = new Vector3D(4, 3, 2);
        Vector3D point_D = new Vector3D(4, 1, 3);
        Vector3D AB = Vector3D.Tru(point_A, point_B);
        Vector3D AC = Vector3D.Tru(point_A, point_C);
        Vector3D AD = Vector3D.Tru(point_A, point_D);
        if (Vector3D.Dot(Vector3D.Cross(AB, AC), AD) != 0)
            Console.WriteLine("A, B, C, D khong dong phang\n");
        else Console.WriteLine("A, B, C, D dong phang\n");
    }
    public static void VD2b()
    {
        Console.WriteLine("Vi du 2b:");
        Vector3D point_A = new Vector3D(1, -1, 2);
        Vector3D point_B = new Vector3D(5, -6, 2);
        Vector3D point_C = new Vector3D(1, 3, -1);
        Vector3D AB = Vector3D.Tru(point_A, point_B); 
        Vector3D AC = Vector3D.Tru(point_A, point_C); 
        Vector3D BC = Vector3D.Tru(point_B, point_C);
        if (Vector3D.Divide(AC, AB).X == Vector3D.Divide(AC, AB).Y && Vector3D.Divide(AC, AB).Y == Vector3D.Divide(AC, AB).Z)
        {
            Console.WriteLine("(5) A, B, C thang hang");
        }
        else Console.WriteLine("(5) A, B, C khong thang hang");
        if (Vector3D.Dot(AB,AC)==0 || Vector3D.Dot(AB, BC) == 0 || Vector3D.Dot(BC, AC) == 0)
        {
            Console.WriteLine("(6) ABC la tam giac vuong");
        }
        else Console.WriteLine("(6) ABC khong la tam giac vuong");
        float dienTich = (Vector3D.Cross(AB, AC)).Norm / 2;
        Console.WriteLine("(7) Dien tich tam giac ABC = {0}", dienTich);
    }
}
