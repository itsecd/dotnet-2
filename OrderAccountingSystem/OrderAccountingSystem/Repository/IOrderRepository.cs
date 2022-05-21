using OrderAccountingSystem.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAccountingSystem.Repository
{
    public interface IOrderRepository
    {
        Task<List<Order>> GetAllOrdersAsync();
        Task<Guid> AddOrderAsync(Order order);
        Task<Order> GetOrderAsync(Guid id);
        Task<double> GetMonthlySales();
        Task<Guid> DeleteOrderAsync(Guid id);
        Task<Guid> ChangeOrderAsync(Guid id, Order newOrder);
        Task<Guid> ChangeOrderStatusAsync(Guid id, Order newOrder);
        Task<bool> CheckOrderAsync(Guid id);
    }
}
