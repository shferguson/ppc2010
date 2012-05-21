using System.Collections.Generic;
using System.Linq;
using System.Xml;
using umbraco.cms.businesslogic.property;

namespace PPC_2010.Extensions
{
    public static class PropertyExtensions
    {
        public static string GetPrevalueAsString(this Property property)
        {
            int value = 0;
            if (property.Value != null)
                if (int.TryParse(property.Value.ToString(), out value))
                    return umbraco.library.GetPrevalueAsString(value);
            return "";
        }

        public static IEnumerable<string> GetPrevalues(this Property property)
        {
            int dataTypeId = property.PropertyType.DataTypeDefinition.Id;

            var preValues = umbraco.library.GetPrevalues(dataTypeId).OfType<IHasXmlNode>();

            IEnumerable<string> preValueList = 
                preValues
                .Where(n => n.GetNode().Name == "preValues")
                .SelectMany(n => n.GetNode().ChildNodes.OfType<XmlNode>().Where(n1 => n1.Name == "preValue").Select(n2 => n2.InnerText));

            return preValueList;
        }

        #region More complex prevalue getting - unused
#if false

        private static readonly TimeSpan preValueCacheTime = TimeSpan.FromSeconds(60);
        private static Dictionary<int, Tuple<DateTime, object>> preValueCache = new Dictionary<int, Tuple<DateTime, object>>();

        public static T GetPrevalue<T>(this Property property)
        {
            int preValue = 0;
            if (property.Value != null && int.TryParse(property.Value as string, out preValue))
            {
                lock (preValueCache)
                {
                    if (preValueCache.ContainsKey(preValue))
                    {
                        var cached = preValueCache[preValue];
                        if (DateTime.Now - cached.Item1 < preValueCacheTime)
                            return (T)cached.Item2;
                    }
                }

                KeyValuePrevalueEditor editor = property.PropertyType.DataTypeDefinition.DataType.PrevalueEditor as KeyValuePrevalueEditor;
                if (editor != null)
                {
                    T value = (T)editor.Prevalues[preValue];

                    lock (preValueCache)
                    {
                        preValueCache[preValue] = new Tuple<DateTime, object>(DateTime.Now, (object)value);
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