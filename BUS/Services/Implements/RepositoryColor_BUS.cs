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
    public class RepositoryColor_BUS : IRepositoryColor_BUS
    {
        private IRepositoryColor _repositoryColor;
        public RepositoryColor_BUS()
        {
            _repositoryColor = new RepositoryColor(new ShoeStoreContext());
        }
        public async Task<bool> Delete(Color entity)
        {
            var color = await Getone(entity.Id);
            if (color == null)
                return false;
            return _repositoryColor.Delete(entity);
        }

        public async Task<List<Color>> GetAll()
        {
            return _repositoryColor.GetAll().Where(c => !c.Status.Value).ToList();
        }

        public async Task<Color> Getone(int id)
        {
            var color = _repositoryColor.GetColor(id);
            if (color == null)
                return null;
            return color.Status.Value ? null : color;
        }

        public async Task<bool> Insert(Color entity)
        {
            entity.Id = 0;
            entity.Status = false;
            if (string.IsNullOrEmpty(entity.ColorName))
                return false;
            var colors = await GetAll();
            if (colors.Any(c=>c.ColorName == entity.ColorName))
            {
                 return false;
            }
            entity.CreatedAt = DateTime.Now;
            return _repositoryColor.Insert(entity);
        }

        public Task<int> InsertReturnId(Color entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Color entity)
        {
            var color = await Getone(entity.Id);
            if (color == null)
                return false;
            if (string.IsNullOrEmpty(entity.ColorName))
                return false;
            entity.ModifiedAt = DateTime.Now;
            return _repositoryColor.Update(entity);
        }
    }
}
