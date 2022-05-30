using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Laba2Client.Properties;

namespace Laba2Client
{
    public class OrderSystemRepository
    {
        private readonly OpenApiClient _openApiClient;
        public OrderSystemRepository()
        {
            var httpClient = new HttpClient();
            var baseUrl = Settings.Default.OpenApiService;
            _openApiClient = new OpenApiClient(baseUrl, httpClient);
        }
        public Task<ICollection<Customer>> GetAllCustomers()
        {
            return _openApiClient.CustomerAllAsync();
        }
        public Task<int> AddCustomer(Customer customer)
        {
            return _openApiClient.CustomerAsync(customer);
        }
        public Task DeleteAllCustomer()
        {
            return _openApiClient.Customer2Async();
        }
        public Task<Customer> GetCustomer(int id)
        {
            return _openApiClient.Customer3Async(id);
        }
        public Task<int> ReplaceCustomer(int id, Customer newCustomer)
        {
            return _openApiClient.Customer4Async(id, newCustomer);
        }
        public Task<int> DeleteCustomer(int id)
        {
            return _openApiClient.Customer5Async(id);
        }
        public Task<ICollection<Order>> GetAllOrders()
        {
            return _openApiClient.OrderAllAsync();
        }
        public Task<int> AddOrder(Order order)
        {
            return _openApiClient.OrderAsync(order);
        }
        public Task DeleteAllOrder()
        {
            return _openApiClient.Order2Async();
        }
        public Task<Order> GetOrder(int id)
        {
            return _openApiClient.Order3Async(id);
        }
        public Task<int> ReplaceOrder(int id, Order newOrder)
        {
            return _openApiClient.Order4Async(id, newOrder);
        }
        public Task<int> DeleteOrder(int id)
        {
            return _openApiClient.Order5Async(id);
        }
        public Task<ICollection<Product>> GetAllProducts(int id)
        {
            return _openApiClient.ProductsAllAsync(id);
        }
        public Task<int> AddProduct(int id, Product product)
        {
            return _openApiClient.ProductsAsync(id, product);
        }
        public Task<int> DeleteProducts(int id)
        {
            return _openApiClient.Products2Async(id);
        }
        public Task<Product> GetProduct(int id, int num)
        {
            return _openApiClient.Products3Async(id, num);
        }
        public Task<int> DeleteProduct(int id, int num)
        {
            return _openApiClient.Products4Async(id, num);
        }
        public Task<int> ReplaceProduct(int id, int num, Product newProduct)
        {
            return _openApiClient.Products5Async(id, num, newProduct);
        }
        public Task<IDictionary<string, int>> MonthlyReport()
        {
            return _openApiClient.MonthlyReportAsync();
        }
        public Task<double> MonthlyTotalCost()
        {
            return _openApiClient.MonthlyTotalCostAsync();
        }
    }
}
