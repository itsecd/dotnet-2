using OrderAccountingSystem.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAccountingSystem.Repositories
{
    public interface ICustomerRepository
    {
        Task<List<Customer>> GetAllCustomersAsync();
        Task<Guid> AddCustomerAsync(Customer product);
        Task<Customer> GetCustomerAsync(Guid id);
        Task<Guid> DeleteCustomerAsync(Guid id);
        Task<Guid> ChangeCustomerAsync(Guid id, Customer newCustomer);
        Task<bool> CheckCustomerAsync(Guid id);
        Task ReadCustomersFileAsync();
        Task WriteCustomersFileAsync();
    }
}
