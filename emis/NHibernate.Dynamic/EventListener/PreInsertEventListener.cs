using NHibernate.Extensions.Data;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Extensions.EventListener
{
    internal class PreInsertEventListener : IPreInsertEventListener
    {
        public bool OnPreInsert(PreInsertEvent @event)
        {
            var entity = @event.Entity as IEntityObject;
            var interceptor = DbContext.GetInterceptor(entity);
            if (interceptor != null)
            {
                if (!interceptor.OnSave(entity))
                {
                    var propertyValues = @event.Persister.GetPropertyValues(entity, @event.Session.ActiveEntityMode);
                    for (var i = 0; i < propertyValues.Length; i++)
                    {
                        @event.State[i] = propertyValues[i];
                    }
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}
