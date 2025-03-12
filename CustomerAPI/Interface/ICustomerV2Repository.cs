using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Model;

namespace CustomerAPI.Interface
{
    public interface ICustomerV2Repository
    {
        List<CustomerV2> GetAll();
        bool AddCustomerV2(CustomerV2 customer);
        CustomerV2 UpdateCustomerV2(int id, CustomerV2 c);
        bool DeleteCustomerV2(int id);
    }
}