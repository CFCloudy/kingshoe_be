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
    public class RepositoryFeature_BUS : IRepositoryFeature_BUS
    {
        private IRepositoryFeature _repositoryFeature;
        public RepositoryFeature_BUS()
        {
            _repositoryFeature = new RepositoryFeature(new ShoeStoreContext());
        }
        public async Task<bool> Delete(Feature entity)
        {
            var feature = await Getone(entity.Id);
            if (feature == null)
                return false;
            return _repositoryFeature.Delete(entity);
        }

        public async Task<List<Feature>> GetAll()
        {
            return _repositoryFeature.GetAll().Where(c => !c.Status.Value).ToList();
        }

        public async Task<Feature> Getone(int id)
        {
            var feature = _repositoryFeature.GetFeature(id);
            if (feature == null)
                return null;
            return feature.Status.Value ? null : feature;
        }

        public async Task<bool> Insert(Feature entity)
        {
            entity.Id = 0;
            entity.Status = false;
            if (string.IsNullOrEmpty(entity.FeatureName))
                return false;
            var features = await GetAll();
            if (features.Any(c => c.FeatureName == entity.FeatureName))
            {
                return false;
            }
            entity.CreatedAt = DateTime.Now;
            return _repositoryFeature.Insert(entity);
        }

        public Task<int> InsertReturnId(Feature entity)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Update(Feature entity)
        {
            var feature = await Getone(entity.Id);
            if (feature == null)
                return false;
            if (string.IsNullOrEmpty(entity.FeatureName))
                return false;
            entity.ModifiedAt = DateTime.Now;
            return _repositoryFeature.Update(entity);
        }
    }
}
