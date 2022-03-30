using AccountingSystem.Model;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public interface IOrderRepository
    {
        IList<Order> GetOrders();
        Order GetOrder(int id);
        void ChangeOrder(int id, Order customer);
        void AddOrder(Order customer);
        void RemoveOrder(int id);
    }
}
