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
    public class ProductService : ProductGreeter.ProductGreeterBase
    {
        private readonly ILogger<ProductService> _logger;
        private readonly IProductRepository _repository;

        public ProductService(IProductRepository repository, ILogger<ProductService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public override Task<AllProductReply> GetAllProducts(NullRequest request, ServerCallContext context)
        {
            AllProductReply productReply = new AllProductReply();
            foreach(Product product in _repository.GetAllProducts())
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
                Product product = _repository.GetProduct(Guid.Parse(request.ProductId));
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
                    ProductId = _repository.AddProduct(new Product(request.Name, request.Price)).ToString()
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
                    ProductId = _repository.ChangeProduct(Guid.Parse(request.ProductId), new Product(request.Name, request.Price)).ToString()
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
                    ProductId = _repository.DeleteProduct(Guid.Parse(request.ProductId)).ToString()
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
    }
}
