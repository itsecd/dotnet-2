using AccountingSystem.Connection;
using AccountingSystem.Exeption;
using AccountingSystem.Model;
using NHibernate;
using System.Collections.Generic;

namespace AccountingSystem.Repository
{
    public class ProductRepository : IProductRepository
    {

        public IList<Product> GetProducts()
        {
            ICriteria criteria = NHibernateSession.OpenSession().CreateCriteria<Product>();
            return criteria.List<Product>();
        }

        public Product GetProduct(int id)
        {
            Product product = NHibernateSession.OpenSession().Get<Product>(id);
            if (product == null)
            {
                throw new NotFoundInDatabaseException();
            }
            return product;
        }

        public int AddProduct(Product product)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                session.Save(product);
                session.GetCurrentTransaction().Commit();
            }
            return product.ProductId;
        }

        public int ChangeProduct(int id, Product newProduct)
        {
            ISession session = NHibernateSession.OpenSession();
            Product product = session.Get<Product>(id);
            if (product == null)
            {
                throw new NotFoundInDatabaseException();
            }
            product.Name = newProduct.Name;
            product.Price = newProduct.Price;
            product.Date = newProduct.Date;
            using (session.BeginTransaction())
            {
                session.Save(product);
                session.GetCurrentTransaction().Commit();
            }
            return id;

        }

        public int RemoveProduct(int id)
        {
            ISession session = NHibernateSession.OpenSession();
            using (session.BeginTransaction())
            {
                Product product = session.Get<Product>(id);
                if (product == null)
                {
                    throw new NotFoundInDatabaseException();
                }
                session.Delete(product);
                session.GetCurrentTransaction().Commit();
            }
            return id;
        }
    }
}
