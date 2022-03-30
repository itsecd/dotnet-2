using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
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

        public void AddOrder(Order order)
        {
            using (session.BeginTransaction())
            {
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
        }

        public void ChangeOrder(int id, Order newOrder)
        {
            using (session.BeginTransaction())
            {
                Order order = session.Get<Order>(id);
                order.Customer = newOrder.Customer;
                order.Price = newOrder.Price;
                order.Status = newOrder.Status;
                order.Products = newOrder.Products;
                session.Save(order);
                session.GetCurrentTransaction().Commit();
            }
        }

        public void RemoveOrder(int id)
        {
            using (session.BeginTransaction())
            {
                session.Delete(session.Get<Order>(id));
                session.GetCurrentTransaction().Commit();
            }
        }
    }
}
