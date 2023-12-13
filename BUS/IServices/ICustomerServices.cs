using DTO.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.IServices
{
    public interface ICustomerServices
    {
        Task<List<CustomerResponse>> GetAllCustomer(CustomerFilterDto blob);
        Task<CustomerDetail> GetCustomerDetails(int id);

    }
}
