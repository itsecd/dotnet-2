

using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
using System;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public class ProductRepository : IProductRepository 
    { 

        private static ISession session = NHibernateSession.OpenSession();

        public IList<Product> GetProducts()
        {
            ICriteria criteria = session.CreateCriteria<Product>();
            return criteria.List<Product>();
        }

        public Product GetProduct(int id)
        {
            return session.Get<Product>(id);
        }

        public int AddProduct(Product product)
        {
            try
            { 
                using (session.BeginTransaction())
                {
                    session.Save(product);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }            
            catch
            {
                return 0;
            }
        }

        public int ChangeProduct(int id, Product newProduct)
        {
            try
            {
                Product product = session.Get<Product>(id);
                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                product.Date = newProduct.Date;
                using (session.BeginTransaction())
                {
                    session.Save(product);
                    session.GetCurrentTransaction().Commit();
                }
                return 1;
            }            
            catch
            {
                return 0;
            }

        }

        public int RemoveProduct(int id)
        {
            try
            { 
                using (session.BeginTransaction())
                {
                    session.Delete(session.Get<Product>(id));
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
