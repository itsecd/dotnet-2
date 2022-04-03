using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
using System.Collections.Generic;


namespace AccountingSystem.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        public IList<Customer> GetCustomers()
        {
            ICriteria criteria = NHibernateSession.OpenSession().CreateCriteria<Customer>();
            return criteria.List<Customer>();
        }

        public Customer GetCustomer(int id)
        {
            return NHibernateSession.OpenSession().Get<Customer>(id);
        }

        public int AddCustomer(Customer customer)
        {
            try
            {
                ISession session = NHibernateSession.OpenSession();
                using (session.BeginTransaction())
                {
                    session.Save(customer);
                    session.GetCurrentTransaction().Commit();
                }
                return customer.CustomerId;
            }
            catch
            {
                return -1;
            }
        }

        public int ChangeCustomer(int id, Customer newCustomer)
        {
            try
            {
                ISession session = NHibernateSession.OpenSession();
                using (session.BeginTransaction())
                {
                    Customer customer = session.Get<Customer>(id);
                    customer.Name = newCustomer.Name;
                    customer.Phone = newCustomer.Phone;
                    customer.Address = newCustomer.Address;
                    session.Save(customer);
                    session.GetCurrentTransaction().Commit();
                }
                return id;
            }
            catch
            {
                return -1;
            }
        }

        public int RemoveCustomer(int id)
        {
            try
            {
                ISession session = NHibernateSession.OpenSession();
                using (session.BeginTransaction())
                {
                    session.Delete(session.Get<Customer>(id));
                    session.GetCurrentTransaction().Commit();
                }
                return id;
            }
            catch
            {
                return -1;
            }

        }
    }
}
