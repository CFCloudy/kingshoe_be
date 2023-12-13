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
    public class SizeControllerAPI : ControllerBase
    {
        private IRepositorySize_BUS _repositorySize_BUS;
        private readonly IMapper _mapper;
        public SizeControllerAPI(IRepositorySize_BUS repositorySize_BUS, IMapper mapper)
        {
            _repositorySize_BUS = repositorySize_BUS;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<List<SizeDTO>> GetSizes()
        {
            var sizes = await _repositorySize_BUS.GetAll();
            var sizeDTOs = new List<SizeDTO>();
            foreach (var item in sizes)
            {
                var size = _mapper.Map<DAL.Models.Size, SizeDTO>(item);
                size.Type = "Size";
                sizeDTOs.Add(size);
            }
            return sizeDTOs;
        }
        [HttpGet("GetSizes")]
        public async Task<IActionResult> GetSizes(bool sort = false, int page = 1)
        {
            var sizes = await _repositorySize_BUS.GetAll();
            if (sort)
            {
                sizes = sizes.OrderBy(c => c.DisplayOrder).ToList();
            }
            var sizeDTOs = new List<SizeDTO>();
            foreach (var item in sizes)
            {
                var size = _mapper.Map<DAL.Models.Size, SizeDTO>(item);
                size.Type = "Size";
                sizeDTOs.Add(size);
            }
            const int pageSize = 12;
            if (page < 1)
            {
                page = 1;
            }
            int recsCount = sizeDTOs.Count();
            var pager = new Pager(recsCount, page, pageSize);
            int recSkip = (page - 1) * pageSize;
            sizeDTOs = sizeDTOs.Skip(recSkip)
                .Take(pager.PageSize).ToList();
            return Ok(new { sizes = sizeDTOs, countPage = Math.Ceiling((decimal)recsCount / 12) });
        }
        [HttpGet("GetSize/{id}")]
        public async Task<IActionResult> GetSize(int id)
        {
            var size = await _repositorySize_BUS.Getone(id);
            if (size == null)
            {
                return BadRequest("Không tìm thấy");
            }
            return Ok(size);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateSizeDTO size)
        {
            var sizeDAL = _mapper.Map<CreateSizeDTO, Size>(size);
            var result = await _repositorySize_BUS.Insert(sizeDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Thêm thành công" });
            }
            return BadRequest("Thêm size thất bại");
        }

        [HttpPut("editSize/{id}")]
        public async Task<IActionResult> Edit(int id, CreateSizeDTO size)
        {
            if (id == 0)
            {
                return BadRequest("Sửa size thất bại");
            }

            var sizeDAL = _mapper.Map<CreateSizeDTO, Size>(size);
            sizeDAL.Id = id;
            var result = await _repositorySize_BUS.Update(sizeDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Sửa thành công" });
            }

            return BadRequest("Sửa size thất bại");
        }

        [HttpDelete("DeleteSize/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var sizeDAL = await _repositorySize_BUS.Getone(id);
            if (sizeDAL == null)
            {
                return BadRequest("Xoá size thất bại");
            }
            var result = await _repositorySize_BUS.Delete(sizeDAL);
            if (result)
            {
                return Ok(new { Status = 200, Message = "Xoá thành công" });
            }
            return BadRequest("Xoá size thất bại");
        }
    }
}
