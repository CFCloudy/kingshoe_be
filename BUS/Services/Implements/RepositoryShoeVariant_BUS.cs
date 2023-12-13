using BUS.IService;
using DAL.Models;
using DAL.Repositories.Implements;
using DAL.Repositories.Interfaces;
using Newtonsoft.Json;

namespace BUS.Service
{
    public class RepositoryShoeVariant_BUS : IRepositoryShoeVariant_BUS
    {
        private IRepositotyProductVariant _repositotyShoesVariant;
        private IRepositoryProduct _repositoryShoe;
        private IRepositorySize _repositorySize;
        private IRepositoryColor _repositoryColor;
        private ShoeStoreContext _shoeStoreContext;
        public RepositoryShoeVariant_BUS()
        {
            _shoeStoreContext = new ShoeStoreContext();
            _repositotyShoesVariant = new RepositoryProductVariant(_shoeStoreContext);
            _repositoryShoe = new RepositoryProduct(_shoeStoreContext);
            _repositoryColor = new RepositoryColor(_shoeStoreContext);
            _repositorySize = new RepositorySize(_shoeStoreContext);
        }
        public async Task<bool> Delete(ShoesVariant entity)
        {
            var shoesVariant = await Getone(entity.Id);
            if (shoesVariant == null)
                return false;
            return _repositotyShoesVariant.Delete(entity);
        }

        public async Task<List<ShoesVariant>> GetAll()
        {
            return _repositotyShoesVariant.GetAllShoeVariant().Where(c => c.Status.Value != 1).ToList();
        }

        public async Task<ShoesVariant> Getone(int id)
        {
            var shoesVariant = _repositotyShoesVariant.GetShoesVariant(id);
            if (shoesVariant == null)
                return null;
            return shoesVariant.Status == 1 ? null : shoesVariant;
        }

        public async Task<List<ShoesVariant>> GetShoesVariantsByIdShoe(int idShoe)
        {
            var shoesVariant = _repositotyShoesVariant.GetAllShoeVariant().Where(c => c.Status.Value != 1 && c.ProductId == idShoe).ToList();

            return shoesVariant;
        }

        public async Task<bool> Insert(ShoesVariant entity)
        {
            entity.Id = 0;
            entity.Status = 0;
            if (!entity.ProductId.HasValue)
                return false;
            var shoesVariants = await GetShoesVariantsByIdShoe(entity.ProductId.Value);
            var shoe = _repositoryShoe.GetShoeById(entity.ProductId.Value);
            if (!entity.Size.HasValue)
                return false;
            var size = _repositorySize.GetSize(entity.Size.Value);
            if (size == null)
                return false;
            if (!entity.Color.HasValue)
                return false;
            var color = _repositorySize.GetSize(entity.Color.Value);
            if (color == null)
                return false;
            if (!entity.Stock.HasValue)
                return false;
            if (shoesVariants.Any(c => c.Color == entity.Color && c.Size == entity.Size))
            {
                return false;
            }
            var sizeName = _repositorySize.GetSize(entity.Size.Value).Size1;
            var colorName = _repositoryColor.GetColor(entity.Color.Value).ColorName;
            entity.CreatedAt = DateTime.Now;
            entity.VariantName = shoe.ProductName + " " + colorName + " " + sizeName;
            var imageEntity = new List<int>();
            if (entity.ImageId != null)
            {
                imageEntity = JsonConvert.DeserializeObject<List<int>>(entity.ImageId);
            }
            foreach (var item in shoesVariants.Where(c => c.Color == entity.Color))
            {
                if (entity.ImageId == item.ImageId)
                {
                    continue;
                }
                var images = new List<int>();
                if (item.ImageId != null)
                {
                    images = JsonConvert.DeserializeObject<List<int>>(item.ImageId);
                }
                if (imageEntity.Any())
                {
                    images.AddRange(imageEntity);
                    images = images.Distinct().ToList();
                }
                imageEntity = images;
                entity.ImageId = JsonConvert.SerializeObject(imageEntity);
                item.ImageId = JsonConvert.SerializeObject(images);
                _repositotyShoesVariant.Update(item);
            }
            return _repositotyShoesVariant.Insert(entity);
        }

        public Task<int> InsertReturnId(ShoesVariant entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(ShoesVariant entity)
        {
            var shoesVariant = await Getone(entity.Id);
            if (shoesVariant == null)
                return false;
            if (!entity.ProductId.HasValue)
                return false;
            var shoe = _repositoryShoe.GetShoeById(entity.ProductId.Value);
            var shoesVariants = await GetShoesVariantsByIdShoe(entity.ProductId.Value);
            if (!entity.Size.HasValue)
                return false;
            var size = _repositorySize.GetSize(entity.Size.Value);
            if (size == null)
                return false;
            if (!entity.Color.HasValue)
                return false;
            var color = _repositorySize.GetSize(entity.Color.Value);
            if (color == null)
                return false;
            if (!entity.Stock.HasValue)
                return false;
            if (shoesVariants.Any(c => c.Color == entity.Color && c.Size == entity.Size && c.Id != entity.Id))
            {
                return false;
            }
            var sizeName = _repositorySize.GetSize(entity.Size.Value).Size1;
            var colorName = _repositoryColor.GetColor(entity.Color.Value).ColorName;
            entity.VariantName = shoe.ProductName + " " + colorName + " " + sizeName;
            entity.ModifiedAt = DateTime.Now;

            foreach (var item in shoesVariants.Where(c => c.Color == entity.Color && c.Id != entity.Id))
            {
                var shoevariant = _shoeStoreContext.ShoesVariants.FirstOrDefault(c=>c.Id == item.Id);
                shoevariant.ImageId = entity.ImageId;
                _repositotyShoesVariant.Update(shoevariant);
            }
            return _repositotyShoesVariant.Update(entity);
        }
        public async Task<bool> UpdateImage(ShoesVariant entity)
        {
            return _repositotyShoesVariant.Update(entity);
        }
    }
}
