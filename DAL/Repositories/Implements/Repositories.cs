using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Models;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Implements
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ShoeStoreContext _context;

        public GenericRepository()
        {
            _context = new ShoeStoreContext();
        }
        public void AddDataCommand(T entity)
        {
            try
            {
                _context.Add<T>(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteDataCommand(T entity)
        {
            try
            {
                _context.Remove<T>(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IQueryable<T> GetAllDataQuery()
        {
            var data = _context.Set<T>().AsNoTracking();
            return data;
        }

        public void UpdateDataCommand(T entity)
        {
            try
            {
                _context.Update<T>(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateDataWithDetachCommand(T entity)
        {
            try
            {
                DetachLocal(entity);
                _context.Update<T>(entity);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DetachLocal(T t)
        {
            var keyValue = GetKey(t);

            if (keyValue != null)
            {
                var local = _context.Set<T>()
                        .Local
                        .FirstOrDefault(entry => GetKey(entry).Equals(keyValue));
                if (local != null)
                {
                    _context.Entry(local).State = EntityState.Detached;
                    _context.Entry(t).State = EntityState.Modified;
                } 
            }
        }

        public virtual Guid? GetKey(T entity)
        {
            try
            {
                var keyName = _context.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties
                        .Select(x => x.Name).Single();

                return (Guid)entity.GetType().GetProperty(keyName).GetValue(entity, null);
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int AddDataCommandReturnId(T entity)
        {
            try
            {
                _context.Add<T>(entity);
                _context.SaveChanges();
                var entityId = entity.GetType().GetProperty("Id").GetValue(entity, null);
                if(entityId != null)
                {
                    return int.Parse(entityId.ToString());
                }
                else
                {
                    return 0;
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
