﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface IRepository<T>
    {
        IQueryable<T> GetAll();
        bool Insert(T entity);
        int InsertReturnID(T entity);
        bool Update(T entity);
        bool Delete(T entity);
    }
}
