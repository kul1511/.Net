using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Model;
using CustomerAPI.Interface;


namespace CustomerAPI.Repositories
{
    public class CustomerV1Repository : ICustomerV1Repository
    {
        public static List<CustomerV1> customerListV1 = new List<CustomerV1>{
            new CustomerV1{Id=1,Name="John",Address="New York"}
        };

        public List<CustomerV1> GetAll()
        {
            return customerListV1;
        }

        public bool AddCustomerV1(CustomerV1 c)
        {
            foreach (var v in customerListV1)
            {
                if (v.Id == c.Id) return false;
            }
            if (c == null)
            {
                return false;
            }
            customerListV1.Add(c);
            return true;
        }
        public CustomerV1 UpdateCustomerV1(int id, CustomerV1 c)
        {
            var existing = customerListV1.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.Name = c.Name;
                existing.Address = c.Address;
                return existing;
            }
            return null;
        }

        public bool DeleteCustomerV1(int id)
        {
            var exist = customerListV1.FirstOrDefault(x => x.Id == id);
            if (exist != null)
            {
                customerListV1.Remove(exist);
                return true;
            }
            return false;
        }
    }
}