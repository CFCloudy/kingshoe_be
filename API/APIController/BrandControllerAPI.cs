using BUS.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DAL.Models;
using BUS.Service;
using DTO.models;
using System;
using AutoMapper;
using Azure;

namespace API.APIController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandControllerAPI : ControllerBase
    {
        private IRepositoryBrand_BUS _repositoryBrand_BUS;
        private readonly IMapper _mapper;
        public BrandControllerAPI(IRepositoryBrand_BUS repositoryBrand_BUS, IMapper mapper)
        {
            _repositoryBrand_BUS = repositoryBrand_BUS;
            _mapper = mapper;
        }
        [HttpGet()]
        public async Task<List<BrandDTO>> GetBrands()
        {
            var brands = await _repositoryBrand_BUS.GetAll();
            var brandDTOs = new List<BrandDTO>();
            foreach (var item in brands)
            {
                var brand = _mapper.Map<DAL.Models.Brand, BrandDTO>(item);
                brand.Type = "Brand";
                brandDTOs.Add(brand);
            }
            return brandDTOs;
        }
        [HttpGet("GetBrands")]
        public async Task<IActionResult> GetBrands(bool sort = false, int page = 1)
        {
            var brands = await _repositoryBrand_BUS.GetAll();
            if (sort)
            {
                brands = brands.OrderBy(c => c.DisplayOrder).ToList();
            }
            var brandDTOs = new List<BrandDTO>();
            foreach (var item in brands)
            {
                var brand = _mapper.Map<DAL.Models.Brand, BrandDTO>(item);
                brand.Type = "Brand";
                brandDTOs.Add(brand);
            }
            const int pageSize = 12;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = brandDTOs.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            brandDTOs = brandDTOs.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            return Ok(new { brands = brandDTOs, countPage = Math.Ceiling((decimal)recsCount / 12) });
        }
        [HttpGet("GetBrand/{id}")]
        public async Task<IActionResult> GetBrand(int id)
        {
            var brand = await _repositoryBrand_BUS.Getone(id);
            if (brand == null)
            {
                return BadRequest("Không tìm thấy");
            }
            return Ok(brand);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateBrandDTO brand)
        {
            var brandDAL = _mapper.Map<CreateBrandDTO, Brand>(brand);
            var result = await _repositoryBrand_BUS.Insert(brandDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Thêm thành công" });
            }
            return BadRequest("Thêm brand thất bại");
        }

        [HttpPut("editBrand/{id}")]
        public async Task<IActionResult> Edit(int id, CreateBrandDTO brand)
        {
            if (id == 0)
            {
                return BadRequest("Sửa brand thất bại");
            }

            var brandDAL = _mapper.Map<CreateBrandDTO, Brand>(brand);
            brandDAL.Id = id;
            var result = await _repositoryBrand_BUS.Update(brandDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Sửa thành công" });
            }

            return BadRequest("Sửa brand thất bại");
        }

        [HttpDelete("DeleteBrand/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var brandDAL = await _repositoryBrand_BUS.Getone(id);
            if (brandDAL == null)
            {
                return BadRequest("Xoá brand thất bại");
            }
            var result = await _repositoryBrand_BUS.Delete(brandDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Xoá thành công" });
            }
            return BadRequest("Xoá brand thất bại");
        }

    }
}
