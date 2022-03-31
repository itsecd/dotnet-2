using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
using System;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private static ISession session = NHibernateSession.OpenSession();

        public IList<Order> GetOrders()
        {
            ICriteria criteria = session.CreateCriteria<Order>();
            return criteria.List<Order>();
        }

        public Order GetOrder(int id)
        {
            return session.Get<Order>(id);
        }
        public double GetAllPrice()
        {
            ICriteria criteria = session.CreateCriteria<Order>();
            double sumPrice = 0;
            foreach (Order order in criteria.List<Order>())
            {
                sumPrice += order.Price;
            }
            return sumPrice;
        }

        public int AddOrder(Order order)
        {
            try 
            { 
                using (session.BeginTransaction())
                {
                    order.Price = calculationPrice(order);
                    session.Save(order);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch(NullReferenceException)
            {
                return 0;
            }
}

        public int ChangeOrder(int id, Order newOrder)
        {
            try
            { 
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
            catch(NullReferenceException)
            {
                return 0;
            }
        }

        public int ChangeOrderStatus(int id, int newStatus)
        {
            try
            { 
                using (session.BeginTransaction())
                {
                    Order order = session.Get<Order>(id);
                    order.Status = newStatus;
                    session.Save(order);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch(NullReferenceException)
            {
                return 0;
            }
}

        public int RemoveOrder(int id)
        {
            try 
            { 
                using (session.BeginTransaction())
                {
                    session.Delete(session.Get<Order>(id));
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch(NullReferenceException)
            {
                return 0;
            }
}

        public double calculationPrice(Order order)
        {
            double price = 0;
            if (order.Products.Count > 0)
            {
                foreach (Product product in order.Products)
                {
                    price += product.Price;
                }
            }
            return price;
        }
    }
}
