using AccountingSystem.Model;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public interface IOrderRepository
    {
        IList<Order> GetOrders();
        Order GetOrder(int id);
        double GetAllPrice();
        int GetCountProductMonthly();
        int ChangeOrder(int id, Order order);
        int PatchStatus(int id, Order order);
        int AddOrder(Order customer);
        int RemoveOrder(int id);
        IList<Product> GetProducts(int id);
        Product GetProduct(int id, int productId);
        int ChangeProduct(int id, Product product, int productId);
        int AddProduct(Product product, int id);
        int RemoveProduct(int id, int productId);
    }
}
