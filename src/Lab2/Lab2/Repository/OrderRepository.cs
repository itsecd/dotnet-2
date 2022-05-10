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
        private List<Order> _orders;
        public OrderRepository()
        {
            _orders = new();
        }
        public OrderRepository(IConfiguration configuration)
        {
            _storageFileName = configuration.GetValue<string>("OrdersFile");
        }
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
        public int AddOrder(Order order)
        {
            if (_orders.Count == 0)
            {
                order.OrderId = 1;
            }
            else
            {
                order.OrderId = _orders.Max(od => od.OrderId) + 1;
            }
            if (order.CustomerId == 0)
            {
                throw new ArgumentException();
            }
            _orders.Add(order);
            order.AmountOrder = GetAllCostOrder(order);
            return order.OrderId;
        }
        public int ReplaceOrder(int id, Order newOrder)
        {
            if (id < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            var orderIndex = _orders.FindIndex(order => order.OrderId == id);
            if (orderIndex == -1)
            {
                throw new NotFoundException();
            }
            Order order = _orders[orderIndex];
            order.Products = newOrder.Products;
            order.Dt = newOrder.Dt;
            order.AmountOrder = GetAllCostOrder(newOrder);
            order.Status = newOrder.Status;
            order.CustomerId = newOrder.CustomerId;
            
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
        
        public List<Product> GetProducts(int id)
        {
            Order order = GetOrder(id);
            if(order == null)
            {
                throw new NotFoundException();
            }
            return order.Products;
        }
        public int AddProduct(int id, Product product)
        {
            Order order = GetOrder(id);
            if (order == null)
            {
                throw new NotFoundException();
            }
            order.Products.Add(product);
            order.AmountOrder = GetAllCostOrder(order);
            return id;
        }
        public Product GetProduct(int id, int num)
        {
            Order order = GetOrder(id);
            return order.Products[num - 1];
        }
        public int ReplaceProduct(int id, int num, Product newProduct)
        {
            Order order = GetOrder(id);
            Product product = GetProduct(id, num);
            product.NameProduct = newProduct.NameProduct;
            product.CostProduct = newProduct.CostProduct;
            order.AmountOrder = GetAllCostOrder(order);
            return id;
        }
        public int RemoveProduct(int id, int num)
        {
            Order order = GetOrder(id);
            order.Products.Remove(GetProduct(id, num));
            order.AmountOrder = GetAllCostOrder(order);
            return id;
        }
        public int RemoveAllProducts(int id)
        {
            Order order = GetOrder(id);
            order.Products.Clear();
            order.AmountOrder = GetAllCostOrder(order);
            return id;
        }
        public Dictionary<string,int> GetProductsMonth()
        {
            return (from order in _orders
                         from product in order.Products
                         where (DateTime.Now - order.Dt).Days < 30
                         group product by product.NameProduct
                         into productGroup
                         orderby productGroup.Count() descending
                         select new KeyValuePair<string, int>(productGroup.Key, productGroup.Count()))
                         .ToDictionary(pair => pair.Key, pair => pair.Value);
        }
        public float GetAllCostOrder(Order order)
        {
            return order.Products.Sum(product => product.CostProduct);
        }
        public float GetAmountMonth()
        {
            List<Order> orders = _orders.Where(order => DateTime.Today < order.Dt.AddDays(30)).ToList();
            return orders.Sum(order => order.AmountOrder);
        }
    }
}
