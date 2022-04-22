using AccountingSystem.Connection;
using AccountingSystem.Exeption;
using AccountingSystem.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using Order = AccountingSystem.Model.Order;

namespace AccountingSystem.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public IList<Order> GetOrders()
        {
            ICriteria criteria = NHibernateSession.OpenSession().CreateCriteria<Order>();
            return criteria.List<Order>();
        }

        public Order GetOrder(int id)
        {
            Order order = NHibernateSession.OpenSession().Get<Order>(id);
            if (order == null)
            {
                throw new NotFoundInDatabaseException();
            }
            return order;

        }
        public double GetAllPrice()
        {
            ICriteria criteria = NHibernateSession.OpenSession().CreateCriteria<Order>();
            return criteria.List<Order>().Sum(f => f.Price);
        }

        public int GetCountProductMonthly()
        {
            ICriteria criteria = NHibernateSession.OpenSession().CreateCriteria<Order>();
            List<Order> monthlyOrder = new List<Order>();
            foreach (Order order in criteria.List<Order>())
            {
                if (DateTime.Today < order.Date.AddDays(30))
                {
                    monthlyOrder.Add(order);
                }
            }
            return monthlyOrder.Sum(f => f.Products.Count);
        }

        public int AddOrder(Order order)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                order.Customer = session.Get<Customer>(order.Customer.CustomerId);
                order.Price = CalculationPrice(order);
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
            return order.OrderId;
        }

        public int ChangeOrder(int id, Order newOrder)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                Order order = session.Get<Order>(id);
                if (order == null)
                {
                    throw new NotFoundInDatabaseException();
                }
                order.Customer = session.Get<Customer>(newOrder.Customer.CustomerId); 
                order.Price = CalculationPrice(newOrder);
                order.Status = newOrder.Status;
                order.Date = newOrder.Date;
                order.Products = newOrder.Products;
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
            return id;
        }

        public int PatchStatus(int id, Order newOrder)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                Order order = session.Get<Order>(id);
                if (order == null)
                {
                    throw new NotFoundInDatabaseException();
                }
                order.Status = newOrder.Status;
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
            return id;
        }

        public int RemoveOrder(int id)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                Order order = session.Get<Order>(id);
                if (order == null)
                {
                    throw new NotFoundInDatabaseException();
                }
                session.Delete(order);
                session.GetCurrentTransaction().Commit();
            }
            return id;
        }

        public IList<Product> GetProducts(int id)
        {
            Order order = NHibernateSession.OpenSession().Get<Order>(id);
            if (order == null)
            {
                throw new NotFoundInDatabaseException();
            }
            return order.Products;
        }

        public Product GetProduct(int id, int productId)
        {
            Order order = NHibernateSession.OpenSession().Get<Order>(id);
            if (order == null)
            {
                throw new NotFoundInDatabaseException();
            }
            foreach (Product product in order.Products)
            {
                if (product.ProductId == productId)
                {
                    return product;
                }
            }
            throw new NotFoundInDatabaseException();
        }

        public int AddProduct(Product product, int id)
        {
            ISession session = NHibernateSession.OpenSession();
            Order order = session.Get<Order>(id);
            using (session.BeginTransaction())
            {
                order.Products.Add(product);
                order.Price = CalculationPrice(order);
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
            return order.OrderId;
        }

        public int ChangeProduct(int id, Product newProduct, int productId)
        {
            ISession session = NHibernateSession.OpenSession();
            Order order = session.Get<Order>(id);
            if (order == null)
            {
                throw new NotFoundInDatabaseException();
            }
            foreach (Product product in order.Products)
            {
                if (product.ProductId == productId)
                {
                    product.Name = newProduct.Name;
                    product.Price = newProduct.Price;
                    product.Date = newProduct.Date;
                    order.Price = CalculationPrice(order);
                    using (session.BeginTransaction())
                    {
                        session.Save(order);
                        session.GetCurrentTransaction().Commit();
                    }
                    return order.OrderId;
                }
            }
            throw new NotFoundInDatabaseException();

        }

        public int RemoveProduct(int id, int productId)
        {
            ISession session = NHibernateSession.OpenSession();
            Order order = session.Get<Order>(id);
            if (order == null)
            {
                throw new NotFoundInDatabaseException();
            }
            foreach (Product product in order.Products)
            {
                if (product.ProductId == productId)
                {
                    using (session.BeginTransaction())
                    {
                        order.Products.Remove(product);
                        order.Price = CalculationPrice(order);
                        session.Save(order);
                        session.GetCurrentTransaction().Commit();
                    }
                    return order.OrderId;
                }
            }
            throw new NotFoundInDatabaseException();
        }

        private double CalculationPrice(Order order)
        {
            return order.Products.Sum(f => f.Price);
        }
    }
}
