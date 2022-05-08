using System.Collections.Generic;
using System.Linq;
using Lab2.Model;

namespace Lab2.Repository
{
    public interface IOrderRepository
    {
        List<Order> GetAllOrders();
        int ReplaceOrder(int id, Order newOrder);
        void AddOrder(Order order);
        Order GetOrder(int id);
        int DeleteOrder(int id);
        void DeleteAllOrders();
        void WriteToFileOrders();
        void ReadFromFileOrders();
        List<Product> GetProducts(int id);
        int AddProduct(int id, Product product);
        Product GetProduct(int id, int num);
        int RemoveProduct(int id, int num);
        int ReplaceProduct(int id, int num, Product newProduct);
        int RemoveAllProducts(int id);
        float GetAllCostOrder(Order order);
        float GetAmountMonth();
        public Dictionary<string, int> GetProductsMonth();
    }
}