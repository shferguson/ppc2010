using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Reflection;

namespace PPC_2010.Data
{
    public class DataTableConverter<T> : DataTable where T:new()
    {
        Dictionary<string, PropertyInfo> properties = new Dictionary<string, PropertyInfo>();

        public DataTableConverter()
        {
            BuildPropertyList();
            CreateColumns();
        }

        public DataTableConverter(IEnumerable<T> items) : this()
        {
            AddItems(items);
        }

        public void AddItems(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                DataRow row = NewRow();
                foreach (DataColumn col in Columns)
                    row[col] = properties[col.ColumnName].GetValue(item, null);
            }
        }

        public List<T> GetItems()
        {
            List<T> items = new List<T>(Rows.Count);

            for (int i = 0; i < items.Count; i++)
            {
                T item = new T();
                DataRow row = Rows[i];
                foreach (DataColumn col in Columns)
                    properties[col.ColumnName].SetValue(item, row[col], null);
            }

            return items;
        }

        private void CreateColumns()
        {
            foreach (var prop in properties.Values)
                Columns.Add(prop.Name, prop.PropertyType);
        }

        private void BuildPropertyList()
        {
            Type t = typeof(T);
            PropertyInfo[] props = t.GetProperties();
            foreach (var prop in props)
                properties.Add(prop.Name, prop);
        }
    }
}