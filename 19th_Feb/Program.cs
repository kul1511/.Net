using System;
using System.Collections.Generic;
using System.Linq;

namespace _19th_Feb
{
    public class Program
    {
        public static List<Customer> customerList = new List<Customer>();
        public static void Main(string[] args)
        {
            Customer cust = new Customer();
            while (true)
            {

                System.Console.WriteLine("Enter Customer Id");
                string id = Console.ReadLine();

                System.Console.WriteLine("Enter Customer Name");
                string name = Console.ReadLine();

                System.Console.WriteLine("Enter Customer City");
                string City = Console.ReadLine();

                System.Console.WriteLine("Enter Age");
                int age = int.Parse(Console.ReadLine());

                System.Console.WriteLine("Enter Phone Number");
                double phone = double.Parse(Console.ReadLine());

                System.Console.WriteLine("Enter PinCode");
                int pincode = int.Parse(Console.ReadLine());

                customerList.Add(new Customer { CustomerId = id, CustomerName = name, City = City, Age = age, Phone = phone, Pincode = pincode });
                System.Console.WriteLine("Customer Details Added Successfully\n");

                System.Console.WriteLine("Do you want to add another Customer? (Y/N)");
                char flag = Console.ReadLine()[0];
                if (flag == 'N') break;
            }
            while (true)
            {
                System.Console.WriteLine("1. Search\n2. Modify\n3. Remove\n4. Display List\n5. Exit");
                int choice = int.Parse(Console.ReadLine());
                if (choice == 5) break;
                switch (choice)
                {
                    case 1:
                        // System.Console.WriteLine();
                        System.Console.WriteLine("\nEnter an id to search for a Customer: ");
                        string searchId = Console.ReadLine();
                        System.Console.WriteLine("\nEnter Customer Name to search: ");
                        string searchName = Console.ReadLine();
                        Customer c = cust.SearchCustomer(searchId, searchName);
                        System.Console.WriteLine($"Searched Customer\nId: {c.CustomerId} Name: {c.CustomerName}\n");
                        break;
                    case 2:
                        System.Console.WriteLine("\nEnter a Customer Id to Modify");
                        string modifyId = Console.ReadLine();
                        cust.ModifyCustomer(modifyId);
                        // System.Console.WriteLine("Customer List After Modifying: ");
                        break;
                    case 3:
                        System.Console.WriteLine("\nEnter a Customer Id to Remove");
                        string removeId = Console.ReadLine();
                        cust.RemoveCustomer(removeId);
                        // System.Console.WriteLine($"Customer List After Removing Customer with Id- {removeId}: ");
                        break;
                    case 4:
                        cust.CustomerInfo();
                        break;
                    case 5:
                        System.Console.WriteLine("\nThank You");
                        break;
                    default: break;
                }
            }
        }
    }
}