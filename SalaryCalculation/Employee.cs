using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SalaryCalculation
{
    public class Employee
    {
        int EmpNo { get; set; }
        string EmpName { get; set; }
        double Salary { get; set; }

        public Employee(int No, string name, double salary)
        {
            this.EmpNo = No;
            this.EmpName = name;
            this.Salary = salary;
        }
        double HRA;
        double TA;
        double DA;
        double PF;
        double TDS;
        double NetSalary;
        double GrossSalary;

        public double CalculateSalary()
        {
            if (Salary < 5000)
            {
                HRA = 0.10 * Salary;
                TA = 0.05 * Salary;
                DA = 0.15 * Salary;
            }
            else if (Salary < 10000)
            {
                HRA = 0.15 * Salary;
                TA = 0.10 * Salary;
                DA = 0.20 * Salary;
            }
            else if (Salary < 15000)
            {
                HRA = 0.20 * Salary;
                TA = 0.15 * Salary;
                DA = 0.25 * Salary;
            }
            else if (Salary < 20000)
            {
                HRA = 0.25 * Salary;
                TA = 0.20 * Salary;
                DA = 0.30 * Salary;
            }
            else if (Salary >= 20000)
            {
                HRA = 0.30 * Salary;
                TA = 0.25 * Salary;
                DA = 0.35 * Salary;
            }

            GrossSalary = Salary + HRA + TA + DA;
            PF = 0.10 * GrossSalary;
            TDS = 0.18 * GrossSalary;
            NetSalary = GrossSalary - (PF + TDS);

            Console.WriteLine("PF: " + PF);
            Console.WriteLine("NetSalary: " + NetSalary);

            return GrossSalary;
        }
    }
}