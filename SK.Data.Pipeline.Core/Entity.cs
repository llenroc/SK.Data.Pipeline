using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SK.Data.Pipeline.Core.Common;

namespace SK.Data.Pipeline.Core
{
    public partial class Entity
    {
        #region Properties
        public Dictionary<string, object> Values
        {
            private set;
            get;
        }

        public string[] Columns
        {
            get
            {
                return Values.Keys.ToArray();
            }
        }
        #endregion

        #region Constructor
        public Entity()
            : this(new Dictionary<string, object>())
        { }

        public Entity(Dictionary<string, object> values)
        {
            Values = values;
        }
        #endregion

        #region Get & Set value
        public bool TryGetValue<T>(string key, out T outer)
        {
            outer = default(T);

            if (!Values.ContainsKey(key)) return false;
            if (!(Values[key] is T)) return false;

            outer = (T)Values[key];
            return true;
        }

        public T GetValue<T>(string key)
        {
            return (T)Values[key];
        }

        public void SetValue(string key, object value, bool AutoConvert = true)
        {
            if (AutoConvert && value is string)
            {
                value = (value as string).AutoConvertToObject();
            }

            Values[key] = value;
        }

        public void RemoveColumn(string key)
        {
            if (Values.ContainsKey(key))
            {
                Values.Remove(key);
            }
        }
        #endregion
    }
}
