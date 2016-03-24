using NHibernate.Extensions.Data;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Extensions.EventListener
{
    internal class PreDeleteEventListener : IPreDeleteEventListener
    {
        public bool OnPreDelete(PreDeleteEvent @event)
        {
            var entity = @event.Entity as IEntityObject;
            var interceptor = DbContext.GetInterceptor(entity);
            if (interceptor != null)
                return interceptor.OnDelete(entity);
            return false;
        }
    }
}
