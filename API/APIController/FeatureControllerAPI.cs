using AutoMapper;
using BUS.IService;
using BUS.Service;
using DAL.Models;
using DTO.models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeatureControllerAPI : ControllerBase
    {
        private IRepositoryFeature_BUS _repositoryFeature_BUS;
        private readonly IMapper _mapper;
        public FeatureControllerAPI(IRepositoryFeature_BUS repositoryFeature_BUS, IMapper mapper)
        {
            _repositoryFeature_BUS = repositoryFeature_BUS;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<List<FeatureDTO>> GetFeatures()
        {
            var features = await _repositoryFeature_BUS.GetAll();
            var featureDTOs = new List<FeatureDTO>();
            foreach (var item in features)
            {
                var feature = _mapper.Map<DAL.Models.Feature, FeatureDTO>(item);
                feature.Type = "Feature";
                featureDTOs.Add(feature);
            }
            return featureDTOs;
        }
        [HttpGet("GetFeatures")]
        public async Task<IActionResult> GetFeatures(bool sort = false, int page = 1)
        {
            var features = await _repositoryFeature_BUS.GetAll();
            if (sort)
            {
                features = features.OrderBy(c => c.DisplayOrder).ToList();
            }
            var featureDTOs = new List<FeatureDTO>();
            foreach (var item in features)
            {
                var feature = _mapper.Map<DAL.Models.Feature, FeatureDTO>(item);
                feature.Type = "Feature";
                featureDTOs.Add(feature);
            }
            const int pageSize = 12;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = featureDTOs.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            featureDTOs = featureDTOs.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            return Ok(new { features = featureDTOs, countPage = Math.Ceiling((decimal)recsCount / 12) });
        }
        [HttpGet("GetFeature/{id}")]
        public async Task<IActionResult> GetFeature(int id)
        {
            var feature = await _repositoryFeature_BUS.Getone(id);
            if (feature == null)
            {
                return BadRequest("Không tìm thấy");
            }
            return Ok(feature);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateFeatureDTO feature)
        {
            var featureDAL = _mapper.Map<CreateFeatureDTO, Feature>(feature);
            var result = await _repositoryFeature_BUS.Insert(featureDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Thêm thành công" });
            }
            return BadRequest("Thêm feature thất bại");
        }

        [HttpPut("editFeature/{id}")]
        public async Task<IActionResult> Edit(int id, CreateFeatureDTO feature)
        {
            if (id == 0)
            {
                return BadRequest("Sửa feature thất bại");
            }

            var featureDAL = _mapper.Map<CreateFeatureDTO, Feature>(feature);
            featureDAL.Id = id;
            var result = await _repositoryFeature_BUS.Update(featureDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Sửa thành công" });
            }

            return BadRequest("Sửa feature thất bại");
        }

        [HttpDelete("DeleteFeature/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var featureDAL = await _repositoryFeature_BUS.Getone(id);
            if (featureDAL == null)
            {
                return BadRequest("Xoá feature thất bại");
            }
            var result = await _repositoryFeature_BUS.Delete(featureDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Xoá thành công" });
            }
            return BadRequest("Xoá feature thất bại");
        }
    }
}
