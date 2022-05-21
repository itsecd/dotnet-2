using OrderAccountingSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAccountingSystem.Repository
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Guid> AddCustomerAsync(Customer product);
        Task<Customer> GetCustomerAsync(Guid id);
        Task<Guid> DeleteCustomerAsync(Guid id);
        Task<Guid> ChangeCustomerAsync(Guid id, Customer newCustomer);
        Task<bool> CheckCustomerAsync(Guid id);
    }
}
