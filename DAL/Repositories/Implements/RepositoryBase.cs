using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Implements
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        protected readonly ShoeStoreContext Context;
        protected readonly DbSet<T> DbSet;

        public RepositoryBase(ShoeStoreContext context)
        {
            Context = context;
            DbSet = context.Set<T>();
        }
        public IQueryable<T> GetAll()
        {
            return DbSet;
        }
        public virtual bool Insert(T entity)
        {
            var result = 0;
            try
            {
                DbSet.Add(entity);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }

            return result > 0;
        }

        public virtual bool Delete(T entity)
        {
            DbSet.Remove(entity);
            return Context.SaveChanges() > 0;
        }

        public virtual bool Update(T entity)
        {
            var result = 0;
            try
            {
                DbSet.Update(entity);
                result = Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }

            return result > 0;
        }

        public int InsertReturnID(T entity)
        {
            try
            {
                DbSet.Add(entity);
                Context.SaveChanges();
                var entityId = entity.GetType().GetProperty("Id").GetValue(entity, null);
                if (entityId != null)
                {
                    return int.Parse(entityId.ToString());
                }
                else
                {
                    return -1;
                }

                // Sau khi lưu thay đổi, entity sẽ có ID được cập nhật bởi cơ sở dữ liệu.
                // Giả sử bạn có một thuộc tính Id trong đối tượng T.
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
