using Lab2.Model;
using System.Collections.Generic;

namespace Lab2.Repositories
{
    public interface IOrderRepository
    {
        void ReadFromFile();
        void WriteToFile();
        int Add(Order order);
        int Change(int id, Order newOrder);
        List<Order> ListOrders();
        Order GetOrder(int id);
        int Remove(int id);
        List<Product> ListProducts(int id);
        int AddProduct(int id, Product product);
        Product GetProduct(int orderId, int productId);
        int ChangeProduct(int orderId, int productId, Product newProduct);
        int RemoveProduct(int orderId, int productId);
        KeyValuePair<Product, float> BestProductOfMonth();
        float GetSumMonth();
    }
}