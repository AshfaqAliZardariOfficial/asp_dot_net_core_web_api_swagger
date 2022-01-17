using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Testing.Models;

namespace Testing.Utils
{
    public class ModelDictionaryMapping<TObject>
    {

        public static List<T> GetModelList<T>(DataTable dt)
        {
            List<T> data = new List<T>();
            if (dt != null)
            {
                foreach (DataRow row in dt.Rows)
                {
                    T item = GetItem<T>(row);
                    data.Add(item);
                }
            }
            return data;
        }
        private static T GetItem<T>(DataRow dr)
        {
            Type temp = typeof(T);
            T obj = Activator.CreateInstance<T>();

            foreach (DataColumn column in dr.Table.Columns)
            {
                foreach (PropertyInfo pro in temp.GetProperties())
                {
                    if (pro.Name == column.ColumnName)
                        pro.SetValue(obj, Convert.IsDBNull(dr[column.ColumnName]) ? null : dr[column.ColumnName], null);
                    else
                        continue;
                }
            }
            return obj;
        }

        public static IDictionary<string, object> GetDictionary(TObject o)
        {
            IDictionary<string, object> res = new Dictionary<string, object>();
            PropertyInfo[] props = typeof(TObject).GetProperties();
            for (int i = 0; i < props.Length; i++)
            {
                if (props[i].CanRead)
                {
                    res.Add(props[i].Name, props[i].GetValue(o, null));
                }
            }
            return res;
        }
    }
}
