using Microsoft.Practices.Unity;
using PPC_2010.Data;

namespace PPC_2010
{
    public class ServiceLocator
    {
        #region Singleton

        private static ServiceLocator instance = new ServiceLocator();

        public static ServiceLocator Instance { get { return instance; } }

        #endregion

        #region Implementation

        private IUnityContainer container = new UnityContainer();

        protected ServiceLocator()
        {
            container.RegisterType<ISermonRepository, Data.LinqToSql.LinqToSqlSermonRepository>();
        }

        public T Locate<T>()
        {
            return container.Resolve<T>();
        }

        #endregion
    }
       
}