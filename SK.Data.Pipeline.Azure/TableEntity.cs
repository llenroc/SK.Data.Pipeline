using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SK.Data.Pipeline.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SK.Data.Pipeline.Azure
{
    public class TableEntity : ITableEntity
    {
        private static Regex TemplateRegex = new Regex("##(.*?)##", RegexOptions.Compiled);

        public string ETag
        {
            get
            {
                return _ETag;
            }
            set
            {
                _ETag = value;
            }
        }

        public string PartitionKey { get; set; }
        public string RowKey { get; set; }
        public DateTimeOffset Timestamp { get; set; }

        private IDictionary<string, EntityProperty> _TableValues;
        private string _ETag = "*";

        public TableEntity(Entity entity, string partitionKey, string rowKey)
        {
            _TableValues = new Dictionary<string, EntityProperty>();
            foreach (string key in entity.Values.Keys)
            {
                _TableValues[key] = ConvertToEntityProperty(key, entity.Values[key]);
            }

            PartitionKey = TemplateRegex.Replace(partitionKey, (match) =>
            {
                object value = null;
                if (entity.TryGetValue(match.Groups[1].Value, out value))
                {
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }

                return match.Value;
            });

            RowKey = TemplateRegex.Replace(rowKey, (match) =>
            {
                object value = null;
                if (entity.TryGetValue(match.Groups[1].Value, out value))
                {
                    if (value != null)
                    {
                        return value.ToString();
                    }
                }

                return match.Value;
            });

            Timestamp = new DateTimeOffset(DateTime.Now);
        }

        public void ReadEntity(IDictionary<string, EntityProperty> properties, OperationContext operationContext)
        {
            _TableValues = properties;
        }

        public IDictionary<string, EntityProperty> WriteEntity(OperationContext operationContext)
        {
            return _TableValues;
        }

        public Entity ToEntity()
        {
            Entity entity = new Entity();
            foreach (string key in _TableValues.Keys)
            {
                entity.SetValue(key, _TableValues[key].PropertyAsObject);
            }

            return entity;
        }

        private EntityProperty ConvertToEntityProperty(string key, object value)
        {
            if (value == null) return new EntityProperty((string)null);
            if (value.GetType() == typeof(byte[]))
                return new EntityProperty((byte[])value);
            if (value.GetType() == typeof(bool))
                return new EntityProperty((bool)value);
            if (value.GetType() == typeof(DateTimeOffset))
                return new EntityProperty((DateTimeOffset)value);
            if (value.GetType() == typeof(DateTime))
                return new EntityProperty((DateTime)value);
            if (value.GetType() == typeof(double))
                return new EntityProperty((double)value);
            if (value.GetType() == typeof(Guid))
                return new EntityProperty((Guid)value);
            if (value.GetType() == typeof(int))
                return new EntityProperty((int)value);
            if (value.GetType() == typeof(long))
                return new EntityProperty((long)value);
            if (value.GetType() == typeof(string))
                return new EntityProperty((string)value);
            throw new Exception("This value type" + value.GetType() + " for " + key);
            throw new Exception(string.Format("This value type {0} is not supported for {1}", key));
        }
    }
}
