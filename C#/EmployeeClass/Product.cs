using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EmployeeClass
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Type { get; set; }


        public Product(int Id, string Name, string Type)
        {
            this.Id = Id;
            this.Name = Name;
            this.Type = Type;
        }


        public void DisplayDetails()
        {
            Console.WriteLine($"{this.Id} : {this.Name} : {this.Type}");
        }
    }
}