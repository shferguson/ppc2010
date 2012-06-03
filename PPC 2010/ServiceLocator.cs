﻿using PPC_2010.Data;
using System.Configuration;
using StructureMap;

namespace PPC_2010
{
    public class ServiceLocator
    {
        #region Singleton

        private static ServiceLocator instance = new ServiceLocator();

        public static ServiceLocator Instance { get { return instance; } }

        #endregion

        #region Implementation

        private readonly IContainer _container = new Container();

        protected ServiceLocator()
        {
            _container.Configure(x =>
            {
                x.For<Data.LinqToSql.ProvidenceDbDataContext>().HttpContextScoped().Use(() => new Data.LinqToSql.ProvidenceDbDataContext(ConfigurationManager.AppSettings["umbracoDbDSN"]));
                x.For<ISermonRepository>().HttpContextScoped().Use<Data.LinqToSql.SermonRepository>();
                x.For<IArticleRepository>().HttpContextScoped().Use<Data.LinqToSql.ArticleRepository>();
                x.For<IPreValueRepository>().HttpContextScoped().Use<Data.LinqToSql.PreValueRepository>();
            });
        }

        public T Locate<T>()
        {
            return _container.GetInstance<T>();
        }

        #endregion     
    }
}