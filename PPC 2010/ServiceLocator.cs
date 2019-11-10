using System.Configuration;
using PPC_2010.CalendarInterface;
using PPC_2010.Data;
using StructureMap;
using StructureMap.Web;
using Umbraco.Core;
using Umbraco.Core.Services;
using PPC_2010.Services;

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
                x.For<IGoogleCalendarService>().HttpContextScoped().Use(
                    () => new GoogleCalendarService(ConfigurationManager.AppSettings["googleServiceAccountEmail"], 
                                                    ConfigurationManager.AppSettings["googleServiceAccountKeyFilePath"]));
            });
        }

        public T Locate<T>()
        {
            return _container.GetInstance<T>();
        }

        #endregion     
    }
}