using Google.Protobuf.Collections;
using Grpc.Core;
using Microsoft.Extensions.Logging;
using OrderAccountingSystem.Exceptions;
using OrderAccountingSystem.Model;
using OrderAccountingSystem.Repositories;
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

        public AccountingSystemService(IProductRepository productRepository, ICustomerRepository customerRepository, ILogger<AccountingSystemService> logger)
        {
            _customerRepository = customerRepository;
            _productRepository = productRepository;
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
            catch(NotFoundException)
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
            catch(ArgumentException)
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
            AllCustomerReply CustomerReply = new AllCustomerReply();
            foreach (Customer customer in _customerRepository.GetAllCustomersAsync().Result)
            {
                CustomerReply.Customers.Add(new CustomerReply
                {
                    CustomerId = customer.CustomerId.ToString(),
                    Name = customer.Name,
                    Phone = customer.Phone
                });
            }
            return Task.FromResult(CustomerReply);
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
    }
}
