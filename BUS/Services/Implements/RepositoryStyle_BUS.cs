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
    public class RepositoryStyle_BUS : IRepositoryStyle_BUS
    {
        private IRepositoryStyle _repositoryStyle;
        public RepositoryStyle_BUS()
        {
            _repositoryStyle = new RepositoryStyle(new ShoeStoreContext());
        }
        public async Task<bool> Delete(Style entity)
        {
            var style = await Getone(entity.Id);
            if (style == null)
                return false;
            return _repositoryStyle.Delete(entity);
        }

        public async Task<List<Style>> GetAll()
        {
            return _repositoryStyle.GetAll().Where(c => !c.Status.Value).ToList();
        }

        public async Task<Style> Getone(int id)
        {
            var style = _repositoryStyle.GetStyle(id);
            if (style == null)
                return null;
            return style.Status.Value ? null : style;
        }

        public async Task<bool> Insert(Style entity)
        {
            entity.Id = 0;
            entity.Status = false;
            if (string.IsNullOrEmpty(entity.StyleName))
                return false;
            var styles = await GetAll();
            if (styles.Any(c => c.StyleName == entity.StyleName))
            {
                return false;
            }
            entity.CreatedAt = DateTime.Now;
            return _repositoryStyle.Insert(entity);
        }

        public Task<int> InsertReturnId(Style entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Style entity)
        {
            var style = await Getone(entity.Id);
            if (style == null)
                return false;
            if (string.IsNullOrEmpty(entity.StyleName))
                return false;
            entity.ModifiedAt = DateTime.Now;
            return _repositoryStyle.Update(entity);
        }
    }
}
