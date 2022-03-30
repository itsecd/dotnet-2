

using AccountingSystem.Connection;
using AccountingSystem.Model;
using NHibernate;
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

        public void AddProduct(Product product)
        {
            using (session.BeginTransaction())
            {

                session.Save(product);
                session.GetCurrentTransaction().Commit();
            }
        }

        public void ChangeProduct(int id, Product newProduct)
        {
            using (session.BeginTransaction())
            {
                Product product = session.Get<Product>(id);
                product.Name = newProduct.Name;
                product.Price = newProduct.Price;
                product.Date = newProduct.Date;
                session.Save(product);
                session.GetCurrentTransaction().Commit();
            }
        }

        public void RemoveProduct(int id)
        {
            using (session.BeginTransaction())
            {
                session.Delete(session.Get<Product>(id));
                session.GetCurrentTransaction().Commit();
            }
        }
    }
}
