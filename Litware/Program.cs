using System;
namespace Litware
{
    public class Program
    {
        public static void Main(string[] args)
        {

            System.Console.WriteLine("Enter Employee No:");
            int empno = int.Parse(Console.ReadLine());
            System.Console.WriteLine("Enter Employee Name:");
            string empName = Console.ReadLine();
            System.Console.WriteLine("Enter Salary:");
            double Salary = double.Parse(Console.ReadLine());
            Employee e = new Employee(empno, empName, Salary);

            e.CalaculateGross();
            e.CalculateSalary();
        }
    }
}