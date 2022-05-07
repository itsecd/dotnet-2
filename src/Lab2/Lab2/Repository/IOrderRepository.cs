using System.Collections.Generic;
using Lab2.Model;

namespace Lab2.Repository
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        int ReplaceOrder(int id, Order order);
        void AddOrder(Order order);
        Order GetOrder(int id);
        int DeleteOrder(int id);
        void DeleteAllOrders();
        void WriteToFileOrders();
        void ReadFromFileOrders();
    }
}