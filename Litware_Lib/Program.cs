using System;
namespace Litware
{
    public class Program
    {
        public static void Main(string[] args)
        {

            Employee e = new Employee();
            System.Console.WriteLine("Enter Employee No:");
            int empno = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Enter Employee Name:");
            string empName = Console.ReadLine();
            System.Console.WriteLine("Enter Salary:");
            string empName = double.Parse(Console.ReadLine());

            e.CalaculateGross();
            e.CalculateSalary();
        }
    }
}