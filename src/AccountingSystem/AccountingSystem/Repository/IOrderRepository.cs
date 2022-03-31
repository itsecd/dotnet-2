using AccountingSystem.Model;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public interface IOrderRepository
    {
        IList<Order> GetOrders();
        Order GetOrder(int id);
        double GetAllPrice();
        int ChangeOrder(int id, Order customer);
        int ChangeOrderStatus(int id, int newStatus);
        int AddOrder(Order customer);
        int RemoveOrder(int id);
    }
}
