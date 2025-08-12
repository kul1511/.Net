using System;
namespace Litware
{
    class Employee
    {
        private int EmpNo;
        private string EmpName;
        private double Salary;
        private double HRA;
        private double TA;
        private double DA;
        private double PF;
        private double TDS;
        private double NetSalary;
        private double GrossSalary;

        public Employee(int EmpNo, string EmpName, double Salary)
        {
            this.EmpNo = EmpNo;
            this.EmpName = EmpName;
            this.Salary = Salary;
        }
        public void CalaculateGross()
        {
            if (Salary < 5000)
            {
                HRA = 0.10 * Salary;
                TA = 0.05 * Salary;
                DA = 0.15 * Salary;
            }
            else if (Salary > 5000 && Salary < 10000)
            {
                HRA = 0.15 * Salary;
                TA = 0.10 * Salary;
                DA = 0.20 * Salary;
            }
            else if (Salary > 10000 && Salary < 15000)
            {
                HRA = 0.20 * Salary;
                TA = 0.15 * Salary;
                DA = 0.25 * Salary;
            }
            else if (Salary > 15000 && Salary < 20000)
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
        }
        public void CalculateSalary()
        {
            PF = 0.10 * GrossSalary;
            TDS = 0.18 * GrossSalary;
            NetSalary = GrossSalary - (PF + TDS);
            System.Console.WriteLine($"Gross Salary: {GrossSalary}");
            System.Console.WriteLine($"Net Salary: {NetSalary}");
        }
    }
}