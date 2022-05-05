using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Lab2.Model;
using Microsoft.Extensions.Configuration;

namespace Lab2.Repository
{
    public class OrderRepository: IOrderRepository
    {
        private readonly string _storageFileName;
        public OrderRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("OrdersFile");
        }
        public List<Order> _orders { get; set; }
        public void ReadFromFileOrders()
        {
            
            if (!File.Exists(_storageFileName))
            {
                _orders = new List<Order>();
                return;
            }
            using (var fileReader = new StreamReader(_storageFileName))
            {
                string jsonString = fileReader.ReadToEnd();
                _orders = JsonSerializer.Deserialize<List<Order>>(jsonString);
            }
        }
        public void WriteToFileOrders()
        {
            string jsonString = JsonSerializer.Serialize(_orders);
            using (var fileWriter = new StreamWriter(_storageFileName))
            {
                fileWriter.Write(jsonString);
            }
        }
        public void AddOrder(Order order)
        {
            _orders.Add(order);

        }
        public void ReplaceOrder(int id, Order order)
        {
            var orderIndex = _orders.FindIndex(order => order.OrderId == id);
            if (orderIndex > 0)
            {
                _orders[orderIndex] = order;
            }
        }
        public List<Order> GetAllOrders()
        {
            return _orders;
        }
        public Order GetOrder(int id)
        {
            Order order = _orders.Where(order => order.OrderId == id).Single();
            return order;
        }
        public void DeleteOrder(int id)
        {
            _orders.Remove(_orders.Single(order => order.OrderId == id));

        }
        public void DeleteAllOrders()
        {
            _orders.Clear();
        }
    }
}
