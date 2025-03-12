using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _19th_Feb
{
    public class Customer
    {
        public string CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
        public int Age { get; set; }
        public double Phone { get; set; }
        public int Pincode { get; set; }

        public Customer SearchCustomer(string id, string name)
        {
            return Program.customerList.FirstOrDefault(x => x.CustomerId == id && x.CustomerName == name);

        }

        public void ModifyCustomer(string id)
        {
            // var customer = Program.customerList.FirstOrDefault(x=>x.CustomerId == id);
            foreach (Customer customer in Program.customerList)
            {
                if (customer.CustomerId == id)
                {
                    System.Console.WriteLine("Enter the Details to modify:");
                    System.Console.WriteLine("Name: ");
                    customer.CustomerName = Console.ReadLine();
                    System.Console.WriteLine("City: ");
                    customer.City = Console.ReadLine();
                    System.Console.WriteLine("Age: ");
                    customer.Age = int.Parse(Console.ReadLine());
                }
            }
            // System.Console.WriteLine("Modified Customer: ");
            // CustomerInfo();
        }

        public void RemoveCustomer(string id)
        {
            // foreach (Customer customer in Program.customerList)
            // {
            // if (customer.CustomerId == id)
            // {
            //     Program.customerList.Remove(customer);
            //     System.Console.WriteLine($"Successfully Removed Customer with Id: {id}");
            // }
            // }
            Program.customerList.RemoveAll(x => x.CustomerId == id);
            System.Console.WriteLine($"Successfully Removed Customer with Id: {id}");
            // System.Console.WriteLine($"Failed to Remove Customer with Id: {id}");
        }
        public void CustomerInfo()
        {
            // System.Console.WriteLine("Customer Details: ");
            foreach (var cust in Program.customerList)
            {
                Console.WriteLine("Id: " + cust.CustomerId);
                Console.WriteLine("Name: " + cust.CustomerName);
                System.Console.WriteLine("City: " + cust.City);
                System.Console.WriteLine("Age: " + cust.Age);
                // System.Console.WriteLine("Phone: "+cust.Phone);
                // System.Console.WriteLine("Pincode: "+cust.Pincode);    
                System.Console.WriteLine();
            }
            System.Console.WriteLine();
        }
    }
}