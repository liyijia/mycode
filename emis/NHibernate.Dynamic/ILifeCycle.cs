using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NHibernate.Extensions
{
    public interface ILifeCycle
    {
        /// <summary>
        /// 在保存时调用
        /// </summary>
        /// <returns>为True时阻止保存操作</returns>
        bool OnSave(IEntityObject entity);

        /// <summary>
        /// 在更新时调用
        /// </summary>
        /// <returns>为True时阻止更新操作</returns>
        bool OnUpdate(IEntityObject entity);

        /// <summary>
        /// 在删除时调用
        /// </summary>
        /// <returns>为True时阻止删除操作</returns>
        bool OnDelete(IEntityObject entity);

        /// <summary>
        /// 在加载时调用
        /// </summary>
        void OnLoad(IEntityObject entity);

        /// <summary>
        /// 在保存完成后调用
        /// </summary>
        /// <param name="entity"></param>
        void Saved(IEntityObject entity);

        /// <summary>
        /// 删除之后
        /// </summary>
        /// <param name="entity"></param>
        void Deleted(IEntityObject entity);
    }

    public interface ILifeCycle<T> : ILifeCycle where T : IEntityObject
    {
        /// <summary>
        /// 在保存时调用
        /// </summary>
        /// <returns>为True时阻止保存操作</returns>
        bool OnSave(T entity);

        /// <summary>
        /// 在更新时调用
        /// </summary>
        /// <returns>为True时阻止更新操作</returns>
        bool OnUpdate(T entity);

        /// <summary>
        /// 在删除时调用
        /// </summary>
        /// <returns>为True时阻止删除操作</returns>
        bool OnDelete(T entity);

        /// <summary>
        /// 在加载时调用
        /// </summary>
        void OnLoad(T entity);

        /// <summary>
        /// 在保存完成后调用
        /// </summary>
        /// <param name="entity"></param>
        void Saved(T entity);

        /// <summary>
        /// 删除之后
        /// </summary>
        /// <param name="entity"></param>
        void Deleted(T entity);

    }

    public abstract class LifeCycle<T> : ILifeCycle<T> where T : IEntityObject
    {
        /// <summary>
        /// 在保存时调用
        /// </summary>
        /// <returns>为True时阻止保存操作</returns>
        public virtual bool OnSave(T entity)
        {
            return false;
        }

        /// <summary>
        /// 在更新时调用
        /// </summary>
        /// <returns>为True时阻止更新操作</returns>
        public virtual bool OnUpdate(T entity)
        {
            return false;
        }

        /// <summary>
        /// 在删除时调用
        /// </summary>
        /// <returns>为True时阻止删除操作</returns>
        public virtual bool OnDelete(T entity)
        {
            return false;
        }

        
        /// <summary>
        /// 在加载时调用
        /// </summary>
        public virtual void OnLoad(T entity)
        {
        }

        /// <summary>
        /// 在保存完成后调用
        /// </summary>
        public virtual void Saved(T entity)
        {

        }

        /// <summary>
        /// 在删除完成时调用
        /// </summary>
        public virtual void Deleted(T entity)
        {

        }

        bool ILifeCycle.OnSave(IEntityObject entity)
        {
            return this.OnSave((T)entity);
        }

        bool ILifeCycle.OnUpdate(IEntityObject entity)
        {
            return this.OnUpdate((T)entity);
        }

        bool ILifeCycle.OnDelete(IEntityObject entity)
        {
            return this.OnDelete((T)entity);
        }

        void ILifeCycle.OnLoad(IEntityObject entity)
        {
            this.OnLoad((T)entity);
        }

        void ILifeCycle.Saved(IEntityObject entity)
        {
            this.Saved((T)entity);
        }

        void ILifeCycle.Deleted(IEntityObject entity)
        {
            this.Deleted((T)entity);
        }
    }

}
