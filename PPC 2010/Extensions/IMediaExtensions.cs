using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Core.Services;
using System.Collections;
using System.Web.UI.WebControls;
using System;


namespace PPC_2010.Extensions
{
    public static class IMediaExtensions
    {
        public static IEnumerable<string> GetPrevaluesForProperty(this IMedia media, string propertyName)
        {
            var property = media.Properties[propertyName];
            var propertyType = media.PropertyTypes.FirstOrDefault(pt => pt.Alias == property.Alias);
            if (propertyType != null)
            {
                var dataTypeService = ServiceLocater.Instance.Locate<IDataTypeService>();
                return dataTypeService.GetPreValuesByDataTypeId(propertyType.DataTypeDefinitionId);
            }

            return Enumerable.Empty<string>();
        }

        public static string GetPreValue(this IMedia media, string propertyName)
        {
            var dataTypeService = ServiceLocater.Instance.Locate<IDataTypeService>();
            int propertyValue = 0; // For some reason prevalue values are stored as strings, not ints
            if (int.TryParse(media.Properties[propertyName].Value.ToString(), out propertyValue)) {
                return dataTypeService.GetPreValueAsString(propertyValue);
            }
            return string.Empty;
        }
    }
}