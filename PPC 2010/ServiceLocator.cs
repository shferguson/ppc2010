using System.Configuration;
using PPC_2010.Data;
using StructureMap;
using StructureMap.Web;
using Umbraco.Core;
using Umbraco.Core.Services;
using PPC_2010.Services;
using System.Net;

namespace PPC_2010
{
    public class ServiceLocater
    {
        #region Singleton

        private static ServiceLocater instance = new ServiceLocater();

        static ServiceLocater()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        public static ServiceLocater Instance { get { return instance; } }

        #endregion

        #region Implementation

        private readonly IContainer _container = new Container();

        protected ServiceLocater()
        {
            _container.Configure(x =>
            {
                x.For<Data.LinqToSql.ProvidenceDbDataContext>().HttpContextScoped().Use(() => new Data.LinqToSql.ProvidenceDbDataContext(ConfigurationManager.ConnectionStrings["umbracoDbDSN"].ConnectionString));
                x.For<ISermonRepository>().HttpContextScoped().Use<Data.LinqToSql.SermonRepository>();
                x.For<IArticleRepository>().HttpContextScoped().Use<Data.LinqToSql.ArticleRepository>();
                x.For<IPreValueRepository>().HttpContextScoped().Use<Data.LinqToSql.PreValueRepository>();
                x.For<IMediaService>().Use(() => ApplicationContext.Current.Services.MediaService);
                x.For<IDataTypeService>().Use(() => ApplicationContext.Current.Services.DataTypeService);
                x.For<IEmailGroupRepository>().HttpContextScoped().Use<Data.Media.EmailGroupRepository>();
                x.For<IMetaTagService>().Use<MetaTagService>();
                x.For<Social.Facebook.IOpenGraphTagsService>().Use<Social.Facebook.OpenGraphTagsService>();
                x.For<Social.Twitter.ITwitterTagService>().Use<Social.Twitter.TwitterTagService>();
                x.For<Social.ISocialTagsService>().Use<Social.SocialTagsService>();
                x.For<Social.Twitter.ITwitterTagService>().Use<Social.Twitter.TwitterTagService>();
                x.For<SermonPublishApi>().Use<SermonPublishApi>().Singleton();
                x.For<IMp3FileService>().Use<Mp3FileService>();
            });
        }

        public T Locate<T>()
        {
            return _container.GetInstance<T>();
        }

        #endregion     
    }
}