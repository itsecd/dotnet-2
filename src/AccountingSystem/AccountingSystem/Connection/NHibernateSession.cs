using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.Reflection;

namespace AccountingSystem.Connection
{
    public static class NHibernateSession
    {
        private static readonly ISessionFactory Factory = CreateSessionFactory();

        private static ISessionFactory CreateSessionFactory()
        {
            Configuration cfg = NHibernateSession.RecoverConfiguration();
            return cfg.BuildSessionFactory();
        }

        public static Configuration RecoverConfiguration()
        {
            Configuration cfg = new Configuration();
            cfg.Configure();
            cfg.AddAssembly(Assembly.GetExecutingAssembly());
            return cfg;
        }

        public static void GenerateSchema()
        {
            Configuration cfg = RecoverConfiguration();
            new SchemaExport(cfg).Create(true, true);
        }

        public static ISession OpenSession()
        {
            return Factory.OpenSession();
        }

    }
}
