using AccountingSystem.Connection;
using AccountingSystem.Exeption;
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
            Order order = NHibernateSession.OpenSession().Get<Order>(id);
            if (order == null)
            {
                throw new NoFoundInDataBaseExeption();
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
                    throw new NoFoundInDataBaseExeption();
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
                    throw new NoFoundInDataBaseExeption();
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
                    throw new NoFoundInDataBaseExeption();
                }
                session.Delete(order);
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
