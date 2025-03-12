using System;

namespace SalaryCalculation
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Enter Employee No: ");
            int EmpNo = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Employee Name: ");
            string EmpName = Console.ReadLine();
            Console.WriteLine("Enter Employee Salary");
            double Salary = double.Parse(Console.ReadLine());

            Employee emp = new Employee(EmpNo, EmpName, Salary);

            double GrossSalary = emp.CalculateSalary();
            Console.WriteLine("Gross Salary : " + GrossSalary);
        }
    }
}