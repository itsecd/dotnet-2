using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

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
            return NHibernateSession.OpenSession().Get<Order>(id);
        }
        public double GetAllPrice()
        {
            ICriteria criteria = NHibernateSession.OpenSession().CreateCriteria<Order>();
            return criteria.List<Order>().Sum(f => f.Price);
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
                order.Customer = newOrder.Customer;
                order.Price = CalculationPrice(newOrder);
                order.Status = newOrder.Status;
                order.Products = newOrder.Products;
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
            return id;
        }

        public int PatchStatus(int status, Order order)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                order.Status = status;
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
            return order.OrderId;
        }

        public int RemoveOrder(int id)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                session.Delete(session.Get<Order>(id));
                session.GetCurrentTransaction().Commit();
            }
            return id;
        }

        private double CalculationPrice(Order order)
        {
            return order.Products.Sum(f => f.Price);
        }
    }
}
