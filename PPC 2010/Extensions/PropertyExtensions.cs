using System.Collections.Generic;
using System.Linq;
using System.Xml;
using umbraco.cms.businesslogic.property;

namespace PPC_2010.Extensions
{
    public static class PropertyExtensions
    {
        public static string GetPreValueAsString(this Property property)
        {
            int value = 0;
            if (property.Value != null)
                if (int.TryParse(property.Value.ToString(), out value))
                    return umbraco.library.GetPreValueAsString(value);
            return "";
        }

        public static IEnumerable<string> GetPreValues(this Property property)
        {
            int dataTypeId = property.PropertyType.DataTypeDefinition.Id;

            var PreValues = umbraco.library.GetPreValues(dataTypeId).OfType<IHasXmlNode>();

            IEnumerable<string> PreValueList = 
                PreValues
                .Where(n => n.GetNode().Name == "PreValues")
                .SelectMany(n => n.GetNode().ChildNodes.OfType<XmlNode>().Where(n1 => n1.Name == "PreValue").Select(n2 => n2.InnerText));

            return PreValueList;
        }

        #region More complex PreValue getting - unused
#if false

        private static readonly TimeSpan PreValueCacheTime = TimeSpan.FromSeconds(60);
        private static Dictionary<int, Tuple<DateTime, object>> PreValueCache = new Dictionary<int, Tuple<DateTime, object>>();

        public static T GetPreValue<T>(this Property property)
        {
            int PreValue = 0;
            if (property.Value != null && int.TryParse(property.Value as string, out PreValue))
            {
                lock (PreValueCache)
                {
                    if (PreValueCache.ContainsKey(PreValue))
                    {
                        var cached = PreValueCache[PreValue];
                        if (DateTime.Now - cached.Item1 < PreValueCacheTime)
                            return (T)cached.Item2;
                    }
                }

                KeyValuePreValueEditor editor = property.PropertyType.DataTypeDefinition.DataType.PreValueEditor as KeyValuePreValueEditor;
                if (editor != null)
                {
                    T value = (T)editor.PreValues[PreValue];

                    lock (PreValueCache)
                    {
                        PreValueCache[PreValue] = new Tuple<DateTime, object>(DateTime.Now, (object)value);
                    }

                    return value;

                }
            }
            return default(T);
        }
#endif
        #endregion
    }
}