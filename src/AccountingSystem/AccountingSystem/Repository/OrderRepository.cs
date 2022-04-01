using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

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
            try
            {
                ISession session = NHibernateSession.OpenSession();
                using (session.BeginTransaction())
                {
                    order.Price = calculationPrice(order);
                    session.Save(order);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        public int ChangeOrder(int id, Order newOrder)
        {
            try
            {
                ISession session = NHibernateSession.OpenSession();
                using (session.BeginTransaction())
                {
                    Order order = session.Get<Order>(id);
                    order.Customer = newOrder.Customer;
                    order.Price = calculationPrice(newOrder);
                    order.Status = newOrder.Status;
                    order.Products = newOrder.Products;
                    session.Save(order);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        public int PatchStatus(int id, int newStatus)
        {
            try
            {
                ISession session = NHibernateSession.OpenSession();
                using (session.BeginTransaction())
                {
                    Order order = session.Get<Order>(id);
                    order.Status = newStatus;
                    session.Save(order);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        public int RemoveOrder(int id)
        {
            try
            {
                ISession session = NHibernateSession.OpenSession();
                using (session.BeginTransaction())
                {
                    session.Delete(session.Get<Order>(id));
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch (NullReferenceException)
            {
                return 0;
            }
        }

        private double calculationPrice(Order order)
        {
            return order.Products.Sum(f => f.Price);
        }
    }
}
