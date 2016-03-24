using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Extensions.Attribute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = true)]
    public class TableAttribute : System.ComponentModel.DataAnnotations.Schema.TableAttribute
    {
        public System.Type NamingStrategyType { get; set; }

        public string DbName { get; set; }

        public TableAttribute(string name)
            : base(name)
        {
        }
    }
}
