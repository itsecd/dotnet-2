using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Lab2.Exeptions;
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
        private List<Order> _orders { get; set; }
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
            if (_orders.Count == 0)
            {
                order.OrderId = 1;
            }
            else
            {
                order.OrderId = _orders.Max(od => od.OrderId) + 1;
            }
            _orders.Add(order);
        }
        public int ReplaceOrder(int id, Order order)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            var orderIndex = _orders.FindIndex(order => order.OrderId == id);
            if (orderIndex > 0)
            {
                Order newOrder = _orders[orderIndex];
                newOrder.Products = order.Products;
                newOrder.CustomerId = order.CustomerId;
                newOrder.dt = order.dt;
                newOrder.AmountOrder = order.AmountOrder;
                newOrder.Status = order.Status;
            }
            else
            {
                throw new NotFoundException();
            }
            return id;
        }
        public List<Order> GetAllOrders()
        {
            return _orders;
        }
        public Order GetOrder(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            IEnumerable<Order> orders = _orders.Where(order => order.OrderId == id);
            if (!orders.Any())
            {
                throw new NotFoundException();
            }
            return orders.Single();
        }
        public int DeleteOrder(int id)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            Order order = GetOrder(id);
            if (order == null)
            {
                throw new NotFoundException();
            }
            _orders.Remove(order);
            return id;
        }
        public void DeleteAllOrders()
        {
            _orders.Clear();
        }
    }
}
