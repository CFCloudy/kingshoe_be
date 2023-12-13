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
    public class Repositories_dal<T> : Interfaces.Repositories_dal<T> where T : class
    {
        protected readonly ShoeStoreContext _Context;
        protected readonly DbSet<T> _DbSet;

        public Repositories_dal(ShoeStoreContext _context)
        {
            _Context = _context;
            _DbSet = _context.Set<T>();
        }
        public virtual bool Delete(T entity)
        {
            _DbSet.Remove(entity);
            return _Context.SaveChanges() > 0;
        }

        public IQueryable<T> GetAll()
        {
            return _DbSet;
        }

        public virtual bool Insert(T entity)
        {
            var _result = 0;
            try
            {
                _DbSet.Add(entity);
                _result = _Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }

            return _result > 0;
        }

        public virtual bool Update(T entity)
        {
            var _result = 0;
            try
            {
                _DbSet.Update(entity);
                _result = _Context.SaveChanges();
            }
            catch (Exception dbEx)
            {
            }

            return _result > 0;
        }
    }
}
