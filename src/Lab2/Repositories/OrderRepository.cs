using Lab2.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace Lab2.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private const string StorageFileName = "DataOrders.xml";
        private List<Order>? _orders;

        public void ReadFromFile()
        {
            if (_orders != null) return;
            if (!File.Exists(StorageFileName))
            {
                _orders = new List<Order>();
                return;
            }
            var xmlSerializer = new XmlSerializer(typeof(List<Order>));
            using var fileStream = new FileStream(StorageFileName, FileMode.Open);
            _orders = (List<Order>)(xmlSerializer.Deserialize(fileStream) ?? throw new InvalidOperationException());
        }

        public void WriteToFile()
        {
            var xmlSerializer = new XmlSerializer(typeof(List<Order>));
            using (var fileStream = new FileStream(StorageFileName, FileMode.Create))
            {
                xmlSerializer.Serialize(fileStream, _orders);
            }
        }

        public int Add(Order order)
        {
            ReadFromFile();
            if (order == null) throw new ArgumentNullException();
            if (order.OrderId < 0) throw new ArgumentException("ID invalid!");
            _orders!.Add(order);
            WriteToFile();
            return order.OrderId;
        }

        public int Change(int id, Order newOrder)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            if (newOrder == null) throw new ArgumentNullException();
            var orderIndex = _orders!.FindIndex(ord => ord.OrderId == id);
            if (orderIndex == -1) throw new NotFoundException();
            var order = _orders[orderIndex];
            order.Products = newOrder.Products;
            order.Date = newOrder.Date;
            order.OrderStatus = newOrder.OrderStatus;
            order.CustomerId = newOrder.CustomerId;
            WriteToFile();
            return id;
        }

        public List<Order> ListOrders()
        {
            ReadFromFile();
            return _orders!;
        }

        public Order GetOrder(int id)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            var order = _orders!.FirstOrDefault(ord => ord.OrderId == id);
            if (order == null) throw new NotFoundException();
            return order;
        }

        public int Remove(int id)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            var order = GetOrder(id);
            if (order == null) throw new NotFoundException();
            _orders!.Remove(order);
            WriteToFile();
            return id;
        }

        public List<Product> ListProducts(int id)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            var order = _orders!.Where(ord => ord.OrderId == id).Single();
            if (order == null) throw new NotFoundException();
            return order.Products!;
        }

        public int AddProduct(int id, Product product)
        {
            ReadFromFile();
            if (id < 0) throw new ArgumentOutOfRangeException();
            var order = GetOrder(id);
            if (product == null) throw new ArgumentNullException();
            order.Products!.Add(product);
            WriteToFile();
            return id;
        }

        public Product GetProduct(int orderId, int productId)
        {
            ReadFromFile();
            if (orderId < 0 || productId < 0) throw new ArgumentOutOfRangeException();
            var order = GetOrder(orderId);
            if (order == null) throw new NotFoundException("Order ID does not exist!");
            var product = order.Products!.FirstOrDefault(pro => pro.Id == productId);
            if (product == null) throw new NotFoundException("Product ID does not exist!");
            return product;
        }

        public int ChangeProduct(int orderId, int productId, Product newProduct)
        {
            ReadFromFile();
            if (orderId < 0 || productId < 0) throw new ArgumentOutOfRangeException();
            var order = GetOrder(orderId);
            if (order == null) throw new NotFoundException("Order ID does not exist!");
            var product = GetProduct(orderId, productId);
            if (product == null) throw new NotFoundException("Product ID does not exist!");
            product.Id = newProduct.Id;
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            WriteToFile();
            return productId;
        }

        public int RemoveProduct(int orderId, int productId)
        {
            ReadFromFile();
            if (orderId < 0 || productId < 0) throw new ArgumentOutOfRangeException();
            var order = GetOrder(orderId);
            if (order == null) throw new NotFoundException("Order ID does not exist!");
            var product = GetProduct(orderId, productId);
            if (product == null) throw new NotFoundException("Product ID does not exist!");
            order.Products!.Remove(product);
            WriteToFile();
            return productId;
        }

        public KeyValuePair<Product, float> BestProductOfMonth()
        {
            ReadFromFile();
            var ListProduct = (from order in _orders
                               from product in order.Products!
                               where (DateTime.Now - order.Date).Days < 30
                               group product by product
                              into productGroup
                               orderby productGroup.Count() descending
                               select new KeyValuePair<Product, float>(productGroup.Key, productGroup.Count()))
                              .ToDictionary(pair => pair.Key, pair => pair.Value * pair.Key.Price);
            var MaxProduct = ListProduct.FirstOrDefault(pair => pair.Value == ListProduct.Max(pro => pro.Value));
            return MaxProduct;
        }

        public float GetSumMonth()
        {
            var orders = _orders!.Where(ord => DateTime.Today < ord.Date.AddDays(30)).ToList();
            return _orders!.Sum(ord => ord.SumOrder);
        }
    }
}
