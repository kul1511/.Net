using System;
using System.Collections.Generic;

namespace _27thFeb
{
    public class Employee
    {
        public int EmployeeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Title { get; set; }
        public DateTime DOB { get; set; }
        public DateTime DOJ { get; set; }
        public string City { get; set; }
    }
    public class Program
    {
        public static List<Employee> empList = new List<Employee>();
        public static void Main(string[] args)
        {

            empList.Add(new Employee { EmployeeID = 1001, FirstName = "Malcolm", LastName = "Daruwalla", Title = "Manager", DOB = DateTime.Parse("16/11/1984"), DOJ = DateTime.Parse("8/6/2011"), City = "Mumbai" });
            empList.Add(new Employee { EmployeeID = 1002, FirstName = "Asdin", LastName = "Dhalla", Title = "AsstManager", DOB = DateTime.Parse("20/08/1984"), DOJ = DateTime.Parse("7/7/2012"), City = "Mumbai" });
            empList.Add(new Employee { EmployeeID = 1003, FirstName = "Madhavi", LastName = "Oza", Title = "Consultant", DOB = DateTime.Parse("14/11/1987"), DOJ = DateTime.Parse("12/4/2015"), City = "Pune" });
            empList.Add(new Employee { EmployeeID = 1004, FirstName = "Saba", LastName = "Shaikh", Title = "SE", DOB = DateTime.Parse("3/6/1990"), DOJ = DateTime.Parse("2/2/2016"), City = "Pune" });
            empList.Add(new Employee { EmployeeID = 1005, FirstName = "Nazia", LastName = "Shaikh", Title = "SE", DOB = DateTime.Parse("8/3/1991"), DOJ = DateTime.Parse("2/2/2016"), City = "Mumbai" });
            empList.Add(new Employee { EmployeeID = 1006, FirstName = "Amit", LastName = "Pathak", Title = "Consultant", DOB = DateTime.Parse("7/11/1989"), DOJ = DateTime.Parse("8/8/2014"), City = "Chennai" });
            empList.Add(new Employee { EmployeeID = 1007, FirstName = "Vijay", LastName = "Natrajan", Title = "Consultant", DOB = DateTime.Parse("2/12/1989"), DOJ = DateTime.Parse("1/6/2015"), City = "Mumbai" });
            empList.Add(new Employee { EmployeeID = 1008, FirstName = "Rahul", LastName = "Dubey", Title = "Associate", DOB = DateTime.Parse("11/11/1993"), DOJ = DateTime.Parse("6/11/2014"), City = "Chennai" });
            empList.Add(new Employee { EmployeeID = 1009, FirstName = "Suresh", LastName = "Mistry", Title = "Associate", DOB = DateTime.Parse("12/8/1992"), DOJ = DateTime.Parse("3/2/2014"), City = "Chennai" });
            empList.Add(new Employee { EmployeeID = 1010, FirstName = "Sumit", LastName = "Shah", Title = "Manager", DOB = DateTime.Parse("12/4/1991"), DOJ = DateTime.Parse("2/1/2016"), City = "Pune" });

            // Employee emp = new Employee();

            System.Console.WriteLine("a. Details of All the Employees:");
            foreach (var e in empList)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("b. Employees whose location is not Mumbai");
            var notMumbai = empList.Where(x => x.City != "Mumbai");
            foreach (var e in notMumbai)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("c. Title is Assistant Manager");
            var asstManager = empList.Where(e => e.Title == "AsstManager");
            foreach (var e in asstManager)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("d. LastName starts with S");
            var lastnameStatsWithS = empList.Where(e => e.LastName.StartsWith("S"));
            foreach (var e in lastnameStatsWithS)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("e. Empoyees joined before 1/1/2015");
            DateTime join = new DateTime(2015, 1, 1);
            var joinedBeforeDate = empList.Where(e => e.DOJ < join);
            foreach (var e in joinedBeforeDate)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("f. Date of Birth is After 1/1/1990");
            DateTime afterDOB = new DateTime(1990, 1, 1);
            var dobAfter = empList.Where(e => e.DOB > afterDOB);
            foreach (var e in dobAfter)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("g. Designation is only Consultant and Associate");
            // DateTime afterDOB = new DateTime(1990, 1, 1);
            var onlyConsultantandAssociate = empList.Where(e => e.Title == "Consultant" || e.Title == "Associate");
            foreach (var e in onlyConsultantandAssociate)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("h. Total Number of Employees");
            System.Console.WriteLine(empList.Count);
            System.Console.WriteLine();

            System.Console.WriteLine("i. Employees belonging to Chennai");
            var cityIsChennai = empList.Where(x => x.City == "Chennai");
            foreach (var e in cityIsChennai)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("j. Highest Employee Id");
            var highesEmployee = empList.Max(e => e.EmployeeID);
            var res = empList.Where(e => e.EmployeeID == highesEmployee);
            foreach (var e in res)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("k. Count of Employees joined after 1/1/2015");
            DateTime afterdate = new DateTime(2015, 1, 1);
            var joinedAfterDate = empList.Where(e => e.DOB > afterDOB);
            int count = 0;
            foreach (var e in joinedAfterDate)
            {
                count++;
            }
            System.Console.WriteLine(count);
            System.Console.WriteLine();

            System.Console.WriteLine("l. Desingation is not Associate");
            var notAssociate = empList.Where(e => e.Title != "Associate");
            foreach (var e in notAssociate)
            {
                System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("m. Total Number of Employees based on City");
            var totalBYCity = empList.GroupBy(e => e.City).Select(c => new { city = c.Key, count = c.Count() });
            foreach (var e in totalBYCity)
            {
                // System.Console.WriteLine($"ID: {e.EmployeeID}, Name: {e.FirstName} {e.LastName}, Title: {e.Title}, DOB: {e.DOB.ToShortDateString()}, DOJ: {e.DOJ.ToShortDateString()}, City: {e.City}");
                System.Console.WriteLine($"City:{e.city} Count: {e.count}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("n. Total Number of Employees based on City and Title");
            var totalBYCityandTitle = empList.GroupBy(e => new { e.City, e.Title }).Select(g => new { city = g.Key.City, title = g.Key.Title, count = g.Count() });
            foreach (var v in totalBYCityandTitle)
            {
                System.Console.WriteLine($"City: {v.city}   Title: {v.title}    Count:{v.count}");
            }
            System.Console.WriteLine();

            System.Console.WriteLine("o. Employee who is youngest");
            var youngest = empList.OrderByDescending(e => e.DOB).First();
            System.Console.WriteLine($"ID: {youngest.EmployeeID}, Name: {youngest.FirstName} {youngest.LastName}, Title: {youngest.Title}, DOB: {youngest.DOB.ToShortDateString()}, DOJ: {youngest.DOJ.ToShortDateString()}, City: {youngest.City}");
        }
    }
}