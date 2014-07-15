using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public partial class Entity
    {
        #region Constant values
        public const string DefaultColumn = "Content";
        public const string DefaultColumnsTemplate = "Column{0}";
        public const string DefaultSeparator = "\t,\t";
        #endregion

        #region Extra methods for Entity
        public bool IsEmpty()
        {
            return Values.Keys.Count() == 0;
        }

        public void Extend(Entity item)
        {
            foreach (string key in item.Values.Keys)
            {
                if (Values.ContainsKey(key)) continue;

                Values[key] = item.Values[key];
            }
        }

        public bool EqualsOther(object obj)
        {
            if (this == obj) return true;
            if (obj == null || !(obj is Entity)) return false;

            return DictionaryEqual(this.Values, (obj as Entity).Values);
        }
        #endregion

        #region private methods
        private bool DictionaryEqual<TKey, TValue>(IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            var comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!comparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }
        #endregion
    }
}
