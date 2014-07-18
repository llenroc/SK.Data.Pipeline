using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Core
{
    public class EntityModel
    {
        private Dictionary<string, object> _ColumnInfos = new Dictionary<string,object>();
        private List<string> _Columns = new List<string>();

        public string[] Columns
        {
            get
            {
                return _Columns.ToArray();
            }
        }

        public Entity DefaultEntity
        {
            get
            {
                var entity = new Entity();

                foreach (string column in _ColumnInfos.Keys)
                {
                    entity.SetValue(column, _ColumnInfos[column]);
                }

                return entity;
            }
        }

        public EntityModel(params string[] columns)
        {
            _ColumnInfos = new Dictionary<string, object>();
            foreach (string column in columns)
            {
                AddColumn(column);
            }
        }

        public void AddColumn(string columnName, object defaultValue = null)
        {
            _ColumnInfos[columnName] = defaultValue;
            _Columns.Add(columnName);
        }

        public void AddColumn(string columnName, Type type)
        {
            AddColumn(columnName, Activator.CreateInstance(type));
        }

        public Entity ToStandradEntity(Entity entity)
        {
            // remove additional column
            foreach (string column in entity.Columns)
            {
                if (_ColumnInfos.ContainsKey(column)) continue;

                entity.RemoveColumn(column);
            }

            return AddDefaultValue(entity);
        }

        public Entity AddDefaultValue(Entity entity)
        {
            Entity defaultEntity = DefaultEntity;

            foreach (string column in defaultEntity.Columns)
            {
                if (entity.Values.ContainsKey(column)) continue;

                entity.SetValue(column, defaultEntity.GetValue<object>(column));
            }

            return entity;
        }
    }
}
