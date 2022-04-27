using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Lab2.Model;

namespace Lab2.Repository
{
    public class OrderRepository: IOrderRepository
    {
        private readonly string _storageFileName = "orders.json";
        public List<Order> _orders { get; set; }
        private async Task ReadFromFile()
        {
            
            if (!File.Exists(_storageFileName))
            {
                _orders = new List<Order>();
                return;
            }
            await DeserializationFile();
        }
        private async Task DeserializationFile()
        {
            //using var streamFile = new FileStream(_storageFileName, FileMode.Open);
            //_orders = await JsonSerializer.DeserializeAsync<List<Order>>(streamFile);
            using (var fileReader = new StreamReader(_storageFileName))
            {
                string jsonString = fileReader.ReadToEnd();
                _orders = JsonSerializer.Deserialize < List<Order>>(jsonString);
            }

        }
        private async Task SerializationFile()
        {
            //using var streamFile = new FileStream(_storageFileName, FileMode.Create);
            //await JsonSerializer.SerializeAsync(streamFile, _orders);
            string jsonString = JsonSerializer.Serialize(_orders);
            using (var fileWriter = new StreamWriter(_storageFileName))
            {
                fileWriter.Write(jsonString);
            }
        }
        private async Task WriteToFile()
        {
            await SerializationFile();
        }
        public void AddOrder(Order order)
        {
            ReadFromFile();
            _orders.Add(order);
            WriteToFile();

        }
        public void ReplaceOrder(Order order, int id)
        {
            ReadFromFile();
            var orderIndex = _orders.FindIndex(order => order.OrderId == id);
            if (orderIndex > 0)
            {
                _orders[orderIndex] = order;
            }
            WriteToFile();
        }
        public List<Order> GetAllOrders()
        {
            ReadFromFile();
            return _orders;
        }
        public Order GetOrder(int id)
        {
            ReadFromFile();
            Order order = _orders.Where(order => order.OrderId == id).Single();
            return order;
        }
        public void DeleteOrder(int id)
        {
            ReadFromFile();
            _orders.Remove(_orders.Single(order => order.OrderId == id));
            WriteToFile();

        }
        public void DeleteAllOrders()
        {
            ReadFromFile();
            _orders.Clear();
            WriteToFile();
        }
    }
}
