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
    public class RepositoryBrand_BUS : IRepositoryBrand_BUS
    {
        private IRepositoryBrands _repositoryBrand;
        public RepositoryBrand_BUS()
        {
            _repositoryBrand = new RepositoryBrand(new ShoeStoreContext());
        }
        public async Task<bool> Delete(Brand entity)
        {
            var brand = await Getone(entity.Id);
            if (brand == null)
                return false;
            return _repositoryBrand.Delete(entity);
        }

        public async Task<List<Brand>> GetAll()
        {
            return _repositoryBrand.GetAll().Where(c => !c.Status.Value).ToList();
        }

        public async Task<Brand> Getone(int id)
        {
            var brand = _repositoryBrand.GetBrand(id);
            if (brand == null)
                return null;
            return brand.Status.Value ? null :brand;
        }

        public async Task<bool> Insert(Brand entity)
        {
            entity.Id = 0;
            entity.Status = false;
            entity.CreatedAt = DateTime.Now;
            if (string.IsNullOrEmpty(entity.BrandName))
                return false;
            var brands = await GetAll();
            if (brands.Any(c=>c.BrandName == entity.BrandName))
            {
                 return false;
            }
            return _repositoryBrand.Insert(entity);
        }

        public Task<int> InsertReturnId(Brand entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Brand entity)
        {
            var brand = await Getone(entity.Id);
            if (brand == null)
                return false;
            if (string.IsNullOrEmpty(entity.BrandName))
                return false;
            entity.ModifiedAt = DateTime.Now;
            return _repositoryBrand.Update(entity);
        }
    }
}
