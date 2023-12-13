using BUS.IService;
using DAL.Models;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BUS.Service
{
    public class RepositorySize_BUS : IRepositorySize_BUS
    {
        private IRepositorySize _repositorySize;
        public RepositorySize_BUS()
        {
            _repositorySize = new RepositorySize(new ShoeStoreContext());
        }
        public async Task<bool> Delete(Size entity)
        {
            var size = await Getone(entity.Id);
            if (size == null)
                return false;
            return _repositorySize.Delete(entity);
        }

        public async Task<List<Size>> GetAll()
        {
            return _repositorySize.GetAll().Where(c => !c.Status.Value).ToList();
        }

        public async Task<Size> Getone(int id)
        {
            var size = _repositorySize.GetSize(id);
            if (size == null)
                return null;
            return size.Status.Value ? null : size;
        }

        public async Task<bool> Insert(Size entity)
        {
            entity.Id = 0;
            entity.Status = false;
            if (string.IsNullOrEmpty(entity.Size1))
                return false;
            if (string.IsNullOrEmpty(entity.Locale))
                return false;
            var sizes = await GetAll();
            if (sizes.Any(c => c.Size1 == entity.Size1))
            {
                return false;
            }
            entity.CreatedAt = DateTime.Now;
            return _repositorySize.Insert(entity);
        }

        public Task<int> InsertReturnId(Size entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Size entity)
        {
            var size = await Getone(entity.Id);
            if (size == null)
                return false;
            if (string.IsNullOrEmpty(entity.Size1))
                return false;
            if (string.IsNullOrEmpty(entity.Locale))
                return false;
            entity.ModifiedAt = DateTime.Now;
            return _repositorySize.Update(entity);
        }
    }
}
