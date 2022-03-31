using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
using System;
using System.Collections.Generic;


namespace AccountingSystem.Repository
{
    public class CustomerRepository : ICustomerRepository
    {

        private static ISession session = NHibernateSession.OpenSession();

        public IList<Customer> GetCustomers()
        {
            ICriteria criteria = session.CreateCriteria<Customer>();
            return criteria.List<Customer>();
        }

        public Customer GetCustomer(int id)
        {
            return session.Get<Customer>(id);
        }

        public int AddCustomer(Customer customer)
        {
            try
            { 
                using (session.BeginTransaction())
                {
                    session.Save(customer);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }            
            catch
            {
                return 0;
            }
        }

        public int ChangeCustomer(int id, Customer newCustomer)
        {
            try
            {
                using (session.BeginTransaction())
                {
                    Customer customer = session.Get<Customer>(id);
                    customer.Name = newCustomer.Name;
                    customer.Phone = newCustomer.Phone;
                    customer.Address = newCustomer.Address;
                    session.Save(customer);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }
            catch
            {
                return 0;
            }
        }

        public int RemoveCustomer(int id)
        {
            try
            { 
                using (session.BeginTransaction())
                {
                    session.Delete(session.Get<Customer>(id));
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }            
            catch
            {
                return 0;
            }

        }
    }
}
