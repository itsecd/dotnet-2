using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OrderAccountingSystem.Exceptions;
using OrderAccountingSystem.Models;
using OrderAccountingSystem.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrderAccountingSystem
{
    public class AccountingSystemService : AccountingSystemGreeter.AccountingSystemGreeterBase
    {
        private readonly ILogger<AccountingSystemService> _logger;
        private readonly IProductRepository _productRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IOrderRepository _orderRepository;

        public AccountingSystemService(IProductRepository productRepository, ICustomerRepository customerRepository, IOrderRepository orderRepository, ILogger<AccountingSystemService> logger)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
            _orderRepository = orderRepository;
            _logger = logger;
        }

        public override Task<AllProductReply> GetAllProducts(NullRequest request, ServerCallContext context)
        {
            AllProductReply productReply = new AllProductReply();
            foreach (Product product in _productRepository.GetAllProductsAsync().Result)
            {
                productReply.Products.Add(new ProductReply
                {
                    ProductId = product.ProductId.ToString(),
                    Name = product.Name,
                    Price = product.Price
                });
            }
            return Task.FromResult(productReply);
        }

        public override Task<ProductReply> GetProduct(ProductRequest request, ServerCallContext context)
        {
            try
            {
                Product product = _productRepository.GetProductAsync(Guid.Parse(request.ProductId)).Result;
                return Task.FromResult(new ProductReply
                {
                    ProductId = product.ProductId.ToString(),
                    Name = product.Name,
                    Price = product.Price
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<ProductReply> AddProduct(ProductRequest request, ServerCallContext context)
        {
            try
            {
                return Task.FromResult(new ProductReply
                {
                    ProductId = _productRepository.AddProductAsync(new Product(request.Name, request.Price)).Result.ToString()
                });
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        ArgumentException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<ProductReply> ChangeProduct(ProductRequest request, ServerCallContext context)
        {
            try
            {
                return Task.FromResult(new ProductReply
                {
                    ProductId = _productRepository.ChangeProductAsync(Guid.Parse(request.ProductId), new Product(request.Name, request.Price)).Result.ToString()
                });
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        ArgumentException = true
                    }
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<ProductReply> DeleteProduct(ProductRequest request, ServerCallContext context)
        {
            try
            {
                return Task.FromResult(new ProductReply
                {
                    ProductId = _productRepository.DeleteProductAsync(Guid.Parse(request.ProductId)).Result.ToString()
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new ProductReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<AllCustomerReply> GetAllCustomers(NullRequest request, ServerCallContext context)
        {
            AllCustomerReply customerReply = new AllCustomerReply();
            foreach (Customer customer in _customerRepository.GetAllCustomersAsync().Result)
            {
                customerReply.Customers.Add(new CustomerReply
                {
                    CustomerId = customer.CustomerId.ToString(),
                    Name = customer.Name,
                    Phone = customer.Phone
                });
            }
            return Task.FromResult(customerReply);
        }


        public override Task<CustomerReply> GetCustomer(CustomerRequest request, ServerCallContext context)
        {
            try
            {
                Customer customer = _customerRepository.GetCustomerAsync(Guid.Parse(request.CustomerId)).Result;
                return Task.FromResult(new CustomerReply
                {
                    CustomerId = customer.CustomerId.ToString(),
                    Name = customer.Name,
                    Phone = customer.Phone
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<CustomerReply> AddCustomer(CustomerRequest request, ServerCallContext context)
        {
            try
            {
                return Task.FromResult(new CustomerReply
                {
                    CustomerId = _customerRepository.AddCustomerAsync(new Customer(request.Name, request.Phone)).Result.ToString()
                });
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        ArgumentException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<CustomerReply> ChangeCustomer(CustomerRequest request, ServerCallContext context)
        {
            try
            {
                return Task.FromResult(new CustomerReply
                {
                    CustomerId = _customerRepository.ChangeCustomerAsync(Guid.Parse(request.CustomerId), new Customer(request.Name, request.Phone)).Result.ToString()
                });
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        ArgumentException = true
                    }
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<CustomerReply> DeleteCustomer(CustomerRequest request, ServerCallContext context)
        {
            try
            {
                return Task.FromResult(new CustomerReply
                {
                    CustomerId = _customerRepository.DeleteCustomerAsync(Guid.Parse(request.CustomerId)).Result.ToString()
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new CustomerReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }


        public override Task<AllOrderReply> GetAllOrders(NullRequest request, ServerCallContext context)
        {
            AllOrderReply allOrderReply = new AllOrderReply();
            foreach (Order order in _orderRepository.GetAllOrdersAsync().Result)
            {
                OrderReply orderReply = new OrderReply();
                orderReply.OrderId = order.OrderId.ToString();
                orderReply.Customer = new CustomerReply
                {
                    CustomerId = order.Customer.CustomerId.ToString(),
                    Name = order.Customer.Name,
                    Phone = order.Customer.Phone
                };
                foreach (Product product in order.Products)
                {
                    orderReply.Products.Add(new ProductReply
                    {
                        ProductId = product.ProductId.ToString(),
                        Name = product.Name,
                        Price = product.Price
                    });
                }
                orderReply.Price = order.Price;
                orderReply.Status = order.Status;
                orderReply.Date = Timestamp.FromDateTimeOffset(order.Date);
                allOrderReply.Orders.Add(orderReply);
            }
            return Task.FromResult(allOrderReply);
        }

        public override Task<OrderReply> GetOrder(OrderRequest request, ServerCallContext context)
        {
            try
            {
                Order order = _orderRepository.GetOrderAsync(Guid.Parse(request.OrderId)).Result;
                OrderReply orderReply = new OrderReply();
                orderReply.OrderId = order.OrderId.ToString();
                orderReply.Customer = new CustomerReply
                {
                    CustomerId = order.Customer.CustomerId.ToString(),
                    Name = order.Customer.Name,
                    Phone = order.Customer.Phone
                };
                foreach (Product product in order.Products)
                {
                    orderReply.Products.Add(new ProductReply
                    {
                        ProductId = product.ProductId.ToString(),
                        Name = product.Name,
                        Price = product.Price
                    });
                }
                orderReply.Price = order.Price;
                orderReply.Status = order.Status;
                orderReply.Date = Timestamp.FromDateTimeOffset(order.Date);
                return Task.FromResult(orderReply);
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<OrderReply> GetMonthlySale(NullRequest request, ServerCallContext context)
        {
            try
            {
                double price = _orderRepository.GetMonthlySales().Result;
                return Task.FromResult(new OrderReply
                {
                    Price = price
                });
            }
            catch
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<OrderReply> AddOrder(OrderRequest request, ServerCallContext context)
        {
            try
            {
                Order order = new Order();
                order.OrderId = Guid.NewGuid();
                if (IsGuid(request.Customer.CustomerId))
                {
                    if (_customerRepository.CheckCustomerAsync(Guid.Parse(request.Customer.CustomerId)).Result)
                    {
                        order.Customer = _customerRepository.GetCustomerAsync(Guid.Parse(request.Customer.CustomerId)).Result;
                    }
                    else
                    {
                        order.Customer = new Customer(Guid.Parse(request.Customer.CustomerId), request.Customer.Name, request.Customer.Phone);
                        _customerRepository.AddCustomerAsync(order.Customer);
                    }
                }
                else
                {
                    order.Customer = new Customer(request.Customer.Name, request.Customer.Phone);
                    _customerRepository.AddCustomerAsync(order.Customer);
                }

                order.Products = new List<Product>();
                foreach (ProductRequest product in request.Products)
                {
                    if (IsGuid(product.ProductId))
                    {
                        if (_productRepository.CheckProductAsync(Guid.Parse(product.ProductId)).Result)
                        {
                            order.Products.Add(_productRepository.GetProductAsync(Guid.Parse(product.ProductId)).Result);
                        }
                        else
                        {
                            Product newProduct = new Product(Guid.Parse(product.ProductId), product.Name, product.Price);
                            order.Products.Add(newProduct);
                            _productRepository.AddProductAsync(newProduct);
                        }
                    }
                    else
                    {
                        Product newProduct = new Product(product.Name, product.Price);
                        order.Products.Add(newProduct);
                        _productRepository.AddProductAsync(newProduct);
                    }

                }
                order.Status = request.Status;
                order.Date = request.Date.ToDateTimeOffset();
                return Task.FromResult(new OrderReply
                {
                    OrderId = _orderRepository.AddOrderAsync(order).Result.ToString()
                }); ;
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        ArgumentException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<OrderReply> ChangeOrder(OrderRequest request, ServerCallContext context)
        {
            try
            {
                Order order = new Order();
                if (IsGuid(request.Customer.CustomerId))
                {
                    if (_customerRepository.CheckCustomerAsync(Guid.Parse(request.Customer.CustomerId)).Result)
                    {
                        order.Customer = _customerRepository.GetCustomerAsync(Guid.Parse(request.Customer.CustomerId)).Result;
                    }
                    else
                    {
                        order.Customer = new Customer(Guid.Parse(request.Customer.CustomerId), request.Customer.Name, request.Customer.Phone);
                        _customerRepository.AddCustomerAsync(order.Customer);
                    }
                }
                else
                {
                    order.Customer = new Customer(request.Customer.Name, request.Customer.Phone);
                    _customerRepository.AddCustomerAsync(order.Customer);
                }

                order.Products = new List<Product>();
                foreach (ProductRequest product in request.Products)
                {
                    if (IsGuid(product.ProductId))
                    {
                        if (_productRepository.CheckProductAsync(Guid.Parse(product.ProductId)).Result)
                        {
                            order.Products.Add(_productRepository.GetProductAsync(Guid.Parse(product.ProductId)).Result);
                        }
                        else
                        {
                            Product newProduct = new Product(Guid.Parse(product.ProductId), product.Name, product.Price);
                            order.Products.Add(newProduct);
                            _productRepository.AddProductAsync(newProduct);
                        }
                    }
                    else
                    {
                        Product newProduct = new Product(product.Name, product.Price);
                        order.Products.Add(newProduct);
                        _productRepository.AddProductAsync(newProduct);
                    }

                }
                order.Status = request.Status;
                order.Date = request.Date.ToDateTime();
                return Task.FromResult(new OrderReply
                {
                    OrderId = _orderRepository.ChangeOrderAsync(Guid.Parse(request.OrderId), order).Result.ToString()
                });
            }
            catch (ArgumentException)
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        ArgumentException = true
                    }
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<OrderReply> ChangeOrderStatus(OrderRequest request, ServerCallContext context)
        {
            try
            {
                Order order = new Order();
                order.Status = request.Status;
                return Task.FromResult(new OrderReply
                {
                    OrderId = _orderRepository.ChangeOrderStatusAsync(Guid.Parse(request.OrderId), order).Result.ToString()
                }); ;
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }

        public override Task<OrderReply> DeleteOrder(OrderRequest request, ServerCallContext context)
        {
            try
            {
                return Task.FromResult(new OrderReply
                {
                    OrderId = _orderRepository.DeleteOrderAsync(Guid.Parse(request.OrderId)).Result.ToString()
                });
            }
            catch (NotFoundException)
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        NotFoundException = true
                    }
                });
            }
            catch
            {
                return Task.FromResult(new OrderReply
                {
                    ExaminationReply = new ExaminationReply
                    {
                        Problem = true
                    }
                });
            }
        }
        private bool IsGuid(string value)
        {
            Guid _x;
            return Guid.TryParse(value, out _x);
        }
    }
}
