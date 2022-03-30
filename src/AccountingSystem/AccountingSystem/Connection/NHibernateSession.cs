using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AccountingSystem.Connection
{
    public class NHibernateSession
    {
        private static ISessionFactory fabrica = CreateSessionFactory();

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
            return fabrica.OpenSession();
        }

    }
}
