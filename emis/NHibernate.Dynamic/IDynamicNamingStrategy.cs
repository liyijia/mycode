using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Extensions
{
    public interface IDynamicNamingStrategy
    {
        string Key { get; }
        string BasedOn { get; set; }
        string ClassToTableName(string className);
    }
}
