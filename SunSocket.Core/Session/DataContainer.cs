using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SunSocket.Core.Session
{
    public class DataContainer
    {
        private Dictionary<string, object> dataList;
        public DataContainer()
        {
            dataList = new Dictionary<string, object>();
        }
        public object this[string key]
        {
            get
            {
                return Get(key);
            }
            set {
                Set(key, value);
            }
        }
        public void Set(string key,object value)
        {
            object data;
            if (!dataList.TryGetValue(key, out data))
            {
                dataList.Add(key, value);
            }
            else
            {
                data = value;
            }
        }
        public object Get(string key)
        {
            object value;
            if (!dataList.TryGetValue(key, out value))
            {
                return null;
            }
            return value;
        }
        public void Clear()
        {
            dataList.Clear();
        }
    }
}
