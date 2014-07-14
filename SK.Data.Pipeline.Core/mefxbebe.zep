using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public partial class Entity
    {
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

        public override bool Equals(object obj)
        {
            if (this == obj) return true;

            return DictionaryEqual(this.DictionaryEqual);
        }

        public bool EqualToOther(Entity first, Entity second)
        {
            return DictionaryEqual(first.Values, second.Values);
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
