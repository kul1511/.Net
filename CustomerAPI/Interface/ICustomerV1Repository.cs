using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CustomerAPI.Model;

namespace CustomerAPI.Interface
{
    public interface ICustomerV1Repository
    {

        List<CustomerV1> GetAll();
        bool AddCustomerV1(CustomerV1 c);
        CustomerV1 UpdateCustomerV1(int id, CustomerV1 c);
        bool DeleteCustomerV1(int id);
        // bool DeleteCustomerV1(CustomerV1 c);
    }
}