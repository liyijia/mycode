using NHibernate.Extensions.Data;
using NHibernate.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Extensions.EventListener
{
    internal class PostUpdateEventListener : IPostUpdateEventListener
    {
        public void OnPostUpdate(PostUpdateEvent @event)
        {
            var entity = @event.Entity as IEntityObject;
            var interceptor = DbContext.GetInterceptor(entity);
            if (interceptor != null)
                interceptor.OnUpdate(entity);
        }

    }
}
