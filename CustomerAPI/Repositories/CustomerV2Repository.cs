using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Model;
using CustomerAPI.Interface;

namespace CustomerAPI.Repositories
{
    public class CustomerV2Repository : ICustomerV2Repository
    {
        public static List<CustomerV2> customerListV2 = new List<CustomerV2>{
            new CustomerV2{Id=101,Name="Johnny",Address="New Jersy",Email="ABC@gmail.com",Phone="9283485762"}
        };
        public List<CustomerV2> GetAll()
        {
            return customerListV2;
        }
        public bool AddCustomerV2(CustomerV2 c)
        {
            if (c == null) return false;
            customerListV2.Add(c);
            return true;
        }

        public CustomerV2 UpdateCustomerV2(int id, CustomerV2 c)
        {
            var existing = customerListV2.FirstOrDefault(x => x.Id == id);
            if (existing != null)
            {
                existing.Name = c.Name;
                existing.Address = c.Address;
                existing.Email = c.Email;
                existing.Phone = c.Phone;
                return existing;
            }
            return null;
        }

        public bool DeleteCustomerV2(int id)
        {
            var exist = customerListV2.FirstOrDefault(x => x.Id == id);
            if (exist != null)
            {
                customerListV2.Remove(exist);
                return true;
            }
            return false;
        }
    }
}