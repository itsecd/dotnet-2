﻿using AccountingSystem.Exeption;
using AccountingSystem.Model;
using AccountingSystem.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccountingSystem.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;

        public ProductController(IProductRepository repository)
        {
            _repository = repository;
        }

        /// <summary>Get All Product</summary>
        /// <returns>All Products</returns>
        [HttpGet]
        public ActionResult<IList<Product>> Get()
        {
            try
            {
                return _repository.GetProducts().ToList();
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
        }

        /// <summary>Get Product By ID</summary>
        /// <returns>Product</returns>
        [HttpGet("{id:int}")]
        public ActionResult<Product> Get(int id)
        {
            try
            {
                return _repository.GetProduct(id);
            }
            catch (NoFoundInDataBaseExeption)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Add Product To DataBase</summary>
        /// <returns>Product ID</returns>
        [HttpPost]
        public ActionResult<int> Post([FromBody] Product product)
        {
            try
            {
                return _repository.AddProduct(product);
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Change Product In DataBase</summary>
        /// <returns>Product ID</returns>
        [HttpPut("{id:int}")]
        public ActionResult<int> Put(int id, [FromBody] Product product)
        {
            try
            {
                return _repository.ChangeProduct(id, product);
            }
            catch (NoFoundInDataBaseExeption)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
            catch
            {
                return Conflict();
            }
        }

        /// <summary>Delete Product From DataBase</summary>
        /// <returns>Product ID</returns>
        [HttpDelete("{id:int}")]
        public ActionResult<int> Delete(int id)
        {
            try
            {
                return _repository.RemoveProduct(id);
            }
            catch (NoFoundInDataBaseExeption)
            {
                return NotFound();
            }
            catch (TypeInitializationException)
            {
                return Problem();
            }
            catch
            {
                return Conflict();
            }
        }
    }
}
