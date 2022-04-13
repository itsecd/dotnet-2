using AccountingSystem.Connection;
using AccountingSystem.Exeption;
using AccountingSystem.Model;
using NHibernate;
using NHibernate.Criterion;
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
            IList<Order> validateOrder = NHibernateSession.OpenSession().Query<Order>().Where(f => f.Date.AddDays(30) > DateTime.Today).ToList();
            return validateOrder.Sum(f => f.Products.Count);
        }

        public int AddOrder(Order order)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
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
                order.Customer = newOrder.Customer;
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
            foreach(Product product in order.Products)
            {
                if(product.ProductId == productId)
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
