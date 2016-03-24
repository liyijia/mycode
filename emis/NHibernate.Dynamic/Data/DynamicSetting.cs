using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using NHibernate.Extensions.Attribute;
using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Collections;

namespace NHibernate.Extensions.Data
{
    internal class DynamicSetting
    {
        public System.Type EntityType { get; set; }

        public string DbName { get; set; }

        public string TableName { get; set; }

        public IDynamicNamingStrategy NamingStrategy { get; set; }

        public ILifeCycle Interceptor { get; set; }
    }

    internal class DynamicSettingCollection : ICollection
    {
        private readonly ConcurrentDictionary<string, DynamicSetting> _Settings = new ConcurrentDictionary<string, DynamicSetting>();

        public DynamicSetting this[string typeName]
        {
            get
            {
                DynamicSetting value = null;
                _Settings.TryGetValue(typeName, out value);
                return value;
            }
            set
            {
                _Settings.AddOrUpdate(typeName, value, (k, v) => value);
            }
        }

        public string[] Keys { get { return _Settings.Keys.ToArray(); } }

        public DynamicSetting[] Values { get { return _Settings.Values.ToArray(); } }

        public DynamicSetting LoadFrom(System.Type entityType)
        {
            if (_Settings.ContainsKey(entityType.ToString()))
                return this[entityType.ToString()];
            var attrTable = entityType.GetCustomAttributes(typeof(TableAttribute), true).FirstOrDefault() as TableAttribute;
            var setting = new DynamicSetting
            {
                DbName = attrTable == null ? null : attrTable.DbName,
                NamingStrategy = attrTable == null ? null : attrTable.NamingStrategyType.InvokeMember("Instance", BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static, null, null, null) as IDynamicNamingStrategy,
                Interceptor = null,
                TableName = attrTable == null ? null : attrTable.Name,
                EntityType = entityType
            };
            this[setting.EntityType.ToString()] = setting;

            return setting;
        }

        public void CopyTo(Array array, int index)
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return _Settings.Count; }
        }

        public bool IsSynchronized
        {
            get { return true; }
        }

        public object SyncRoot
        {
            get { return string.Empty; }
        }

        public IEnumerator GetEnumerator()
        {
            foreach (var setting in _Settings)
            {
                yield return setting.Value;
            }
        }
    }

    internal static class DynamicSettingHelper
    {
        private static readonly DynamicSettingCollection dynamicSettings = new DynamicSettingCollection();

        public static DynamicSetting LoadFrom(System.Type entityType)
        {
            return dynamicSettings.LoadFrom(entityType);
        }

        public static DynamicSettingCollection Settings
        {
            get
            {
                return dynamicSettings;
            }
        }
    }
}
