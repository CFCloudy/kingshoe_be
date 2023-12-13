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
    public class RepositoryShoe_BUS : IRepositoryShoe_BUS
    {
        private IRepositoryProduct _repositoryShoe;
        private IRepositoryBrands _repositoryBrand;
        private IRepositoryFeature _repositoryFeature;
        private IRepositoryStyle _repositoryStyle;
        private ShoeStoreContext _shoeStoreContext;
        public RepositoryShoe_BUS()
        {
            _shoeStoreContext = new ShoeStoreContext();
            _repositoryShoe = new RepositoryProduct(_shoeStoreContext);
            _repositoryBrand = new RepositoryBrand(_shoeStoreContext);
            _repositoryFeature = new RepositoryFeature(_shoeStoreContext);
            _repositoryStyle = new RepositoryStyle(_shoeStoreContext);
        }
        public async Task<bool> Delete(Shoe entity)
        {
            var shoe = await Getone(entity.Id);
            if (shoe == null)
                return false;
            return _repositoryShoe.Delete(entity);
        }

        public async Task<List<Shoe>> GetAll()
        {
            return _repositoryShoe.GetAllShoe().Where(c => c.Status.Value != 1).ToList();
        }

        public async Task<Shoe> Getone(int id)
        {
            var shoe = _repositoryShoe.GetShoeById(id);
            if (shoe == null)
                return null;
            return shoe.Status == 1 ? null : shoe;
        }

        public async Task<bool> Insert(Shoe entity)
        {
            var shoes = await GetAll();
            entity.Id = 0;
            entity.Status = 0;
            if (string.IsNullOrEmpty(entity.ProductName))
                return false;
            if (!entity.BrandGroup.HasValue)
                return false;
            var brand = _repositoryBrand.GetBrand(entity.BrandGroup.Value);
            if (brand == null)
                return false;
            if (!entity.StyleGroup.HasValue)
                return false;
            var style = _repositoryStyle.GetStyle(entity.StyleGroup.Value);
            if (style == null)
                return false;
            if (entity.Feature.HasValue)
            {
                var feature = _repositoryFeature.GetFeature(entity.Feature.Value);
                if (feature == null)
                    return false;
            }
            if (shoes.Any(c => c.ProductName == entity.ProductName))
            {
                return false;
            }
            if (!entity.IsHotProduct.HasValue) entity.IsHotProduct = false;
            entity.CreatedAt = DateTime.Now;
            return _repositoryShoe.Insert(entity);
        }

        public async Task<int> InsertReturnId(Shoe entity)
        {
            var shoes = await GetAll();
            entity.Id = 0;
            entity.Status = 0;
            if (string.IsNullOrEmpty(entity.ProductName))
                return -1;
            if (!entity.BrandGroup.HasValue)
                return -1;
            var brand = _repositoryBrand.GetBrand(entity.BrandGroup.Value);
            if (brand == null)
                return -1;
            if (!entity.StyleGroup.HasValue)
                return -1;
            var style = _repositoryStyle.GetStyle(entity.StyleGroup.Value);
            if (style == null)
                return -1;
            if (entity.Feature.HasValue)
            {
                var feature = _repositoryFeature.GetFeature(entity.Feature.Value);
                if (feature == null)
                    return -1;
            }
            if (shoes.Any(c => c.ProductName == entity.ProductName))
            {
                return -1;
            }
            if (!entity.IsHotProduct.HasValue) entity.IsHotProduct = false;
            entity.CreatedAt = DateTime.Now;
            return _repositoryShoe.InsertReturnID(entity);
        }

        public async Task<bool> Update(Shoe entity)
        {
            var shoes = await GetAll();

            var shoe = await Getone(entity.Id);
            if (shoe == null)
                return false;
            if (string.IsNullOrEmpty(entity.ProductName))
                return false;
            if (!entity.BrandGroup.HasValue)
                return false;
            var brand = _repositoryBrand.GetBrand(entity.BrandGroup.Value);
            if (brand == null)
                return false;
            if (!entity.StyleGroup.HasValue)
                return false;
            var style = _repositoryStyle.GetStyle(entity.StyleGroup.Value);
            if (style == null)
                return false;
            if (entity.Feature.HasValue)
            {
                var feature = _repositoryFeature.GetFeature(entity.Feature.Value);
                if (feature == null)
                    return false;
            }
            if (shoes.Any(c => c.ProductName == entity.ProductName && c.Id != entity.Id))
            {
                return false;
            }
            if (!entity.IsHotProduct.HasValue) entity.IsHotProduct = false;
            entity.ModifiedAt = DateTime.Now;
            //entity.Status = 
            return _repositoryShoe.Update(entity);
        }
    }
}
